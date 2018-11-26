using System;
using System.Collections.Generic;
using kbs2.Desktop.View.Camera;
using kbs2.Desktop.View.EventArgs;
using kbs2.Desktop.World.World;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
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

        // Calculate the size (Width) of a tile
        public int TileSize => (int)(GraphicsDevice.Viewport.Width / Camera.CameraModel.TileCount);

        // Calculates the height of a cell
        public int CellHeight => (int)(Camera.CameraModel.TileCount / GraphicsDevice.Viewport.AspectRatio);

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

            base.Draw(gameTime);
        }

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
                    Color color = (((Math.Abs(chunkGrid.Key.x) % 2) == (((Math.Abs(chunkGrid.Key.y) % 2) == 1) ? 1: 0)) ? Color.Gray : Color.Pink);

                    Color color2 = (((Math.Abs(cell.RealCoords.x) % 2) == (((Math.Abs(cell.RealCoords.y) % 2) == 1) ? 1 : 0)) ? Color.Gray : Color.Pink);

                    Random random = new Random(cell.RealCoords.y * cell.RealCoords.x);
                    Color color3 = ((random.Next(0 , 2) == 1) ? Color.Gray : Color.Pink);

                    spriteBatch.Draw(this.Content.Load<Texture2D>("grass"), new Rectangle(x, y, TileSize, TileSize), color3);
                }
            }

            spriteBatch.End();
        }

        // Calculates wich chunks are in the camera's view and returns them in a list
        public Dictionary<Coords, WorldChunkController> GetChunksOnScreen()
        {
            Dictionary<Coords, WorldChunkController> chunksOnScreen = new Dictionary<Coords, WorldChunkController>();
            chunksOnScreen = World.WorldModel.ChunkGrid;
            float x =  Camera.GetViewMatrix().M41;
            float y = Camera.GetViewMatrix().M42;
            float boundsX = x / (Camera.CameraModel.TileCount * TileSize);
            Console.WriteLine($"Xpos: {Camera.GetViewMatrix().M41}");
            Console.WriteLine($"Ypos: {Camera.GetViewMatrix().M42}");
            return chunksOnScreen;
        }
    }
}