using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Desktop.View.Camera;
using kbs2.Desktop.World.World;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.World.TerrainDef;
using kbs2.World.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace kbs2.GamePackage
{
    class GameView : Game
    {
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
        public GameView()
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

            // ================ Temp Game items == These items do not belong here but are for testing ==================

            TerrainDef.TerrainDictionairy.Add(TerrainType.Sand, "grass");

            // Temp view added to drawList for testing
            FloatCoords coords = new FloatCoords { x = 1, y = 1 };
            WorldCellView view = new WorldCellView(coords, "grass");
            DrawList.Add(view);
            DrawStaticList.Add(view);



            // ================ End of Temp code =====================================
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
                Texture2D texture = this.Content.Load<Texture2D>(DrawItem.Texture);
                spriteBatch.Draw(texture, new Rectangle((int)DrawItem.Coords.x, (int)DrawItem.Coords.y, (int)(DrawItem.Width * TileSize), (int)(DrawItem.Height * TileSize)), DrawItem.Color);
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
    }
}
