using System;
using kbs2.Desktop.View.EventArgs;
using kbs2.Desktop.World.World;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

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

            int moveSpeed = 2;

            if (Keyboard.GetState().IsKeyDown(Keys.Right)) moveVelocity += new Vector2(moveSpeed, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) moveVelocity += new Vector2(0, moveSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) moveVelocity += new Vector2(-moveSpeed, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) moveVelocity += new Vector2(0, -moveSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.G)) Zoom -= 0.1;
            if (Keyboard.GetState().IsKeyDown(Keys.H)) Zoom += 0.1;

            updateZoom(Mouse.GetState());

            camera2D.Move(moveVelocity);

            base.Update(gameTime);
        }

        private void updateZoom(MouseState mouseState)
        {
            int currentScrollWheelValue = mouseState.ScrollWheelValue;
            int scrollChange = previousScrollWheelValue - currentScrollWheelValue;

            if (Math.Abs(scrollChange) == 0) return;

            Console.WriteLine($"PreviousScrollValue: {previousScrollWheelValue}");
            Console.WriteLine($"CurrentScrollValue: {currentScrollWheelValue}");
            Console.WriteLine($"ScrollChange: {scrollChange}");
            Console.WriteLine($"Tiles on screen: {TileCount}");
            Zoom = 1.0 + scrollChange / 100.0;

            previousScrollWheelValue = currentScrollWheelValue;
        }

        // added temp camera
        Camera2D camera2D;

        // tempsize
        private const int DefaultTiles = 30;
        private const double MinZoom = 1.0 / 6.0; //    Percent zoom (outer-most zoom)
        private const double MaxZoom = 8.0; //    Percent zoom (inner-most zoom)

        private int previousScrollWheelValue;

        private double zoom = 1;

        private double Zoom
        {
            get => zoom;
            set
            {
                if (!(value < MaxZoom && value > MinZoom))
                    return;

                zoom = value;
                Console.WriteLine($"Zoom: {zoom}");
                ZoomEvent?.Invoke(this, new ZoomEventArgs(Zoom));
            }
        }

        public event ZoomObserver ZoomEvent;


        double TileCount => Math.Ceiling((DefaultTiles / Zoom) > 1.0 ? (DefaultTiles / Zoom) : 1);

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Window.AllowUserResizing = true;
            base.IsMouseVisible = true;

            // handy functions
            Viewport viewPort = base.GraphicsDevice.Viewport;
            float viewPortRatio = viewPort.AspectRatio;
            int viewPortHeight = viewPort.Height;
            int viewPortWidth = viewPort.Width;

            int tileSize = (int)(viewPortWidth / TileCount);
            int CellWidth = (int)TileCount;
            int CellHeight = (int)(CellWidth / viewPortRatio);

            // TODO: Add your drawing code here
            // Done draw basic sprite on screen
            spriteBatch.Begin(transformMatrix: camera2D.GetViewMatrix());

            Coords coords = new Coords();
            coords.x = 1;
            coords.y = 1;
            WorldChunkController chunkController = WorldChunkFactory.ChunkOfTerrainType(coords, TerrainType.Sand);
            foreach (WorldCellModel cell in chunkController.worldChunkModel.grid)
            {
                int y = cell.RealCoords.y * tileSize;
                int x = cell.RealCoords.x * tileSize;
                spriteBatch.Draw(this.Content.Load<Texture2D>("grass"), new Rectangle(x, y, tileSize, tileSize), Color.White);
            }



            /*
            Vector2 tilePostition = Vector2.Zero;
            for (int x = 0; x < CellWidth; x++)
            {
                for (int y = 0; y < CellHeight; y++)
                {
                    spriteBatch.Draw(this.Content.Load<Texture2D>("grass"),
                        new Rectangle((int) tilePostition.X, (int) tilePostition.Y, tileSize, tileSize), Color.White);
                    tilePostition.Y += tileSize;
                }

                tilePostition.Y = 0;
                tilePostition.X += tileSize;
            }
            */
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawCells(GameTime gameTime)
        {
        }
    }
}