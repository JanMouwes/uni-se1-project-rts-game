using System;
using System.Collections.Generic;
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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private WorldModel worldModel;

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
            // TODO: Add your initialization logic here
            camera2D = new Camera2D(GraphicsDevice);

            camera2D.MinimumZoom = (float) MinZoom;
            camera2D.MaximumZoom = (float) MaxZoom;

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


            // TODO: Add your update logic here
            // Add possible camera logic
            Vector2 moveVelocity = Vector2.Zero;

            const float moveSpeed = 1.5f;

            if (Keyboard.GetState().IsKeyDown(Keys.Right)) moveVelocity += new Vector2(moveSpeed, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) moveVelocity += new Vector2(0, moveSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) moveVelocity += new Vector2(-moveSpeed, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) moveVelocity += new Vector2(0, -moveSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.G)) camera2D.ZoomOut((float) 0.1);
            if (Keyboard.GetState().IsKeyDown(Keys.H)) camera2D.ZoomIn((float) 0.1);

            updateZoom(Mouse.GetState());

            camera2D.Move(moveVelocity);

            base.Update(gameTime);
        }


        private void updateZoom(MouseState mouseState)
        {
            int currentScrollWheelValue = mouseState.ScrollWheelValue;
            int scrollChange = previousScrollWheelValue - currentScrollWheelValue;
            double zoomChange = scrollChange / 36000.0;

            camera2D.ZoomIn((float) zoomChange);


            previousScrollWheelValue = currentScrollWheelValue;

            if (Math.Abs(zoomChange) < 0.000001) return;

            Console.WriteLine($"Zoom: {Zoom}");
            Console.WriteLine($"Camera-pos: {camera2D.Position}");
        }

        // added temp camera
        Camera2D camera2D;

        // tempsize
        private const int DefaultTiles = 30;
        private const double MinZoom = 1.0 / 6.0; //    Percent zoom (outer-most zoom)
        private const double MaxZoom = 8.0; //    Percent zoom (inner-most zoom)

        private int previousScrollWheelValue;

        private float Zoom => camera2D.Zoom;

        float TileCount => (float) Math.Ceiling((DefaultTiles / Zoom) > 1.0 ? (DefaultTiles / Zoom) : 1);

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Allows the user to resize the window
            base.Window.AllowUserResizing = true;

            // Makes the mouse visible in the window
            base.IsMouseVisible = true;

            int tileSize = (int) (base.GraphicsDevice.Viewport.Width / TileCount);
            int CellWidth = (int) TileCount;
            int CellHeight = (int) (CellWidth / base.GraphicsDevice.Viewport.AspectRatio);
            
            // Start spritebatch for drawing
            spriteBatch.Begin(transformMatrix: camera2D.GetViewMatrix());

            // initialize world
            WorldController world = WorldFactory.GetNewWorld();
            
            // draw each tile in the chunks in the chunkGrid
            foreach(KeyValuePair<Coords, WorldChunkController> chunkGrid in world.worldModel.ChunkGrid)
            {
                foreach (WorldCellModel cell in chunkGrid.Value.worldChunkModel.grid)
                {
                    int y = cell.RealCoords.y * tileSize;
                    int x = cell.RealCoords.x * tileSize;
                    spriteBatch.Draw(this.Content.Load<Texture2D>("grass"), new Rectangle(x, y, tileSize, tileSize), Color.LawnGreen);
                }
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawCells(GameTime gameTime)
        {
        }
    }
}