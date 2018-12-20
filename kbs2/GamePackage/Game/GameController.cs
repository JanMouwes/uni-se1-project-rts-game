using System;
using System.Collections.Generic;
using System.Timers;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Desktop.View.Camera;
using kbs2.GamePackage.DayCycle;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
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
using kbs2.Actions.GameActionGrid;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.UserInterface.GameActionGui;
using kbs2.WorldEntity.Building.BuildingMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;
using kbs2.WorldEntity.WorldEntitySpawner;
using MonoGame.Extended.Timers;

namespace kbs2.GamePackage
{
    public delegate void GameSpeedObserver(object sender, GameSpeedEventArgs eventArgs);

    public delegate void GameStateObserver(object sender, EventArgsWithPayload<GameState> eventArgs);

    public delegate void MouseStateObserver(object sender, EventArgsWithPayload<MouseState> e);

    public delegate void OnTick(object sender, OnTickEventArgs eventArgs);

    public delegate void ShaderDelegate();

    public class GameController : Game
    {
        public int Id { get; } = -1;

        public GameModel GameModel { get; set; } = new GameModel();
        public GameView GameView { get; set; }
        public EntitySpawner Spawner;

        public GameTime LastUpdateGameTime { get; private set; }

        public MouseInput MouseInput { get; set; }

        public const int TicksPerSecond = 30;

        public static int TickIntervalMilliseconds => 1000 / TicksPerSecond;

        private Timer gameTimer; //TODO

        public GameActionGuiController GameActionGui { get; set; }
        public bool PreviousQPressed { get; set; }
        public bool APressed { get; set; }
        public Terraintester Terraintester { get; set; }

        public event ElapsedEventHandler GameTick
        {
            add => gameTimer.Elapsed += value;
            remove => gameTimer.Elapsed -= value;
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

        public CameraController Camera;

        private ShaderDelegate shader;

        public GameController(GameSpeed gameSpeed, GameState gameState)
        {
            this.GameSpeed = gameSpeed;
            this.GameState = gameState;

            GameStateChange += PauseGame;
            GameStateChange += UnPauseGame;

            graphicsDeviceManager = new GraphicsDeviceManager(this);
            
            GameActionGui = new GameActionGuiController(this);

            shader = RandomPattern2;

            Content.RootDirectory = "Content";
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
            GameModel.World = WorldFactory.GetNewWorld(FastNoise.NoiseType.Simplex);

            // Pathfinder 
            GameModel.pathfinder = new Pathfinder(GameModel.World.WorldModel, 500);

            // Spawner
            Spawner = new EntitySpawner(this);

            GameModel.ActionBox = new ActionBoxController(new FloatCoords() {x = 50, y = 50});

            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

            Camera = new CameraController(GraphicsDevice);

            GameView = new GameView(GameModel, graphicsDeviceManager, spriteBatch, Camera, GraphicsDevice, Content);

            gameTimer = new Timer(TickIntervalMilliseconds);

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
            //TESTCODE
            PreviousQPressed = false;
            APressed = false;
            Terraintester = new Terraintester();


            onTick += SetBuilding;
            onTick += f.UpdateTime;
            onTick += GameModel.MouseInput.Selection.Update;
            GameModel.MouseInput.Selection.onSelectionChanged += ChangeSelection;

            StatusBarView statusBarView = new StatusBarView(this);
            LeftButtonBar leftButtonBar = new LeftButtonBar(this);
            RightButtonBar rightButtonBar = new RightButtonBar(this);

            BottomBarView bottomBarView = new BottomBarView(this);
            MiniMapBar miniMap = new MiniMapBar(this);
            ActionBarView actionBar = new ActionBarView(this);

            GameActionGuiController gameActionUI = new GameActionGuiController(this);

            GameModel.GuiItemList.Add(statusBarView);
            GameModel.GuiItemList.Add(leftButtonBar);
            GameModel.GuiItemList.Add(rightButtonBar);
            GameModel.GuiItemList.Add(bottomBarView);
            GameModel.GuiItemList.Add(miniMap);
            GameModel.GuiItemList.Add(actionBar);


            //TESTCODE
            DBController.OpenConnection("DefDex.db");
            UnitDef unitdef = DBController.GetDefinitionFromUnit(1);
            DBController.CloseConnection();

            for (int i = 0; i < 12; i++)
            {
                UnitController unit =
                    UnitFactory.CreateNewUnit(unitdef, new FloatCoords {x = i, y = 5}, GameModel.World.WorldModel);

                unit.UnitModel.Speed = 0.05f;
                unit.LocationController.LocationModel.UnwalkableTerrain.Add(TerrainType.Water);
                Spawner.SpawnUnit(unit, PlayerFaction);
                PlayerFaction.currency_Controller.AddUpkeepCost(unitdef.Upkeep);
            }

            //============= More TestCode ===============

            MouseStateChange += GameModel.MouseInput.OnMouseStateChange;
            MouseStateChange += GameModel.ActionBox.OnRightClick;
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
                WorldPositionCalculator.TransformWindowCoords(
                    windowCoords,
                    Camera.GetViewMatrix()
                ),
                GameView.TileSize
            );


            loadChunkIfUnloaded(WorldPositionCalculator.ChunkCoordsOfCellCoords(cellCoords));
        }

