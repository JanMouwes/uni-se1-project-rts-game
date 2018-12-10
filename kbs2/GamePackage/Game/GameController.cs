using System;
using System.Collections.Generic;
using System.Timers;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Desktop.View.Camera;
using kbs2.Desktop.World.World;
using kbs2.Faction.CurrencyMVC;
using kbs2.GamePackage.DayCycle;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
using kbs2.Unit.Unit;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.Enums;
using kbs2.World.Structs;
using kbs2.World.TerrainDef;
using kbs2.UserInterface;
using kbs2.View.GUI.ActionBox;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Unit;
using kbs2.WorldEntity.Unit.MVC;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace kbs2.GamePackage
{
    public delegate void GameSpeedObserver(object sender, GameSpeedEventArgs eventArgs);

    public delegate void GameStateObserver(object sender, GameStateEventArgs eventArgs);

	public delegate void MouseStateObserver(object sender, EventArgsWithPayload<MouseState> e);

    public delegate void OnTick(object sender, OnTickEventArgs eventArgs);

    public delegate void ShaderDelegate();

    public class GameController : Game
    {
        public GameModel gameModel { get; set; } = new GameModel();
        public GameView gameView { get; set; }

		public MouseInput MouseInput { get; set; }

        public const int TicksPerSecond = 30;

        public static int TickIntervalMilliseconds => 1000 / TicksPerSecond;

        private Timer GameTimer; //TODO

        public ActionInterface ActionInterface { get; set; }// testcode ===============

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

        DayController f = new DayController();
        Currency_Controller currency = new Currency_Controller();
        
		public event MouseStateObserver MouseStateChange;

		private MouseState mouseStatus;

		public MouseState MouseStatus
		{
			get => mouseStatus;
			set
			{
				mouseStatus = value;
				MouseStateChange?.Invoke(this, new EventArgsWithPayload<MouseState>(mouseStatus));
			}
		}

        public event OnTick onTick;

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

        private readonly GraphicsDeviceManager graphicsDeviceManager;

        private CameraController camera;

        private ShaderDelegate shader;

        public GameController(GameSpeed gameSpeed, GameState gameState)
        {
            this.GameSpeed = gameSpeed;
            this.GameState = gameState;

            graphicsDeviceManager = new GraphicsDeviceManager(this);

            shader = DefaultPattern;

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
      
            // Fill the Dictionairy
            TerrainDef.TerrainDictionary.Add(TerrainType.Grass, "grass");
            TerrainDef.TerrainDictionary.Add(TerrainType.Water, "Water-MiracleSea");

            // Generate world
            gameModel.World = WorldFactory.GetNewWorld();

			// Pathfinder 
			gameModel.pathfinder = new Pathfinder(gameModel.World.WorldModel, 500);

			gameModel.Selection = new Selection_Controller("PurpleLine");
            CellChunkCheckered();

			gameModel.MouseInput = new MouseInput();
            gameModel.Selection = new Selection_Controller("PurpleLine");
            gameModel.ActionBox = new ActionBoxController(new FloatCoords() { x = 50, y = 50 });

            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new CameraController(GraphicsDevice);
            gameView = new GameView(gameModel, graphicsDeviceManager, spriteBatch, camera, GraphicsDevice, Content);

            GameTimer = new Timer(TickIntervalMilliseconds);

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
            UnitDef unitdef = DBController.GetDefinitionFromUnit(1);
            DBController.CloseConnection();

            Building_Controller building = BuildingFactory.CreateNewBuilding(def, new Coords {x = 0, y = 0});
            Unit_Controller unit = UnitFactory.CreateNewUnit(unitdef, new Coords {x = 5, y = 5});
            
            gameModel.World.AddBuilding(def, building);

            BUCController building2 = BUCFactory.CreateNewBUC(def, new Coords { x = 0, y = 0 }, 10 );
            gameModel.World.AddBuildingUnderCunstruction(def, building2);
            building2.World = gameModel.World;
            building2.gameController = this;
            onTick += building2.Update;

            onTick += f.UpdateTime;

            UIView ui = new UIView(this);

            gameModel.GuiItemList.Add(ui);

            ActionInterface = new ActionInterface(this);
            ActionInterface.SetActions(new BuildActions(this));


            //TESTCODE


			//============= More TestCode ===============

			      MouseStateChange += gameModel.MouseInput.OnMouseStateChange;
            MouseStateChange += gameModel.ActionBox.OnRightClick;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        //    Loads chunk at mouse coordinates if not already loaded
        private void mouseChunkLoadUpdate(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            Coords windowCoords = new Coords
            {
                x = mouseState.X,
                y = mouseState.Y
            };

            FloatCoords cellCoords = (FloatCoords) WorldPositionCalculator.DrawCoordsToCellCoords(
                WorldPositionCalculator.TransformWindowCoords(
                    windowCoords,
                    camera.GetViewMatrix()
                ),
                gameView.TileSize
            );


            loadChunkIfUnloaded(WorldPositionCalculator.ChunkCoordsOfCellCoords(cellCoords));
        }

        private bool chunkExists(Coords chunkCoords) => gameModel.World.WorldModel.ChunkGrid.ContainsKey(chunkCoords) &&
                                                        gameModel.World.WorldModel.ChunkGrid[chunkCoords] != null;

        private void loadChunkIfUnloaded(Coords chunkCoords)
        {
            if (chunkExists(chunkCoords)) return;

            gameModel.World.WorldModel.ChunkGrid[chunkCoords] = WorldChunkLoader.ChunkGenerator(chunkCoords);
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


            List<IViewable> BUCs = new List<IViewable>();
            List<IText> Counters = new List<IText>();
            foreach (BUCController BUC in gameModel.World.WorldModel.UnderConstruction)
            {
                BUCs.Add(BUC.BUCView);
                Counters.Add(BUC.counter);
            }

            gameModel.ItemList.AddRange(BUCs);
            gameModel.TextList.AddRange(Counters);

            if (gameModel.ActionBox.BoxModel.Show)
            {
                gameModel.ItemList.Add(gameModel.ActionBox.BoxView);
                gameModel.TextList.Add(gameModel.ActionBox.BoxModel.Text);
            }

            List<IViewable> Cells = new List<IViewable>();
            foreach (KeyValuePair<Coords, WorldChunkController> chunk in gameModel.World.WorldModel.ChunkGrid)
            {
                foreach (WorldCellController cell in chunk.Value.WorldChunkModel.grid)
                {
                    Cells.Add(cell.worldCellView);
                }
            }   


            gameModel.ItemList.AddRange(Cells);


            gameModel.GuiTextList.Add(currency.view);
            onTick += currency.DailyReward;
            
            DBController.OpenConnection("DefDex");
            UnitDef unitdef = DBController.GetDefinitionFromUnit(1);
            DBController.CloseConnection();

            Unit_Controller unit = UnitFactory.CreateNewUnit(unitdef, new Coords { x = 5, y = 5 });

            gameModel.ItemList.Add(unit.UnitView);

            if (Keyboard.GetState().IsKeyDown(Keys.R)) shader = RandomPattern2;
            if (Keyboard.GetState().IsKeyDown(Keys.C)) shader = CellChunkCheckered;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) shader = DefaultPattern;

            mouseChunkLoadUpdate(gameTime);
            shader.Invoke();

            // ======================================================================================

            //  gameModel.Selection.Model.SelectionBox.DrawSelectionBox(Mouse.GetState(), camera.GetViewMatrix(), gameView.TileSize);

            // gameModel.Selection.CheckClickedBox(gameModel.World.WorldModel.Units, camera.GetInverseViewMatrix(), gameView.TileSize, camera.Zoom);

            // fire Ontick event
            OnTickEventArgs args = new OnTickEventArgs(gameTime);
            onTick?.Invoke(this,args);
            

            // Calls the game update
           
			//======= Fire MOUSESTATE ================
			MouseStatus = Mouse.GetState();

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
                    item2.worldCellView.Color = Math.Abs(item2.worldCellModel.ParentChunk.ChunkCoords.x) % 2 ==
                                                (Math.Abs(item2.worldCellModel.ParentChunk.ChunkCoords.y) % 2 == 1
                                                    ? 1
                                                    : 0)
                        ? Math.Abs(item2.worldCellModel.RealCoords.x) % 2 ==
                          (Math.Abs(item2.worldCellModel.RealCoords.y) % 2 == 1 ? 1 : 0)
                            ? Color.Gray
                            : Color.Yellow
                        : Math.Abs(item2.worldCellModel.RealCoords.x) % 2 ==
                          (Math.Abs(item2.worldCellModel.RealCoords.y) % 2 == 1 ? 1 : 0)
                            ? Color.Green
                            : Color.Sienna;
                }
            }
        }

        // Draws a random pattern on the cells
        public void DefaultPattern()
        {
            foreach (var Chunk in gameModel.World.WorldModel.ChunkGrid)
            {
                foreach (var item2 in Chunk.Value.WorldChunkModel.grid)
                {
                    item2.worldCellView.Color = Color.White;
                }
            }
        }

        public void RandomPattern2()
        {
            Random random = new Random(gameModel.World.WorldModel.seed);

            foreach (var Chunk in gameModel.World.WorldModel.ChunkGrid)
            {
                foreach (var item2 in Chunk.Value.WorldChunkModel.grid)
                {
                    switch (random.Next(0, 3))
                    {
                        case 0:
                            item2.worldCellView.Color = Color.Gray;
                            break;
                        case 1:
                            item2.worldCellView.Color = Color.Pink;
                            break;
                        default:
                            item2.worldCellView.Color = Color.White;
                            break;
                    }
                }
            }
        }
    }
}