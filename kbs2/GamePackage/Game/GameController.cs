using System;
using System.Collections.Generic;
using System.Timers;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Desktop.View.Camera;
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
using kbs2.World.World;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Unit;
using kbs2.WorldEntity.Unit.MVC;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using kbs2.Faction.FactionMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.WorldEntitySpawner;
using kbs2.UserInterface.BottomBar;

namespace kbs2.GamePackage
{
    public delegate void GameSpeedObserver(object sender, GameSpeedEventArgs eventArgs);

    public delegate void GameStateObserver(object sender, EventArgsWithPayload<GameState> eventArgs);

    public delegate void MouseStateObserver(object sender, EventArgsWithPayload<MouseState> e);

    public delegate void OnTick(object sender, OnTickEventArgs eventArgs);

    public delegate void ShaderDelegate();

    public class GameController : Game
    {
        public GameModel gameModel { get; set; } = new GameModel();
        public GameView gameView { get; set; }
        public EntitySpawner spawner;

        public MouseInput MouseInput { get; set; }

        public const int TicksPerSecond = 30;

        public static int TickIntervalMilliseconds => 1000 / TicksPerSecond;

        private Timer GameTimer; //TODO

        public ActionInterface ActionInterface { get; set; } // testcode ===============
        public BuildActions BuildActions { get; set; }
        public bool QPressed { get; set; }
        public bool APressed { get; set; }
        public Terraintester Terraintester { get; set; }

        private BottomBarView bottomBarView;

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

        public Faction_Controller PlayerFaction = new Faction_Controller("PlayerFaction");

        public event MouseStateObserver MouseStateChange;


        public MouseState PreviousMouseButtonsStatus { get; set; }

        public event OnTick onTick;

        //    GameState and its event
        private GameState gameState;

        public GameState GameState
        {
            get => gameState;
            set
            {
                gameState = value;
                GameStateChange?.Invoke(this,
                    new EventArgsWithPayload<GameState>(gameState)); //Invoke event if has subscribers
            }
        }

        public event GameStateObserver GameStateChange;

        private readonly GraphicsDeviceManager graphicsDeviceManager;

        public CameraController camera;

        private ShaderDelegate shader;