        /// <summary>
        /// 
        /// </summary>
        private bool chunkExists(Coords chunkCoords) => GameModel.World.WorldModel.ChunkGrid.ContainsKey(chunkCoords) &&
                                                        GameModel.World.WorldModel.ChunkGrid[chunkCoords] != null;

        /// <summary>
        /// 
        /// </summary>
        private void loadChunkIfUnloaded(Coords chunkCoords)
        {
            if (chunkExists(chunkCoords)) return;

            GameModel.World.WorldModel.ChunkGrid[chunkCoords] = WorldChunkLoader.ChunkGenerator(chunkCoords);

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                SaveToDB();
                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z)) GameState = GameState.Running;

            if (gameState == GameState.Paused) return;

            LastUpdateGameTime = gameTime;

            // Updates camera according to the pressed buttons
            Camera.MoveCamera();

            // ============== Temp Code ===================================================================

            MouseState temp = Mouse.GetState();
            Coords tempcoords = new Coords {x = temp.X, y = temp.Y};
            Coords coords = WorldPositionCalculator.DrawCoordsToCellCoords(WorldPositionCalculator.TransformWindowCoords(tempcoords, Camera.GetViewMatrix()), GameView.TileSize);
            if (GameModel.World.GetCellFromCoords(coords) != null)
            {
                Terraintester.Text = $"{coords.x},{coords.y}  {GameModel.World.GetCellFromCoords(coords).worldCellModel.Terrain.ToString()}";
                Terraintester.Colour = GameModel.World.GetCellFromCoords(coords).worldCellModel.BuildingOnTop != null ? Color.Red : Color.Blue;
            }

            GameModel.GuiTextList.Add(Terraintester);

            // Update Buildings on screen
            List<IViewImage> buildings = (from structure in GameModel.World.WorldModel.Structures
                let building = structure as BuildingController
                select building.View as IViewImage).ToList();

            GameModel.ItemList.AddRange(buildings);


            List<IViewImage> constructingBuildingViews = new List<IViewImage>();
            List<IViewText> counters = new List<IViewText>();
            foreach (ConstructingBuildingController building in GameModel.World.WorldModel.UnderConstruction)
            {
                constructingBuildingViews.Add(building.ConstructingBuildingView);
                counters.Add(building.Counter);
            }

            GameModel.ItemList.AddRange(constructingBuildingViews);
            GameModel.TextList.AddRange(counters);


            List<IViewImage> units = (from unit in GameModel.World.WorldModel.Units select unit.UnitView).Cast<IViewImage>().ToList();

            GameModel.ItemList.AddRange(units);

            if (GameModel.ActionBox.BoxModel.Show)
            {
                GameModel.ItemList.Add(GameModel.ActionBox.BoxView);
                GameModel.TextList.Add(GameModel.ActionBox.BoxModel.Text);
            }

            //    FIXME Is this still up to date?
            int tileSize = (int) (GraphicsDevice.Viewport.Width / Camera.CameraModel.TileCount);

