using System;
using System.Collections.Generic;
using System.Diagnostics;
using kbs2.Desktop.View.Camera;
using kbs2.GamePackage.DayCycle;
using kbs2.GamePackage.EventArgs;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.Enums;
using kbs2.World.Structs;
using kbs2.World.TerrainDef;
using kbs2.UserInterface;
using kbs2.World.World;
using kbs2.WorldEntity.Unit;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using kbs2.Actions.ActionTabActions;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.GameActionSelector;
using kbs2.Actions.Interfaces;
using kbs2.Faction;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.Animation;
using kbs2.GamePackage.Interfaces;
using kbs2.GamePackage.Selection;
using kbs2.UserInterface.GameActionGui;
using kbs2.View.GUI;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Pathfinder;
using kbs2.WorldEntity.Structures;
using kbs2.WorldEntity.Structures.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.WorldEntitySpawner;
using kbs2.GamePackage.CPU;

namespace kbs2.GamePackage
{
    public delegate void GameSpeedObserver(object sender, GameSpeedEventArgs eventArgs);

    public delegate void GameStateObserver(object sender, EventArgsWithPayload<GameState> eventArgs);

    public delegate void MouseStateObserver(object sender, EventArgsWithPayload<MouseState> e);

    public delegate void OnTickHandler(object sender, OnTickEventArgs eventArgs);

    public delegate void ShaderDelegate();

    public class GameController : Game
    {
        public const int CHUNK_LOAD_RANGE = 2;

        public int Id { get; } = -1;

        public GameModel GameModel { get; set; }
        public GameView GameView { get; set; }

        private GameScenario.GameScenario scenario;

        public EntitySpawner Spawner;

        public IMapAction SelectedMapAction => MapActionSelector.SelectedMapAction;
        public readonly MapActionSelector MapActionSelector;

        public readonly AnimationController AnimationController;

        public GameTime LastUpdateGameTime { get; private set; }

        public MouseInput MouseInput { get; set; }

        public GameActionGuiController GameActionGui { get; set; }
        private BottomBarView bottomBarView;
        public FogController FogController { get; set; }

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

        public readonly TimeController TimeController = new TimeController();

        public Faction_Controller PlayerFaction;
        public CPU_Controller CPU1;

        public event MouseStateObserver MouseStateChange;

        public MouseState PreviousMouseButtonsStatus { get; set; }

        public virtual event OnTickHandler onTick;

        // testcode
        public bool F7Pressed { get; set; } = false;


        //    GameState and its event
        private GameState gameState;

        public GameState GameState
        {
            get => gameState;
            set
            {
                gameState = value;
                GameStateChange?.Invoke(this, new EventArgsWithPayload<GameState>(gameState)); //Invoke event if has subscribers
            }
        }

        public event GameStateObserver GameStateChange;

        private readonly GraphicsDeviceManager graphicsDeviceManager;

        public CameraController Camera;

        private ShaderDelegate shader;

        #region FPS-debug-info

        private int ThisSecond;
        private int FramesThisSecond;
        private int FramesOutput;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scenario">Game-scenario. Null for default scenario</param>
        /// <param name="gameSpeed">Game's default speed</param>
        /// <param name="gameState">Game's default state</param>
        public GameController(GameScenario.GameScenario scenario, GameSpeed gameSpeed = GameSpeed.Regular, GameState gameState = GameState.Paused)
        {
            this.GameSpeed = gameSpeed;
            this.GameState = gameState;

            GameModel = new GameModel();

            GameStateChange += PauseGame;
            GameStateChange += UnPauseGame;

            AnimationController = new AnimationController();


            MapActionSelector = new MapActionSelector();

            graphicsDeviceManager = new GraphicsDeviceManager(this);

            shader = RandomPattern2;

            Content.RootDirectory = "Content";

            FactionFactory factionFactory = new FactionFactory(this);

            this.scenario = scenario ?? GameScenario.GameScenario.DefaultScenario;

            PlayerFaction = factionFactory.CreatePlayerFaction("PlayerFaction", this.scenario.StartingBalance);

            this.scenario.Initialise(this, PlayerFaction);
        }

