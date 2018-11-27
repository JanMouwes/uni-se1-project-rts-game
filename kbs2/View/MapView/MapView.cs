using System;
using System.Collections.Generic;
using kbs2.Desktop.View.Camera;
using kbs2.Desktop.View.EventArgs;
using kbs2.Desktop.World.World;
using kbs2.GamePackage;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.World;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace kbs2.Desktop.View.MapView
{
    public delegate void ZoomObserver(object sender, ZoomEventArgs eventArgs);

    public class MapView : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private CameraController Camera;
        private WorldController World;
        private Selection_Controller Selection;
        private Unit_View Pichu = new Unit_View("unitview", 1, 1);

        // Calculate the size (Width) of a tile
        public int TileSize => (int) (GraphicsDevice.Viewport.Width / Camera.CameraModel.TileCount);

        // Calculates the height of a cell
        public int CellHeight => (int) (Camera.CameraModel.TileCount / GraphicsDevice.Viewport.AspectRatio);

        public MapView()
        {
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
            // Add your initialization logic here

            // initialize world
            World = WorldFactory.GetNewWorld();

            // initialize camera
            Camera = new CameraController(GraphicsDevice);

            // initialize selection
            Selection = new Selection_Controller("PurpleLine", Mouse.GetState());

            // Allows the user to resize the window
            base.Window.AllowUserResizing = true;

            // Makes the mouse visible in the window
            base.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Camera.MoveCamera();

            Selection.DrawSelectionBox(Mouse.GetState());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            DrawCells();

            DrawSelection();

            DrawUnits();

            base.Draw(gameTime);
        }

        public delegate Color TileColourDelegate(WorldCellModel cell);

        public TileColourDelegate TileColour;

        private Color ChunkCheckered(WorldCellModel cell) =>
            Math.Abs(cell.ParentChunk.ChunkCoords.x) % 2 ==
            (Math.Abs(cell.ParentChunk.ChunkCoords.y) % 2 == 1 ? 1 : 0)
                ? Color.Gray
                : Color.Pink;

        private Color CellCheckered(WorldCellModel cell) =>
            Math.Abs(cell.RealCoords.x) % 2 == ((Math.Abs(cell.RealCoords.y) % 2 == 1) ? 1 : 0)
                ? Color.Gray
                : Color.Pink;

        private Color CellChunkCheckered(WorldCellModel cell) =>
            Math.Abs(cell.ParentChunk.ChunkCoords.x) % 2 ==
            (Math.Abs(cell.ParentChunk.ChunkCoords.y) % 2 == 1 ? 1 : 0)
                ? Math.Abs(cell.RealCoords.x) % 2 == ((Math.Abs(cell.RealCoords.y) % 2 == 1) ? 1 : 0)
                    ? Color.Gray
                    : Color.Yellow
                : Math.Abs(cell.RealCoords.x) % 2 == ((Math.Abs(cell.RealCoords.y) % 2 == 1) ? 1 : 0)
                    ? Color.Green
                    : Color.Red;

        private Color RandomColour(WorldCellModel cell) =>
            new Random(cell.RealCoords.y * cell.RealCoords.x).Next(0, 2) == 1 ? Color.Gray : Color.Pink;

        private void DrawCells()
        {
            // Start spritebatch for drawing
            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            // draw each tile in the chunks in the chunkGrid
            foreach (KeyValuePair<Coords, WorldChunkController> chunkGrid in GetChunksOnScreen())
            {
                foreach (WorldCellModel cell in chunkGrid.Value.WorldChunkModel.grid)
                {
                    int y = cell.RealCoords.y * TileSize;
                    int x = cell.RealCoords.x * TileSize;

                    Color colour = TileColour != null ? TileColour(cell) : CellChunkCheckered(cell);

                    spriteBatch.Draw(this.Content.Load<Texture2D>("grass"), new Rectangle(x, y, TileSize, TileSize),
                        colour);
                }
            }

            spriteBatch.End();
        }

        // Calculates wich chunks are in the camera's view and returns them in a list
        public Dictionary<Coords, WorldChunkController> GetChunksOnScreen()
        {
            Dictionary<Coords, WorldChunkController> chunksOnScreen = new Dictionary<Coords, WorldChunkController>();
            chunksOnScreen = World.WorldModel.ChunkGrid;
            float x = Camera.GetViewMatrix().M41;
            float y = Camera.GetViewMatrix().M42;
            Console.WriteLine($"X: {x}");
            Console.WriteLine($"Y: {y}");
            return chunksOnScreen;
        }

        public void DrawSelection()
        {
            spriteBatch.Begin();

            DrawHorizontalLine(Selection.View.Selection.Y);
            DrawHorizontalLine(Selection.View.Selection.Y + Selection.View.Selection.Height);
            DrawVerticalLine(Selection.View.Selection.X);
            DrawVerticalLine(Selection.View.Selection.X + Selection.View.Selection.Width);

            spriteBatch.End();
        }

        public void DrawHorizontalLine(int PositionY)
        {
            Texture2D texture = Content.Load<Texture2D>(Selection.View.LineTexture);
            if (Selection.View.Selection.Width > 0)
            {
                for (int i = 0; i <= Selection.View.Selection.Width - 10; i += 10)
                {
                    if (Selection.View.Selection.Width - i >= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle(Selection.View.Selection.X + i, PositionY, 10, 5),
                            Color.White);
                    }
                }
            }
            else if (Selection.View.Selection.Width < 0)
            {
                for (int i = -10; i >= Selection.View.Selection.Width; i -= 10)
                {
                    if (Selection.View.Selection.Width - i <= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle(Selection.View.Selection.X + i, PositionY, 10, 5),
                            Color.White);
                    }
                }
            }
        }

        public void DrawVerticalLine(int PositionX)
        {
            Texture2D texture = Content.Load<Texture2D>(Selection.View.LineTexture);
            if (Selection.View.Selection.Height > 0)
            {
                for (int i = -2; i <= Selection.View.Selection.Height; i += 10)
                {
                    if (Selection.View.Selection.Height - i >= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle(PositionX, Selection.View.Selection.Y + i, 10, 5),
                            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, MathHelper.ToRadians(90),
                            new Vector2(0, 0), SpriteEffects.None, 0);
                    }
                }
            }

            else if (Selection.View.Selection.Height < 0)
            {
                for (int i = 0; i >= Selection.View.Selection.Height; i -= 10)
                {
                    if (Selection.View.Selection.Height - i <= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle(PositionX - 10, Selection.View.Selection.Y + i, 10, 5),
                            Color.White);
                    }
                }
            }
        }

        public void DrawUnits()
        {
            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            spriteBatch.Draw(Content.Load<Texture2D>(Pichu.Draw()),
                new Rectangle(20, 20, (int)(TileSize * Pichu.Height), (int)(TileSize * Pichu.Width)), Color.White);

            spriteBatch.End();
        }
    }
}