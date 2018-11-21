using kbs2.Desktop.World.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace kbs2.Desktop.View.MapView
{
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
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) moveVelocity += new Vector2(1, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) moveVelocity += new Vector2(0, 1);
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) moveVelocity += new Vector2(-1, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) moveVelocity += new Vector2(0, -1);
            if (Keyboard.GetState().IsKeyDown(Keys.G)) tileSize += 1;
            if (Keyboard.GetState().IsKeyDown(Keys.H)) tileSize -= 1;

            camera2D.Move(moveVelocity);

            base.Update(gameTime);
        }

        // added temp camera
        Camera2D camera2D;
        // temp size
        int tileSize = 50;

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
            int viewPortX = viewPort.X;
            int viewPortY = viewPort.Y;

            // TODO: Add your drawing code here
            // Done draw basic sprite on screen
            spriteBatch.Begin(transformMatrix: camera2D.GetViewMatrix());

            Vector2 tilePostition = Vector2.Zero;

            int width = 15;
            int height = 15;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    spriteBatch.Draw(this.Content.Load<Texture2D>("grass"), new Rectangle((int)tilePostition.X, (int)tilePostition.Y, tileSize, tileSize), Color.White);
                    tilePostition.Y += tileSize;
                }
                tilePostition.Y = 0;
                tilePostition.X += tileSize;
            }

            spriteBatch.End();

            // end own code

            base.Draw(gameTime);
        }

        private void DrawCells(GameTime gameTime)
        {
        }
    }
}
 