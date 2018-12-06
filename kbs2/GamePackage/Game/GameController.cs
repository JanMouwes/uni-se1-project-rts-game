using System;
using System.Collections.Generic;
using System.Timers;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Desktop.View.Camera;
using kbs2.Desktop.World.World;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.World;
using kbs2.WorldEntity.Building;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace kbs2.GamePackage
{
    public delegate void GameSpeedObserver(object sender, GameSpeedEventArgs eventArgs);

    public delegate void GameStateObserver(object sender, GameStateEventArgs eventArgs);

    public class GameController : Game
    {
        public GameModel gameModel { get; set; } = new GameModel();

        public GameView gameView { get; set; }

        public const int TicksPerSecond = 30;

        public static int TickIntervalMilliseconds => 1000 / TicksPerSecond;

        private Timer GameTimer; //TODO

        public event ElapsedEventHandler GameTick
        {
            add => GameTimer.Elapsed += value;
            remove => GameTimer.Elapsed -= value;
        }

        //    GameSpeed and its event
        private GameSpeed gameSpeed;

        public GameSpeed GameSpeed
        {
            get => gameSpeed;
            set
            {
                gameSpeed = value;
                GameSpeedChange?.Invoke(this, new GameSpeedEventArgs(gameSpeed)); //Invoke event if has subscribers
            }
        }

        public event GameSpeedObserver GameSpeedChange;

        //    GameState and its event
        private GameState gameState;

        public GameState GameState
        {
            get => gameState;
            set
            {
                gameState = value;
                GameStateChange?.Invoke(this, new GameStateEventArgs(gameState)); //Invoke event if has subscribers
            }
        }

        public event GameStateObserver GameStateChange;

        private CameraController camera;

        public GameController(GameSpeed gameSpeed, GameState gameState)
        {
            this.GameSpeed = gameSpeed;
            this.GameState = gameState;
            
            Content.RootDirectory = "Content";

            gameModel.World = WorldFactory.GetNewWorld();

            gameModel.Selection = new Selection_Controller("PurpleLine");

            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new CameraController(GraphicsDevice);
            GraphicsDeviceManager graphicsDeviceManager = new GraphicsDeviceManager(this);
            gameView = new GameView(gameModel, graphicsDeviceManager, spriteBatch, camera, GraphicsDevice, Content);

            GameTimer = new Timer(TickIntervalMilliseconds);
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
            camera.MoveCamera();

            // ============== Temp Code ===================================================================
            // Update Buildings on screen
            List<IViewable> buildings = new List<IViewable>();
            foreach (Building_Controller building in gameModel.World.WorldModel.buildings)
            {
                buildings.Add(building.View);
            }
            gameModel.ItemList.AddRange(buildings);

            List<IViewable> Cells = new List<IViewable>();
            foreach (KeyValuePair<Coords, WorldChunkController> chunk in gameModel.World.WorldModel.ChunkGrid)
            {
                foreach (WorldCellController cell in chunk.Value.WorldChunkModel.grid)
                {
                    Cells.Add(cell.worldCellView);
                }
            }
            gameModel.ItemList.AddRange(Cells);

            // ======================================================================================

            gameModel.Selection.Model.SelectionBox.DrawSelectionBox(Mouse.GetState(), camera.GetViewMatrix(), gameView.TileSize);

            gameModel.Selection.CheckClickedBox(gameModel.World.WorldModel.Units, camera.GetInverseViewMatrix(), gameView.TileSize, camera.Zoom);

            // Calls the game update
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            gameView.Draw();

            // Calls the game's draw function
            base.Draw(gameTime);
        }

        

        // ===========================================================================================================================
        // Draws the chunks and cells in a Checkered pattern for easy debugging
        public void CellChunkCheckered()
        {
            foreach (var Chunk in gameModel.World.WorldModel.ChunkGrid)
            {
                foreach (var item2 in Chunk.Value.WorldChunkModel.grid)
                {
                    item2.worldCellView.Color =  Math.Abs(item2.worldCellModel.ParentChunk.ChunkCoords.x) % 2 ==
                    (Math.Abs(item2.worldCellModel.ParentChunk.ChunkCoords.y) % 2 == 1 ? 1 : 0)
                        ? Math.Abs(item2.worldCellModel.RealCoords.x) % 2 == (Math.Abs(item2.worldCellModel.RealCoords.y) % 2 == 1 ? 1 : 0)
                            ? Color.Gray
                            : Color.Yellow
                        : Math.Abs(item2.worldCellModel.RealCoords.x) % 2 == (Math.Abs(item2.worldCellModel.RealCoords.y) % 2 == 1 ? 1 : 0)
                            ? Color.Green
                            : Color.Sienna;
                }
            }
        }

        // Draws a random pattern on the cells
        public void RandomPattern()
        {
            Random random = new Random();

            foreach (var Chunk in gameModel.World.WorldModel.ChunkGrid)
            {
                foreach (var item2 in Chunk.Value.WorldChunkModel.grid)
                {
                    item2.worldCellView.Color = random.Next(0, 3) == 1 ? Color.Gray : Color.Pink;
                }
            }
        }
    }
}