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
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingMVC;
using kbs2.WorldEntity.Unit.MVC;
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
        public List<IViewable> DrawList = new List<IViewable>();
        // List for drawing items without offset
        public List<IViewable> DrawGuiList = new List<IViewable>();

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

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //TESTCODE
            DBController.OpenConnection("DefDex");
            BuildingDef def = DBController.GetDefinitionBuilding(1);
            DBController.CloseConnection();

            Building_Controller building = BuildingFactory.CreateNewBuilding(def, new Coords { x = 0, y = 0 });
            gameModel.World.AddBuilding(def, building);
			//TESTCODE
			

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

            // ============== Temp Code ===================================================================
            // Update Buildings on screen
            List<IViewable> buildings = new List<IViewable>();
            foreach(Building_Controller building in gameModel.World.WorldModel.buildings)
            {
                buildings.Add(building.View);
            }
            gameModel.ItemList.AddRange(buildings);

            List<IViewable> Cells = new List<IViewable>();
            foreach(KeyValuePair<Coords, WorldChunkController> chunk in gameModel.World.WorldModel.ChunkGrid)
            {
                foreach(WorldCellController cell in chunk.Value.WorldChunkModel.grid)
                {
                    Cells.Add(cell.worldCellView);
                }
            }
            gameModel.ItemList.AddRange(Cells);

			// Hard Coded Unit
			Unit_Controller unit = new Unit_Controller(gameModel.World.WorldModel, "pikachu_idle", 2, 2, 0, 0 );

			gameModel.ItemList.Add(unit.UnitView);

			// ======================================================================================

			gameModel.Selection.DrawSelectionBox(gameModel.World.WorldModel.Units, Mouse.GetState(), Camera.GetViewMatrix(), TileSize, Camera.Zoom);

            /*gameModel.Selection.DrawHorizontalLine((int) gameModel.Selection.View.Coords.y);
            gameModel.Selection.DrawHorizontalLine((int) (gameModel.Selection.View.Coords.y + gameModel.Selection.View.Height));
            gameModel.Selection.DrawVerticalLine((int) gameModel.Selection.View.Coords.x);
            gameModel.Selection.DrawVerticalLine((int) (gameModel.Selection.View.Coords.x + gameModel.Selection.View.Width));

            for (int i = 0; i < gameModel.Selection.Model.Box.Count; i++){
                DrawGuiList.Add(gameModel.Selection.Model.Box[i]);
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

            // Updates everything on screen
            UpdateOnScreen();

            DrawNonGui();
            
            // Temp Code ==========================
            spriteBatch.Begin();

            // Is niets niet kijken
            DrawHorizontalLine((int) gameModel.Selection.View.Coords.y);
            DrawHorizontalLine((int) (gameModel.Selection.View.Coords.y + gameModel.Selection.View.Height));
            DrawVerticalLine((int)gameModel.Selection.View.Coords.x);
            DrawVerticalLine((int) (gameModel.Selection.View.Coords.x + gameModel.Selection.View.Width));

            spriteBatch.End();

            // End temp code ==========================================
            
            DrawGui();

            // Calls the game's draw function
            base.Draw(gameTime);
        }

        // Draws every item in the DrawList with camera offset
        private void DrawNonGui()
        {
            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            foreach (IViewable DrawItem in DrawList)
            {
                if (DrawItem == null) continue;
                Texture2D texture = this.Content.Load<Texture2D>(DrawItem.Texture);
                spriteBatch.Draw(texture, new Rectangle((int)(DrawItem.Coords.x * TileSize), (int)(DrawItem.Coords.y * TileSize), (int)(DrawItem.Width * TileSize), (int)(DrawItem.Height * TileSize)), DrawItem.Color);
            }

            spriteBatch.End();
        }

        // Draws every item in the DrawList without offset
        private void DrawGui()
        {
            spriteBatch.Begin();

            foreach (IViewable DrawItem in DrawGuiList)
            {
                Texture2D texture = this.Content.Load<Texture2D>(DrawItem.Texture);
                spriteBatch.Draw(texture, new Rectangle((int)DrawItem.Coords.x, (int)DrawItem.Coords.y, (int)(DrawItem.Width * TileSize), (int)(DrawItem.Height * TileSize)), DrawItem.Color);
            }

            spriteBatch.End();
        }

        // Returns everything that is in the view
        public void UpdateOnScreen()
        {
            DrawList.Clear();
            DrawGuiList.Clear();

            Vector2 TopLeft = Vector2.Transform(new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y), Camera.GetInverseViewMatrix());

            Vector2 BottomRight = Vector2.Transform(new Vector2(GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height), Camera.GetInverseViewMatrix());

            foreach (var item in gameModel.ItemList)
            {
                if (item.Coords.x < (TopLeft.X / TileSize) - item.Width || item.Coords.y < (TopLeft.Y / TileSize) - item.Height || item.Coords.x > BottomRight.X / TileSize || item.Coords.y > BottomRight.Y / TileSize) continue;
                DrawList.Add(item);
            }

            DrawList = (from Item in DrawList
                        orderby Item.ZIndex ascending
                        select Item).ToList();

            foreach (var item in gameModel.GuiItemList)
            {
                if (item.Coords.x < (TopLeft.X / TileSize) - item.Width || item.Coords.y < (TopLeft.Y / TileSize) - item.Height || item.Coords.x > BottomRight.X / TileSize || item.Coords.y > BottomRight.Y / TileSize) continue;
                DrawGuiList.Add(item);
            }

            DrawGuiList = (from Item in DrawGuiList
                              orderby Item.ZIndex ascending
                        select Item).ToList();

            gameModel.ItemList.Clear();
            gameModel.GuiItemList.Clear();
        }

        // ====================================================================================================== V


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