            List<IViewImage> cells = new List<IViewImage>();
            List<WorldChunkController> chunks = (from chunk in GameModel.World.WorldModel.ChunkGrid
                let rightBottomViewBound = WorldPositionCalculator.DrawCoordsToCellCoords(
                    WorldPositionCalculator.TransformWindowCoords(
                        new Coords
                        {
                            x = GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width,
                            y = GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height
                        }, Camera.GetViewMatrix()), tileSize)
                let topLeftViewBound = WorldPositionCalculator.DrawCoordsToCellCoords(
                    WorldPositionCalculator.TransformWindowCoords(
                        new Coords
                        {
                            x = GraphicsDevice.Viewport.X,
                            y = GraphicsDevice.Viewport.Y
                        }, Camera.GetViewMatrix()), tileSize)
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
                    (rightBottomBound.x < 0 ? rightBottomBound.x * -1 : rightBottomBound.x),
                    (rightBottomBound.y < 0 ? rightBottomBound.y * -1 : rightBottomBound.y))
                let viewRectangle = new Rectangle(topLeftViewBound.x, topLeftViewBound.y,
                    Math.Abs(topLeftViewBound.x - rightBottomViewBound.x),
                    Math.Abs(topLeftViewBound.y - rightBottomViewBound.y))
                where (chunkRectangle.Intersects(viewRectangle))
                select chunk.Value).ToList();
            //Console.WriteLine(chunks.Count);

            foreach (WorldChunkController chunk in chunks)
            {
                cells.AddRange(from WorldCellController cell in chunk.WorldChunkModel.grid
                    select cell.worldCellView);
            }

            GameModel.ItemList.AddRange(cells);


            GameModel.GuiTextList.Add(PlayerFaction.currency_Controller.view);
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
            MouseState currentMouseButtonsStatus = Mouse.GetState();
            if (currentMouseButtonsStatus.LeftButton != PreviousMouseButtonsStatus.LeftButton ||
                currentMouseButtonsStatus.RightButton != PreviousMouseButtonsStatus.RightButton)
            {
                MouseStateChange?.Invoke(this, new EventArgsWithPayload<MouseState>(currentMouseButtonsStatus));
                PreviousMouseButtonsStatus = currentMouseButtonsStatus;
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
            bool CheckKeysAndPlaceBuilding(bool isKeyPressed, bool wasKeyPressed, int buildingId, MouseState mouseState, List<TerrainType> legalTerrainTypes)
            {
                //FIXME temp
                //if (isKeyPressed == wasKeyPressed) return wasKeyPressed;

                if (!isKeyPressed) return true;

                DBController.OpenConnection("DefDex.db");
                IStructureDef def = DBController.GetDefinitionBuilding(buildingId);
                DBController.CloseConnection();

                Coords tempCoords = new Coords {x = mouseState.X, y = mouseState.Y};

                Coords coords = WorldPositionCalculator.DrawCoordsToCellCoords(WorldPositionCalculator.TransformWindowCoords(tempCoords, Camera.GetViewMatrix()), GameView.TileSize);

                List<Coords> buildingCoords = new List<Coords>();
                foreach (Coords buildingShape in def.BuildingShape) buildingCoords.Add(coords + buildingShape);

                if (!GameModel.World.AreTerrainCellsLegal(buildingCoords, legalTerrainTypes)) return true;

                using (ConstructingBuildingFactory constructionFactory = new ConstructingBuildingFactory(PlayerFaction))
                {
                    ViewValues viewValues = new ViewValues(ConstructingBuildingFactory.ConstructionImageSource, def.ViewValues.Width, def.ViewValues.Height);

                    ConstructingBuildingDef buildingDef = new ConstructingBuildingDef(def, 20) {BuildingShape = def.BuildingShape, ViewValues = viewValues};

                    ConstructingBuildingController building = constructionFactory.CreateBUC(buildingDef);

                    Spawner.SpawnStructure(coords, building);

                    onTick += building.Update;

                    building.ConstructionComplete += (o, args) =>
                    {
                        if (!(o is ConstructingBuildingController structure)) return;

                        onTick -= structure.Update;
                        GameModel.World.RemoveStructure(structure);

                        Spawner.SpawnStructure(structure.StartCoords, BuildingFactory.CreateNewBuilding((BuildingDef) structure.Def.CompletedBuildingDef));
                    };
                }

                return true;
            }

            bool isQPressed = Keyboard.GetState().IsKeyDown(Keys.D1);

            KeyboardState keyboardState = Keyboard.GetState();

            List<TerrainType> terrainList = new List<TerrainType>()
            {
                TerrainType.Grass,
                TerrainType.Default
            };

            if (isQPressed)
            {
                PreviousQPressed = CheckKeysAndPlaceBuilding(isQPressed, PreviousQPressed, 2, Mouse.GetState(), terrainList);
                return;
            }

            if (keyboardState.IsKeyDown(Keys.D3))
            {
                PreviousQPressed = CheckKeysAndPlaceBuilding(keyboardState.IsKeyDown(Keys.D3), PreviousQPressed, 3, Mouse.GetState(), terrainList);
                return;
            }

            if (keyboardState.IsKeyDown(Keys.D4))
            {
                PreviousQPressed = CheckKeysAndPlaceBuilding(keyboardState.IsKeyDown(Keys.D4), PreviousQPressed, 4, Mouse.GetState(), terrainList);
                return;
            }

            if (keyboardState.IsKeyDown(Keys.D5))
            {
                PreviousQPressed = CheckKeysAndPlaceBuilding(keyboardState.IsKeyDown(Keys.D5), PreviousQPressed, 5, Mouse.GetState(), terrainList);
                return;
            }

            if (keyboardState.IsKeyDown(Keys.D6))
            {
                PreviousQPressed = CheckKeysAndPlaceBuilding(keyboardState.IsKeyDown(Keys.D6), PreviousQPressed, 6, Mouse.GetState(), terrainList);
                return;
            }
        }

        public void ChangeSelection(object sender, EventArgsWithPayload<List<IHasGameActions>> eventArgs)
        {
            List<IGameAction> actions = eventArgs.Value.First().GameActions;
            IGameAction[] actionArray = actions.ToArray();
            GameActionGui.SetActions(new List<GameActionTabModel> {new GameActionTabModel(actionArray)});
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GameView.Draw();

            // Calls the game's draw function
            base.Draw(gameTime);
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
            Random random = new Random(GameModel.World.WorldModel.seed);

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