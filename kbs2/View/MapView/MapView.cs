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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        CameraController Camera;

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

            MoveCamera();

            base.Update(gameTime);
        }

        /// <summary>
        /// Checks keys for moving the camera and updates the camera.
        /// </summary>
        private void MoveCamera()
        {
            Vector2 moveVelocity = Vector2.Zero;

            const float moveSpeed = 1.5f;

            if (Keyboard.GetState().IsKeyDown(Keys.Right)) moveVelocity += new Vector2(moveSpeed, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) moveVelocity += new Vector2(0, moveSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) moveVelocity += new Vector2(-moveSpeed, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) moveVelocity += new Vector2(0, -moveSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.G)) Camera.ZoomOut((float)0.1);
            if (Keyboard.GetState().IsKeyDown(Keys.H)) Camera.ZoomIn((float)0.1);

            updateZoom();

            Camera.Move(moveVelocity);
        }

        /// <summary>
        /// Checks for zoom updates and changes zoom accordingly.
        /// </summary>
        private void updateZoom()
        {
            int currentScrollWheelValue = Mouse.GetState().ScrollWheelValue;
            int scrollChange = Camera.cameraModel.PreviousScrollWheelValue - currentScrollWheelValue;
            double zoomChange = scrollChange / 36000.0;

            Camera.ZoomOut((float) zoomChange);

            Camera.cameraModel.PreviousScrollWheelValue = currentScrollWheelValue;

            if (Math.Abs(zoomChange) < 0.000001) return;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            DrawCells(gameTime);

            base.Draw(gameTime);
        }

        private void DrawCells(GameTime gameTime)
        {
            // Calculate the size (Width) of a tile
            int tileSize = (int)(GraphicsDevice.Viewport.Width / Camera.cameraModel.TileCount);

            // Calculates the height of a cell
            int CellHeight = (int)(Camera.cameraModel.TileCount / GraphicsDevice.Viewport.AspectRatio);

            // Start spritebatch for drawing
            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            // initialize world
            WorldController world = WorldFactory.GetNewWorld();

            // draw each tile in the chunks in the chunkGrid
            foreach (KeyValuePair<Coords, WorldChunkController> chunkGrid in world.worldModel.ChunkGrid)
            {
                foreach (WorldCellModel cell in chunkGrid.Value.worldChunkModel.grid)
                {
                    int y = cell.RealCoords.y * tileSize;
                    int x = cell.RealCoords.x * tileSize;
                    spriteBatch.Draw(this.Content.Load<Texture2D>("grass"), new Rectangle(x, y, tileSize, tileSize), Color.LawnGreen);
                }
            }

            spriteBatch.End();
        }
    }
}