        public GameController(GameSpeed gameSpeed, GameState gameState)
        {
            this.GameSpeed = gameSpeed;
            this.GameState = gameState;

            GameStateChange += PauseGame;

            graphicsDeviceManager = new GraphicsDeviceManager(this);

            shader = DefaultPattern;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Is subscribed to the gamestate so this is called every time the gamestate is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public void PauseGame(object sender, EventArgsWithPayload<GameState> eventArgs)
        {
            if (eventArgs.Value != GameState.Paused) return;
            throw new NotImplementedException();
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
            TerrainDef.TerrainDictionary.Add(TerrainType.Sand, "Sand");
            TerrainDef.TerrainDictionary.Add(TerrainType.Soil, "Soil");
            TerrainDef.TerrainDictionary.Add(TerrainType.Snow, "Snow");
            TerrainDef.TerrainDictionary.Add(TerrainType.Rock, "Rock");
            TerrainDef.TerrainDictionary.Add(TerrainType.Trees, "Tree-2");

            // Generate world
            gameModel.World = WorldFactory.GetNewWorld();

            // Pathfinder 
            gameModel.pathfinder = new Pathfinder(gameModel.World.WorldModel, 500);

            // Spawner
            spawner = new EntitySpawner(gameModel.World, ref onTick);

            gameModel.ActionBox = new ActionBoxController(new FloatCoords() {x = 50, y = 50});

            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new CameraController(GraphicsDevice);
            gameView = new GameView(gameModel, graphicsDeviceManager, spriteBatch, camera, GraphicsDevice, Content);

            GameTimer = new Timer(TickIntervalMilliseconds);


            gameModel.MouseInput = new MouseInput(this);

            // Allows the user to resize the window
            base.Window.AllowUserResizing = true;

            // Makes the mouse visible in the window
            base.IsMouseVisible = true;

            shader();

            // Initalize game
            base.Initialize();
        }

        /// <summary>
        /// LoadContent is called once per game and is to load all the content.
        /// </summary>
        protected override void LoadContent()

        {
            //TESTCODE
            QPressed = false;
            APressed = false;
            Terraintester = new Terraintester();


            onTick += SetBuilding;
            onTick += f.UpdateTime;
            onTick += gameModel.MouseInput.Selection.Update;
            gameModel.MouseInput.Selection.onSelectionChanged += ChangeSelection;
            gameModel.MouseInput.Selection.onSelectionChanged += UpdateHUDOnSelect;

			StatusBarView statusBarView = new StatusBarView(this);
			LeftButtonBar leftButtonBar = new LeftButtonBar(this);
			RightButtonBar rightButtonBar = new RightButtonBar(this);

			bottomBarView = new BottomBarView(this);
			MiniMapBar miniMap = new MiniMapBar(this);
			ActionBarView actionBar = new ActionBarView(this);

            gameModel.GuiItemList.Add(statusBarView);
			gameModel.GuiItemList.Add(leftButtonBar);
			gameModel.GuiItemList.Add(rightButtonBar);
			gameModel.GuiItemList.Add(bottomBarView);
			gameModel.GuiItemList.Add(miniMap);
			gameModel.GuiItemList.Add(actionBar);


			ActionInterface = new ActionInterface(this);
            BuildActions = new BuildActions(this);
            ActionInterface.SetActions(BuildActions);

            //TESTCODE
            DBController.OpenConnection("DefDex");
            UnitDef unitdef = DBController.GetDefinitionFromUnit(1);
            DBController.CloseConnection();

            for (int i = 0; i < 12; i++)
            {
                Unit_Controller unit =
                    UnitFactory.CreateNewUnit(unitdef, new Coords {x = i, y = 5}, gameModel.World.WorldModel);

                unit.UnitModel.Speed = 0.05f;
                unit.LocationController.LocationModel.UnwalkableTerrain.Add(TerrainType.Water);
                spawner.SpawnUnit(unit, PlayerFaction);
                onTick += unit.LocationController.Ontick;
            }

            //============= More TestCode ===============

            MouseStateChange += gameModel.MouseInput.OnMouseStateChange;
            MouseStateChange += gameModel.ActionBox.OnRightClick;
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
        /// SaveToDB is called by the user or when the game is closed to save the game to the database
        /// </summary>
        public void SaveToDB()
        {
            //GameState = GameState.Paused;

            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads chunk at mouse coordinates if not already loaded
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        private bool chunkExists(Coords chunkCoords) => gameModel.World.WorldModel.ChunkGrid.ContainsKey(chunkCoords) &&
                                                        gameModel.World.WorldModel.ChunkGrid[chunkCoords] != null;

        /// <summary>
        /// 
        /// </summary>
        private void loadChunkIfUnloaded(Coords chunkCoords)
        {
            if (chunkExists(chunkCoords)) return;

            gameModel.World.WorldModel.ChunkGrid[chunkCoords] = WorldChunkLoader.ChunkGenerator(chunkCoords);

            shader();
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

            MouseState temp = Mouse.GetState();
            Coords tempcoords = new Coords {x = temp.X, y = temp.Y};
            Coords coords = WorldPositionCalculator.DrawCoordsToCellCoords(
                WorldPositionCalculator.TransformWindowCoords(tempcoords, camera.GetViewMatrix()), gameView.TileSize);
            if (gameModel.World.GetCellFromCoords(coords) != null)
            {
                Terraintester.Text =
                    $"{coords.x},{coords.y}  {gameModel.World.GetCellFromCoords(coords).worldCellModel.Terrain.ToString()}";
                if (gameModel.World.GetCellFromCoords(coords).worldCellModel.BuildingOnTop != null)
                {
                    Terraintester.Text += " b";
                }
            }

            //gameModel.GuiTextList.Add(Terraintester);

            // Update Buildings on screen
            List<IViewImage> buildings = new List<IViewImage>();
            foreach (Building_Controller building in gameModel.World.WorldModel.buildings)
            {
                buildings.Add(building.View);
            }

            gameModel.ItemList.AddRange(buildings);


            List<IViewImage> BUCs = new List<IViewImage>();
            List<IViewText> Counters = new List<IViewText>();
            foreach (BUCController BUC in gameModel.World.WorldModel.UnderConstruction)
            {
                BUCs.Add(BUC.BUCView);
                Counters.Add(BUC.counter);
            }

            gameModel.ItemList.AddRange(BUCs);
            gameModel.TextList.AddRange(Counters);


            List<IViewImage> Units = (from unit in gameModel.World.WorldModel.Units
                select unit.UnitView).Cast<IViewImage>().ToList();

            gameModel.ItemList.AddRange(Units);


            if (gameModel.ActionBox.BoxModel.Show)
            {
                gameModel.ItemList.Add(gameModel.ActionBox.BoxView);
                gameModel.TextList.Add(gameModel.ActionBox.BoxModel.Text);
            }

            int TileSize = (int) (GraphicsDevice.Viewport.Width / camera.CameraModel.TileCount);

            List<IViewImage> Cells = new List<IViewImage>();
            List<WorldChunkController> chunks = (from chunk in gameModel.World.WorldModel.ChunkGrid
                let rightBottomViewBound = WorldPositionCalculator.DrawCoordsToCellCoords(
                    WorldPositionCalculator.TransformWindowCoords(
                        new Coords()
                        {
                            x = GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width,
                            y = GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height
                        }, camera.GetViewMatrix()), TileSize)
                let topLeftViewBound = WorldPositionCalculator.DrawCoordsToCellCoords(
                    WorldPositionCalculator.TransformWindowCoords(
                        new Coords() {x = GraphicsDevice.Viewport.X, y = GraphicsDevice.Viewport.Y},
                        camera.GetViewMatrix()), TileSize)
                let rightBottomBound = new Coords() {x = 20 + WorldChunkModel.ChunkSize, y = 20}
                let leftTopBound = new Coords()
                    {x = (chunk.Key.x * WorldChunkModel.ChunkSize), y = (chunk.Key.y * WorldChunkModel.ChunkSize)}
                let chunkRectangle = new Rectangle(leftTopBound.x, leftTopBound.y,
                    (rightBottomBound.x < 0 ? rightBottomBound.x * -1 : rightBottomBound.x),
                    (rightBottomBound.y < 0 ? rightBottomBound.y * -1 : rightBottomBound.y))
                let viewRectangle = new Rectangle(topLeftViewBound.x, topLeftViewBound.y,
                    Math.Abs(topLeftViewBound.x - rightBottomViewBound.x),
                    Math.Abs(topLeftViewBound.y - rightBottomViewBound.y))
                where (chunkRectangle.Intersects(viewRectangle))
                select chunk.Value).ToList();
            Console.WriteLine(chunks.Count);

            foreach (WorldChunkController chunk in chunks)
            {
                foreach (WorldCellController cell in chunk.WorldChunkModel.grid)
                {
                    Cells.Add(cell.worldCellView);
                }
            }


            gameModel.ItemList.AddRange(Cells);


            gameModel.GuiTextList.Add(PlayerFaction.currency_Controller.view);
            onTick += PlayerFaction.currency_Controller.DailyReward;


            ShaderDelegate tempShader = null;

            if (Keyboard.GetState().IsKeyDown(Keys.R)) tempShader = RandomPattern2;
            if (Keyboard.GetState().IsKeyDown(Keys.C)) tempShader = CellChunkCheckered;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) tempShader = DefaultPattern;

            mouseChunkLoadUpdate(gameTime);

            if (tempShader != null)
            {
                shader = tempShader;
                shader();
            }

            // ======================================================================================

            //  gameModel.Selection.Model.SelectionBox.DrawSelectionBox(Mouse.GetState(), camera.GetViewMatrix(), gameView.TileSize);

            // gameModel.Selection.CheckClickedBox(gameModel.World.WorldModel.Units, camera.GetInverseViewMatrix(), gameView.TileSize, camera.Zoom);

            // fire Ontick event
            OnTickEventArgs args = new OnTickEventArgs(gameTime);
            onTick?.Invoke(this, args);


            // Calls the game update

            //======= Fire MOUSESTATE ================
            MouseState CurrentMouseButtonsStatus = Mouse.GetState();
            if (CurrentMouseButtonsStatus.LeftButton != PreviousMouseButtonsStatus.LeftButton ||
                CurrentMouseButtonsStatus.RightButton != PreviousMouseButtonsStatus.RightButton)
            {
                MouseStateChange?.Invoke(this, new EventArgsWithPayload<MouseState>(CurrentMouseButtonsStatus));
                PreviousMouseButtonsStatus = CurrentMouseButtonsStatus;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.S)) SaveToDB();

            // Calls the game update
            base.Update(gameTime);
        }

        /// <summary>
        /// TestCode
        /// </summary>
        public void SetBuilding(object sender, OnTickEventArgs eventArgs)
        {
            if ((!QPressed) && Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                DBController.OpenConnection("DefDex");
                BuildingDef def = DBController.GetDefinitionBuilding(1);
                DBController.CloseConnection();

                MouseState temp = Mouse.GetState();
                Coords tempcoords = new Coords {x = temp.X, y = temp.Y};
                Coords coords = WorldPositionCalculator.DrawCoordsToCellCoords(
                    WorldPositionCalculator.TransformWindowCoords(tempcoords, camera.GetViewMatrix()),
                    gameView.TileSize);

                List<Coords> buidlingcoords = new List<Coords>();
                foreach (Coords stuff in def.BuildingShape)
                {
                    buidlingcoords.Add(coords + stuff);
                }

                List<TerrainType> whitelist = new List<TerrainType>();
                whitelist.Add(TerrainType.Grass);
                whitelist.Add(TerrainType.Default);


                if (gameModel.World.checkTerainCells(buidlingcoords, whitelist))
                {
                    BUCController building = BUCFactory.CreateNewBUC(def, coords,
                        30 + (int) eventArgs.GameTime.TotalGameTime.TotalSeconds, PlayerFaction);
                    gameModel.World.AddBuildingUnderCunstruction(def, building);
                    building.World = gameModel.World;
                    building.gameController = this;
                    onTick += building.Update;
                }

                QPressed = true;
            }

            if ((!Keyboard.GetState().IsKeyDown(Keys.Q)) && QPressed == true)
            {
                QPressed = false;
            }
        }

        public void ChangeSelection(object sender, EventArgsWithPayload<List<IHasActions>> eventArgs)
        {
            IHasActions actions = eventArgs.Value.Count > 0 ? eventArgs.Value.First() : BuildActions;
            ActionInterface.SetActions(actions);
        }


        /// <summary>
        /// Changes the HUD according to the selected entities
        /// </summary>
        /// <param name="eventArgs">List of selected entities</param>
        public void UpdateHUDOnSelect(object sender, EventArgsWithPayload<List<IHasActions>> eventArgs)
        {
            // Clean the GUI of selected entities
            foreach (BottomBarStatView view in bottomBarView.Model.StatViews)
            {
                gameModel.GuiItemList.Remove(view.StatImage);
                gameModel.GuiTextList.Remove(view.StatText);
            }

            bottomBarView.Model.StatViews.Clear();

            // Convert all IHasActions to Unit_Controllers
            List<IHasActions> units = gameModel.MouseInput.Selection.SelectUnits();
            units.ConvertAll(o => (Unit_Controller) o);
            // Add new views to the model
            foreach(Unit_Controller unit in units)
                bottomBarView.Model.StatViews.Add(new BottomBarStatView(bottomBarView.Model, unit.UnitView, unit.HPController.HPModel));
            // Convert all IHasActions to Unit_Controllers
            List<IHasActions> buildings = gameModel.MouseInput.Selection.SelectBuildings();
            buildings.ConvertAll(o => (Building_Controller)o);
            // Add new views to the model
            foreach (Building_Controller building in buildings)
                bottomBarView.Model.StatViews.Add(new BottomBarStatView(bottomBarView.Model, building.View, building.HPController.HPModel));
            // Adds the views to the gameModel
            foreach (BottomBarStatView view in bottomBarView.Model.StatViews)
            {
                gameModel.GuiItemList.Add(view.StatImage);
                gameModel.GuiTextList.Add(view.StatText);
            }
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
        /// <summary>
        /// Draws the chunks and cells in a Checkered pattern for easy debugging
        /// </summary>
        public void CellChunkCheckered()
        {
            foreach (var Chunk in gameModel.World.WorldModel.ChunkGrid)
            {
                foreach (var item2 in Chunk.Value.WorldChunkModel.grid)
                {
                    item2.worldCellView.Colour = Math.Abs(item2.worldCellModel.ParentChunk.ChunkCoords.x) % 2 ==
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
                    item2.worldCellView.Colour = Color.White;
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
                    switch (item2.worldCellModel.Terrain)
                    {
                        case TerrainType.Grass:
                            switch (random.Next(0, 5))
                            {
                                case 0:
                                    item2.worldCellView.Colour = Color.Gray;
                                    break;
                                case 1:
                                    item2.worldCellView.Colour = Color.DarkGray;
                                    break;
                                case 2:
                                    item2.worldCellView.Colour = Color.LightGreen;
                                    break;
                                default:
                                    item2.worldCellView.Colour = Color.White;
                                    break;
                            }

                            break;
                        case TerrainType.Sand:
                            switch (random.Next(0, 2))
                            {
                                case 0:
                                    item2.worldCellView.Colour = Color.WhiteSmoke;
                                    break;
                                default:
                                    item2.worldCellView.Colour = Color.White;
                                    break;
                            }

                            break;
                        case TerrainType.Water:
                            switch (random.Next(0, 5))
                            {
                                case 0:
                                    item2.worldCellView.Colour = Color.LightBlue;
                                    break;
                                case 1:
                                    item2.worldCellView.Colour = Color.LightGray;
                                    break;
                                case 2:
                                    item2.worldCellView.Colour = Color.LightCyan;
                                    break;
                                default:
                                    item2.worldCellView.Colour = Color.White;
                                    break;
                            }

                            break;
                        case TerrainType.Rock:
                            switch (random.Next(0, 4))
                            {
                                case 0:
                                    item2.worldCellView.Colour = Color.LightGray;
                                    break;
                                case 1:
                                    item2.worldCellView.Colour = Color.DarkGray;
                                    break;
                                default:
                                    item2.worldCellView.Colour = Color.White;
                                    break;
                            }

                            break;
                        case TerrainType.Soil:
                            switch (random.Next(0, 7))
                            {
                                case 0:
                                    item2.worldCellView.Colour = Color.SaddleBrown;
                                    break;
                                case 1:
                                    item2.worldCellView.Colour = Color.RosyBrown;
                                    break;
                                default:
                                    item2.worldCellView.Colour = Color.White;
                                    break;
                            }

                            break;
                        case TerrainType.Trees:
                            switch (random.Next(0, 3))
                            {
                                case 0:
                                    item2.worldCellView.Colour = Color.DarkGreen;
                                    break;
                                case 1:
                                    item2.worldCellView.Colour = Color.Green;
                                    break;
                                default:
                                    item2.worldCellView.Colour = Color.ForestGreen;
                                    break;
                            }

                            break;
                        case TerrainType.Snow:
                            switch (random.Next(0, 3))
                            {
                                case 0:
                                    item2.worldCellView.Colour = Color.White;
                                    break;
                                default:
                                    item2.worldCellView.Colour = Color.WhiteSmoke;
                                    break;
                            }

                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}