        /// <summary>
        /// Is subscribed to the GameState so this is called every time the GameState is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public void PauseGame(object sender, EventArgsWithPayload<GameState> eventArgs)
        {
            if (eventArgs.Value != GameState.Paused) return;
            //throw new NotImplementedException();
            Console.WriteLine("pause");
        }

        /// <summary>
        /// Is subscribed to the gamestate so this is called every time the gamestate is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public void UnPauseGame(object sender, EventArgsWithPayload<GameState> eventArgs)
        {
            if (eventArgs.Value != GameState.Running) return;
            //throw new NotImplementedException();
            Console.WriteLine("unpause");
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
            GameModel.World = WorldFactory.GetNewWorld(FastNoise.NoiseType.SimplexFractal);

            // Create CPU player
            FactionFactory factionFactory = new FactionFactory(this);
            CPU1 = CPU_Factory.CreateSimpleCpu(factionFactory.CreateCPUFaction("CPU1"));

            //GameModel.Factions.Add((SimpleAI)(CPU1.CpuModel.AI).Faction);

            FogController = new FogController(PlayerFaction, GameModel.World);


            // Pathfinder 
            GameModel.pathfinder = new Pathfinder(GameModel.World);

            //    Animation-frame-loading
            onTick += (sender, args) =>
            {
                List<IViewItem> frameViewItems = AnimationController.NextFrame;
                GameModel.ItemList.AddRange(frameViewItems.OfType<IViewImage>());
                GameModel.TextList.AddRange(frameViewItems.OfType<IViewText>());
            };

            // Spawner
            Spawner = new EntitySpawner(this);

//            GameModel.ActionBox = new ActionBoxController(new FloatCoords() {x = 50, y = 50});

            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

            Camera = new CameraController(GraphicsDevice);

            GameView = new GameView(GameModel, graphicsDeviceManager, spriteBatch, Camera, GraphicsDevice, Content);

            GameActionGui = new GameActionGuiController(this);

            GameActionGui.SetActions(new List<List<IGameAction>>
            {
                scenario.BaseActions.ToList()
            });

            GameModel.MouseInput = new MouseInput(this);

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
            bottomBarView = new BottomBarView(GraphicsDevice);


            //TESTCODE

            onTick += SetBuilding;
            onTick += TimeController.UpdateTime;
            onTick += GameModel.MouseInput.Selection.Update;
            GameModel.MouseInput.Selection.OnSelectionChanged += ChangeSelection;
            GameModel.MouseInput.Selection.OnSelectionChanged += bottomBarView.UpdateHUDOnSelect;

            //TESTCODE
            DBController.OpenConnection("DefDex.db");
            UnitDef unitdef = DBController.GetUnitDef(1);
            UnitDef unitdef2 = DBController.GetUnitDef(2);
            DBController.CloseConnection();

            // Get coords of a cell that has terrain a unit is allowed to walk on
            Dictionary<int, Coords> itterator = new Dictionary<int, Coords>();
            itterator.Add(0, new Coords {x = 0, y = 1});
            itterator.Add(1, new Coords {x = 1, y = 0});
            itterator.Add(2, new Coords {x = 0, y = -1});
            itterator.Add(3, new Coords {x = -1, y = 0});


            List<Coords> CheckedCoords = new List<Coords>();
            bool IsValid(Coords coords) => GameModel.World.GetCellFromCoords(coords).worldCellModel.Terrain != TerrainType.Water;
            bool Checked(Coords coords) => CheckedCoords.Contains(coords);

            int i = 0;
            Coords checkCoords = new Coords();
            Coords currentCoords = new Coords();
            while (!IsValid(currentCoords))
            {
                if (!Checked(checkCoords)) i++;

                Coords relativeCoords = itterator[i % 4];
                currentCoords = checkCoords;
                checkCoords = checkCoords + relativeCoords;
                CheckedCoords.Add(currentCoords);
            }

            UnitFactory unitFactory = new UnitFactory(PlayerFaction, this);

            UnitController unit = unitFactory.CreateNewUnit(unitdef);

            unit.LocationController.chunkChanged += LoadNewChunks;
            unit.LocationController.LocationModel.UnwalkableTerrain.Add(TerrainType.Water);
            Spawner.SpawnUnit(unit, currentCoords);
            Camera.LookAt(new Vector2(currentCoords.x * GameView.TileSize, currentCoords.y * GameView.TileSize));


            // Create a unit for the CPU1 Faction
            /*UnitFactory unitFactory2 = new UnitFactory(, this);

            FloatCoords coords2 = new FloatCoords() { x = 20, y = 20 };
            UnitController unit2 = unitFactory2.CreateNewUnit(unitdef2);
            Spawner.SpawnUnit(unit2, (Coords)coords2);*/

            //============= More TestCode ===============

            MouseStateChange += GameModel.MouseInput.OnMouseStateChange;
//            MouseStateChange += GameModel.ActionBox.OnRightClick;
            //TESTCODE

            // NOT TESTCODE
            onTick += (sender, args) =>
            {
                MouseState mouseState = Mouse.GetState();

                FloatCoords mouseCellCoords = WorldPositionCalculator.WindowCoordsToCellCoords(new Coords()
                {
                    x = mouseState.X,
                    y = mouseState.Y
                }, Camera.GetViewMatrix(), GameView.TileSize);

                IWorldEntity worldEntity;
                if (!CellContainsIWorldEntity(mouseCellCoords, out worldEntity)) return;

                GameModel.ItemList.Add(new SelectableImage(worldEntity));
            };
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
            GameState = GameState.Paused;

            //throw new NotImplementedException();
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
                (Coords) WorldPositionCalculator.TransformWindowCoords(
                    windowCoords,
                    Camera.GetViewMatrix()
                ),
                GameView.TileSize
            );

