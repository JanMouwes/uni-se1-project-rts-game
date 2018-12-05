using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Desktop.View.Camera;
using kbs2.Desktop.World.World;
using kbs2.GamePackage.Interfaces;
using kbs2.GamePackage.Selection;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.Structs;
using kbs2.World.TerrainDef;
using kbs2.World.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace kbs2.GamePackage
{
    public class GameView : Game
    {
        private GameModel gameModel;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private CameraController Camera;

        // List for drawing items with the camera offset
        private List<IViewable> DrawList;
        // List for drawing items without offset
        private List<IViewable> DrawStaticList;

        // Calculate the size (Width) of a tile
        public int TileSize => (int)(GraphicsDevice.Viewport.Width / Camera.CameraModel.TileCount);

        // Constructor
        public GameView(GameModel gameModel)
        {
            this.gameModel = gameModel;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Allows the user to resize the window
            base.Window.AllowUserResizing = true;

            // Makes the mouse visible in the window
            base.IsMouseVisible = true;

            // Initalize game
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // initialize camera
            Camera = new CameraController(GraphicsDevice);

            // Initializes the lists that hold the views to draw
            DrawList = new List<IViewable>();
            DrawStaticList = new List<IViewable>();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Exit game if escape is pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Updates camera according to the pressed buttons
            Camera.MoveCamera();

            // Updates cells on screen ================================================================================= <>
            GetChunksOnScreen();

            gameModel.Selection.DrawSelectionBox(gameModel.World.WorldModel.Units, Mouse.GetState(), Camera.GetViewMatrix(), TileSize, Camera.Zoom);

            /*gameModel.Selection.DrawHorizontalLine((int) gameModel.Selection.View.Coords.y);
            gameModel.Selection.DrawHorizontalLine((int) (gameModel.Selection.View.Coords.y + gameModel.Selection.View.Height));
            gameModel.Selection.DrawVerticalLine((int) gameModel.Selection.View.Coords.x);
            gameModel.Selection.DrawVerticalLine((int) (gameModel.Selection.View.Coords.x + gameModel.Selection.View.Width));

            for (int i = 0; i < gameModel.Selection.Model.Box.Count; i++){
                DrawStaticList.Add(gameModel.Selection.Model.Box[i]);
            }*/
            
            // Calls the game update
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clears the GraphicsDevice to make room for the new draw items
            GraphicsDevice.Clear(Color.Black);

            DrawMovable();

            spriteBatch.Begin();

            // Is niets niet kijken
            DrawHorizontalLine((int) gameModel.Selection.View.Coords.y);
            DrawHorizontalLine((int) (gameModel.Selection.View.Coords.y + gameModel.Selection.View.Height));
            DrawVerticalLine((int)gameModel.Selection.View.Coords.x);
            DrawVerticalLine((int) (gameModel.Selection.View.Coords.x + gameModel.Selection.View.Width));

            spriteBatch.End();

            DrawStationairy();

            // Calls the game's draw function
            base.Draw(gameTime);
        }

        // Draws every item in the DrawList with camera offset
        private void DrawMovable()
        {
            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            foreach (IViewable DrawItem in DrawList)
            {
                if (DrawItem == null) continue;
                Texture2D texture = this.Content.Load<Texture2D>(DrawItem.Texture);
                spriteBatch.Draw(texture, new Rectangle((int)DrawItem.Coords.x * TileSize, (int)DrawItem.Coords.y * TileSize, (int)(DrawItem.Width * TileSize), (int)(DrawItem.Height * TileSize)), DrawItem.Color);
            }

            spriteBatch.End();
        }

        // Draws every item in the DrawList without offset
        private void DrawStationairy()
        {
            spriteBatch.Begin();

            foreach (IViewable DrawItem in DrawStaticList)
            {
                Texture2D texture = this.Content.Load<Texture2D>(DrawItem.Texture);
                spriteBatch.Draw(texture, new Rectangle((int)DrawItem.Coords.x, (int)DrawItem.Coords.y, (int)(DrawItem.Width * TileSize), (int)(DrawItem.Height * TileSize)), DrawItem.Color);
            }

            spriteBatch.End();
        }

        // ====================================================================================================== V

        // Returns everything that is in the view
        public List<IViewable> GetOnScreen(List<IViewable> totalList, Viewport viewport, Matrix inverseMatrix)
        {
            List<IViewable> drawList = new List<IViewable>();
            
            Vector2 TopLeft = Vector2.Transform(new Vector2(viewport.X, viewport.Y), inverseMatrix);
            
            Vector2 BottomRight = Vector2.Transform(new Vector2(viewport.X + viewport.Width, viewport.Y + viewport.Height), inverseMatrix);

            foreach (var item in totalList)
            {
                if ( item.Coords.x < (TopLeft.X / TileSize) - 1 || item.Coords.y < (TopLeft.Y / TileSize) - 1 || item.Coords.x > BottomRight.X / TileSize || item.Coords.y > BottomRight.Y / TileSize ) continue;
                drawList.Add(item);
            }

            return drawList;
        }

        // Gets the cells that are in the view 
        public void GetCellsOnScreen()
        {
            Vector2 CameraPosition = new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);
            Vector2 realCameraPosition = Vector2.Transform(CameraPosition, Camera.GetInverseViewMatrix());

            Vector2 CameraBottomPosition = new Vector2(GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height);
            Vector2 RealCameraPos2 = Vector2.Transform(CameraBottomPosition, Camera.GetInverseViewMatrix());

            DrawList.Clear();

            foreach (var Chunk in gameModel.World.WorldModel.ChunkGrid)
            {
                foreach (var item2 in Chunk.Value.WorldChunkModel.grid)
                {
                    if (item2.worldCellView.Coords.x < (realCameraPosition.X / TileSize) - 1
                        || item2.worldCellView.Coords.y < (realCameraPosition.Y / TileSize) - 1
                        || item2.worldCellView.Coords.x > RealCameraPos2.X / TileSize
                        || item2.worldCellView.Coords.y > RealCameraPos2.Y / TileSize
                        ) continue;
                    DrawList.Add(item2.worldCellView);
                }
            }
        }

        // Gets the chunks that are in the view 
        public void GetChunksOnScreen()
        {
            Vector2 CameraPosition = new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);
            Vector2 realCameraPosition = Vector2.Transform(CameraPosition, Camera.GetInverseViewMatrix());

            Vector2 CameraBottomPosition = new Vector2(GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height);
            Vector2 RealCameraPos2 = Vector2.Transform(CameraBottomPosition, Camera.GetInverseViewMatrix());

            DrawList.Clear();

            foreach (var Chunk in gameModel.World.WorldModel.ChunkGrid)
            {
                if (Chunk.Key.x < (realCameraPosition.X / TileSize / WorldChunkModel.ChunkSize) - 1
                        || Chunk.Key.y < (realCameraPosition.Y / TileSize / WorldChunkModel.ChunkSize) - 1
                        || Chunk.Key.x > RealCameraPos2.X / TileSize / WorldChunkModel.ChunkSize
                        || Chunk.Key.y > RealCameraPos2.Y / TileSize / WorldChunkModel.ChunkSize
                        ) continue;

                foreach (var item2 in Chunk.Value.WorldChunkModel.grid)
                {
                    DrawList.Add(item2.worldCellView);
                }
            }
        }

        public void DrawHorizontalLine(int PositionY)
        {
            Texture2D texture = Content.Load<Texture2D>(gameModel.Selection.View.Texture);
            if (gameModel.Selection.View.Width > 0)
            {
                for (int i = 0; i <= gameModel.Selection.View.Width - 10; i += 10)
                {
                    if (gameModel.Selection.View.Width - i >= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle((int) (gameModel.Selection.View.Coords.x + i), PositionY, 10, 5),
                            Color.White);
                    }
                }
            }
            else if (gameModel.Selection.View.Width < 0)
            {
                for (int i = -10; i >= gameModel.Selection.View.Width; i -= 10)
                {
                    if (gameModel.Selection.View.Width - i <= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle((int) (gameModel.Selection.View.Coords.x + i), PositionY, 10, 5),
                            Color.White);
                    }
                }
            }
        }

        public void DrawVerticalLine(int PositionX)
        {
            Texture2D texture = Content.Load<Texture2D>(gameModel.Selection.View.Texture);
            if (gameModel.Selection.View.Height > 0)
            {
                for (int i = -2; i <= gameModel.Selection.View.Height; i += 10)
                {
                    if (gameModel.Selection.View.Height - i >= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle(PositionX, (int) (gameModel.Selection.View.Coords.y + i), 10, 5),
                            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, MathHelper.ToRadians(90),
                            new Vector2(0, 0), SpriteEffects.None, 0);
                    }
                }
            }

            else if (gameModel.Selection.View.Height < 0)
            {
                for (int i = 0; i >= gameModel.Selection.View.Height; i -= 10)
                {
                    if (gameModel.Selection.View.Height - i <= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle(PositionX - 10, (int) (gameModel.Selection.View.Coords.y + i), 10, 5),
                            Color.White);
                    }
                }
            }
        }
    }
}