            Coords chunkCoords = WorldPositionCalculator.ChunkCoordsOfCellCoords(cellCoords);


            if (ChunkExists(chunkCoords)) return;

            GameModel.World.WorldModel.ChunkGrid[chunkCoords] = WorldChunkLoader.ChunkGenerator(chunkCoords);
            shader();
        }

        public void LoadNewChunks(object sender, EventArgsWithPayload<Coords> eventArgs)
        {
            for (int x = -CHUNK_LOAD_RANGE; x <= CHUNK_LOAD_RANGE; x++)
            {
                for (int y = -CHUNK_LOAD_RANGE; y <= CHUNK_LOAD_RANGE; y++)
                {
                    Coords chunkCoords = new Coords {x = x, y = y} + eventArgs.Value;
                    if (ChunkExists(chunkCoords)) continue;

                    GameModel.World.WorldModel.ChunkGrid[chunkCoords] = WorldChunkLoader.ChunkGenerator(chunkCoords);
                    shader();
                }
            }

            if (!GameModel.FogEnabled)
            {
                FogController.SetEverything(ViewMode.Full);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private bool ChunkExists(Coords chunkCoords) => GameModel.World.WorldModel.ChunkGrid.ContainsKey(chunkCoords) &&
                                                        GameModel.World.WorldModel.ChunkGrid[chunkCoords] != null;

        public bool CellContainsIWorldEntity(FloatCoords coords, out IWorldEntity targetable)
        {
            IEnumerable<TITargetableType> CellsITargetable<TITargetableType>(IEnumerable<TITargetableType> source) where TITargetableType : ITargetable =>
                from item in source
                where (Coords) item.FloatCoords == (Coords) coords
                select item;

            //    IStructure
            IStructure<IStructureDef> cellsStructure;
            bool cellContainsStructure = GameModel.World.CellContainsStructure((Coords) coords, out cellsStructure);

            //    Unit
            IEnumerable<UnitController> cellsUnit = CellsITargetable(GameModel.World.WorldModel.Units).ToList();
            bool cellContainsUnit = cellsUnit.Any();

            bool result = cellContainsStructure || cellContainsUnit;

            targetable = null;
            if (!result) return false;

            targetable = cellContainsUnit ? (IWorldEntity) cellsUnit.First() : cellsStructure;

            return true;
        }

        /// <summary>
        /// This function and its actions need to be refactored
        /// </summary>
        public void AddGui()
        {
            StatusBarView statusBarView = new StatusBarView(GraphicsDevice);
            LeftButtonBar leftButtonBar = new LeftButtonBar(GraphicsDevice);
            RightButtonBar rightButtonBar = new RightButtonBar(GraphicsDevice);
            MiniMapBar miniMap = new MiniMapBar(GraphicsDevice);
            GameActionGuiView actionBar = GameActionGui.View;

            GameModel.GuiItemList.Add(statusBarView);
            GameModel.GuiItemList.Add(leftButtonBar);
            GameModel.GuiItemList.Add(rightButtonBar);
            GameModel.GuiItemList.Add(bottomBarView);
            GameModel.GuiItemList.Add(miniMap);
            GameModel.GuiItemList.Add(actionBar);

            List<IGuiViewImage> addList = new List<IGuiViewImage>();
            foreach (IGuiViewImage guiViewImage in GameModel.GuiItemList)
            {
                addList.AddRange(guiViewImage.GetContents());
            }

            GameModel.GuiItemList.AddRange(addList);
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Exit game if escape is pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                GameState = GameState.Paused;
                //                SaveToDB();
                //                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z)) GameState = GameState.Running;


            // Updates camera according to the pressed buttons
            Camera.MoveCamera();

            //    Clear gui-item-list always
            GameModel.GuiItemList.Clear();
            GameModel.GuiTextList.Clear();
            AddGui();
            // ============== Temp Code ===================================================================


            MouseState temp = Mouse.GetState();
            Coords tempcoords = new Coords {x = temp.X, y = temp.Y};
            Coords coords = (Coords) WorldPositionCalculator.WindowCoordsToCellCoords(tempcoords, Camera.GetViewMatrix(), GameView.TileSize);
            if (GameModel.World.GetCellFromCoords(coords) != null)
            {
                TerrainTester terrainTester = new TerrainTester(new FloatCoords() {x = 0, y = 100})
                {
                    Text = $"{coords.x},{coords.y}  {GameModel.World.GetCellFromCoords(coords).worldCellModel.Terrain.ToString()}"
                };
                terrainTester.Colour = GameModel.World.GetCellFromCoords(coords).worldCellModel.BuildingOnTop != null ? Color.Red : Color.Blue;


                GameModel.GuiTextList.Add(terrainTester);

                TerrainTester tester = new TerrainTester(new FloatCoords {x = 0, y = 120})
                {
                    Text = $"Chunk: {WorldPositionCalculator.ChunkCoordsOfCellCoords((FloatCoords) coords).x},{WorldPositionCalculator.ChunkCoordsOfCellCoords((FloatCoords) coords).y} "
                };

                GameModel.GuiTextList.Add(tester);
                TerrainTester fogtester = new TerrainTester(new FloatCoords {x = 0, y = 140})
                {
                    Text = $"view: {GameModel.World.GetCellFromCoords(coords).worldCellView.ViewMode}"
                };

                GameModel.GuiTextList.Add(fogtester);
            }


            if (gameState == GameState.Paused) return;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            LastUpdateGameTime = gameTime;
            // Clears the itemList

            GameModel.ItemList.Clear();
            GameModel.TextList.Clear();


            // Update Buildings & constructing buildings on screen
            GameModel.ItemList.AddRange(GameModel.World.WorldModel.Structures.Select(building => building.View));
            GameModel.TextList.AddRange(GameModel.World.WorldModel.UnderConstruction.Select(building => building.Counter));

            //    Update Units on screen
            GameModel.ItemList.AddRange(GameModel.World.WorldModel.Units.Select(unit => unit.View));

//            if (GameModel.ActionBox.BoxModel.Show)
//            {
//                GameModel.GuiItemList.Add(GameModel.ActionBox.BoxView);
//                GameModel.GuiTextList.Add(GameModel.ActionBox.BoxModel.Text);
//            }

            //    Calculate viewport-bounds
            Coords leftTopViewBound = (Coords) WorldPositionCalculator.WindowCoordsToCellCoords(new Coords
            {
                x = GraphicsDevice.Viewport.X,
                y = GraphicsDevice.Viewport.Y
            }, Camera.GetViewMatrix(), GameView.TileSize);
            Coords rightBottomViewBound = (Coords) WorldPositionCalculator.WindowCoordsToCellCoords(new Coords
            {
                x = GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width,
                y = GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height
            }, Camera.GetViewMatrix(), GameView.TileSize);
            Rectangle viewRectangle = new Rectangle(leftTopViewBound.x, leftTopViewBound.y,
                Math.Abs(leftTopViewBound.x - rightBottomViewBound.x),
                Math.Abs(leftTopViewBound.y - rightBottomViewBound.y));

            List<WorldChunkController> chunks = (from chunk in GameModel.World.WorldModel.ChunkGrid
                let rightBottomBound = new Coords
                {
                    x = 20 + WorldChunkModel.ChunkSize,
                    y = 20
                }
                let leftTopBound = new Coords
                {
                    x = (chunk.Key.x * WorldChunkModel.ChunkSize),
                    y = (chunk.Key.y * WorldChunkModel.ChunkSize)
                }
                let chunkRectangle = new Rectangle(leftTopBound.x, leftTopBound.y,
                    Math.Abs(rightBottomBound.x),
                    Math.Abs(rightBottomBound.y)
                )
                where (chunkRectangle.Intersects(viewRectangle))
                select chunk.Value).ToList();

            foreach (WorldChunkController chunk in chunks)
            {
                GameModel.ItemList.AddRange(from WorldCellController cell in chunk.WorldChunkModel.grid where cell.worldCellView.ViewMode != ViewMode.None select cell.worldCellView);
            }

            GameModel.GuiTextList.Add(PlayerFaction.CurrencyController.View);

            // Chunks generate when hovered over
            //mouseChunkLoadUpdate(gameTime);

            // fire Ontick event
            Stopwatch tick_stopwatch = new Stopwatch();
            tick_stopwatch.Start();


            if (GameModel.FogEnabled) FogController.UpdateViewModes(ViewMode.Fog);


            OnTickEventArgs args = new OnTickEventArgs(gameTime);
            onTick?.Invoke(this, args);


            if (GameModel.FogEnabled) FogController.UpdateViewModes(ViewMode.Full);

            tick_stopwatch.Stop();


            //updates the viewmode for everything on screen

            // comment line bellow to turn on fog
            //FogController.UpdateEverythingVisible();

            // Calls the game update

            //======= Fire MOUSESTATE ================
            MouseState currentMouseButtonsStatus = Mouse.GetState();
            if (currentMouseButtonsStatus.LeftButton != PreviousMouseButtonsStatus.LeftButton ||
                currentMouseButtonsStatus.RightButton != PreviousMouseButtonsStatus.RightButton)
            {
                MouseStateChange?.Invoke(this, new EventArgsWithPayload<MouseState>(currentMouseButtonsStatus));
                PreviousMouseButtonsStatus = currentMouseButtonsStatus;
            }

            AddShader();

            if (Keyboard.GetState().IsKeyDown(Keys.S)) SaveToDB();

            if (Keyboard.GetState().IsKeyDown(Keys.F7) && !F7Pressed)
            {
                GameModel.FogEnabled = !GameModel.FogEnabled;
                if (!GameModel.FogEnabled)
                {
                    FogController.SetEverything(ViewMode.Full);
                }
                else
                {
                    FogController.SetEverything(ViewMode.None);
                }

                F7Pressed = true;
            }

            if (!Keyboard.GetState().IsKeyDown(Keys.F7) && F7Pressed)
            {
                F7Pressed = false;
            }

            stopwatch.Stop();

            // Calls the game update
            base.Update(gameTime);

            //Console.Clear();
            //printStopWatchResults(tick_stopwatch, "OnTick");
            //printStopWatchResults(stopwatch, "Update");
            //Console.WriteLine($"OnTick's percentage: {(tick_stopwatch.Elapsed.Ticks / (float)stopwatch.Elapsed.Ticks) * 100}%");
            //Console.WriteLine("frames: " + FramesOutput);
        }

        public static void printStopWatchResults(Stopwatch toPrint, string description) => Console.WriteLine($"{description} took: {toPrint.Elapsed.Ticks} ticks or {toPrint.Elapsed.Milliseconds} ms");

        private void updateFrames(GameTime gameTime)
        {
            if (ThisSecond < gameTime.TotalGameTime.Seconds)
            {
                ThisSecond = gameTime.TotalGameTime.Seconds;
                FramesOutput = FramesThisSecond;
                FramesThisSecond = 0;
            }

            FramesThisSecond++;
        }

        /// <summary>
        /// This checks if a new shader needs to be applied and applies shader to new chunks
        /// </summary>
        private void AddShader()
        {
            ShaderDelegate tempShader = null;

            if (Keyboard.GetState().IsKeyDown(Keys.R)) tempShader = RandomPattern2;
            if (Keyboard.GetState().IsKeyDown(Keys.C)) tempShader = CellChunkCheckered;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) tempShader = DefaultPattern;

            if (tempShader == null) return;

            shader = tempShader;
            shader();
        }

        /// <summary>
        /// TestCode
        /// </summary>
        public void SetBuilding(object sender, OnTickEventArgs eventArgs)
        {
            //  IMPORTANT NOTE this method is only here to scavenge off of later. Don't delete it just yet, as we might need it later.
            void CheckKeysAndPlaceBuilding(bool isKeyPressed, int buildingId, MouseState mouseState, List<TerrainType> legalTerrainTypes)
            {
                if (!isKeyPressed) return;

                DBController.OpenConnection("DefDex.db");
                BuildingDef def = DBController.GetBuildingDef(buildingId);
                DBController.CloseConnection();

                Coords tempCoords = new Coords {x = mouseState.X, y = mouseState.Y};

                Coords coords = (Coords) WorldPositionCalculator.WindowCoordsToCellCoords(tempCoords, Camera.GetViewMatrix(), GameView.TileSize);

                List<Coords> buildingCoords = new List<Coords>();
                foreach (Coords buildingShape in def.BuildingShape) buildingCoords.Add(coords + buildingShape);

                if (!GameModel.World.AreTerrainCellsLegal(buildingCoords, legalTerrainTypes)) return;

                using (ConstructingBuildingFactory constructionFactory = new ConstructingBuildingFactory(PlayerFaction))
                {
                    ConstructingBuildingController building = constructionFactory.CreateConstructingBuildingControllerOf(def);
                    Spawner.SpawnStructure(coords, building);
                    
                }
            }
        }

        private void AddGameActionTab()
        {
        }

        public void ChangeSelection(object sender, EventArgsWithPayload<Selection_Controller> eventArgs)
        {
            List<IGameActionHolder> gameActionHolders = eventArgs.Value.SelectedItems;
            List<List<IGameAction>> gameActionTabModels = eventArgs.Value.SelectedItems.Any()
                ? gameActionHolders.Select(item => new List<IGameAction>(item.GameActions)).ToList()
                : new List<List<IGameAction>>
                {
                    scenario.BaseActions.ToList()
                };
            
            foreach (IGameActionHolder gameActionHolder in gameActionHolders)
            {
                if (!gameActionHolder.GameActions.OfType<SelectMapAction_GameAction>().Any()) continue;
                MapActionSelector.Select(gameActionHolder.GameActions.OfType<SelectMapAction_GameAction>().First().MapAction);
            }

            GameActionGui.SetActions(gameActionTabModels);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            GameView.Draw();

            // Calls the game's draw function
            base.Draw(gameTime);

            stopwatch.Stop();
            updateFrames(gameTime);
            printStopWatchResults(stopwatch, "Drawing");


            //            Uncomment the line below to show all view-items in the console
            //            Console.Clear();
            //            Console.WriteLine(this.GameModel.AllDrawItems.GroupBy(item => item.GetType(), (typeKey, typeSource) => new KeyValuePair<string, int>(typeKey.Name, typeSource.Count())).Select((pair => $"{pair.Key}: {pair.Value}")).Aggregate((s, s1) => $"{s}\n{s1}"));
        }


        // ===========================================================================================================================
        /// <summary>
        /// Draws the chunks and cells in a Checkered pattern for easy debugging
        /// </summary>
        public void CellChunkCheckered()
        {
            foreach (var Chunk in GameModel.World.WorldModel.ChunkGrid)
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
            foreach (var Chunk in GameModel.World.WorldModel.ChunkGrid)
            {
                foreach (var item2 in Chunk.Value.WorldChunkModel.grid)
                {
                    item2.worldCellView.Colour = Color.White;
                }
            }
        }

        public void RandomPattern2()
        {
            Random random = new Random(WorldModel.Seed);

            foreach (var Chunk in GameModel.World.WorldModel.ChunkGrid)
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