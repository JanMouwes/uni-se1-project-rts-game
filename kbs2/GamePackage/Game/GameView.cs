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
using kbs2.World.Chunk;
using kbs2.World.Structs;
using kbs2.World.TerrainDef;
using kbs2.World.World;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingMVC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace kbs2.GamePackage
{
    public class GameView : Game
    {
        private GameModel gameModel;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private CameraController Camera;

        // List for drawing items with the camera offset
        public List<IViewable> DrawList;
        // List for drawing items without offset
        public List<IViewable> DrawStaticList;

        // Calculate the size (Width) of a tile
        public int TileSize => (int)(GraphicsDevice.Viewport.Width / Camera.CameraModel.TileCount);

        // Constructor
        public GameView(GameModel gameModel)
        {
            this.gameModel = gameModel;
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


            //TESTCODE
            BuildingDef def = new BuildingDef();
            def.BuildingShape = new List<Coords>
            {
                new Coords { x = 0, y = 0 },
                new Coords { x = 1, y = 0 },
                new Coords { x = 1, y = -1 },
                new Coords { x = 0, y = -1 }
            };
            def.height = 2f;
            def.width = 2f;
            def.imageSrc = "TrainingCenter";
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
            Camera.MoveCamera();

            // ============== Temp Code ===================================================================
            // Updates cells on screen 
            GetCellsOnScreen();

            // Update Buildings on screen
            List<IViewable> buildings = new List<IViewable>();
            foreach(Building_Controller building in gameModel.World.WorldModel.buildings)
            {
                buildings.Add(building.View);
            }
            DrawList.AddRange( GetOnScreen(buildings,GraphicsDevice.Viewport,Camera.GetInverseViewMatrix()));

            // ======================================================================================

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

            var DrawItems = from Item in DrawList
                            orderby Item.ZIndex ascending
                            select Item;

            foreach (IViewable DrawItem in DrawItems)
            {
                if (DrawItem == null) continue;
                Texture2D texture = this.Content.Load<Texture2D>(DrawItem.Texture);
                spriteBatch.Draw(texture, new Rectangle((int)DrawItem.Coords.x * TileSize, (int)DrawItem.Coords.y * TileSize, (int)(DrawItem.Width * TileSize), (int)(DrawItem.Height * TileSize)), DrawItem.Color);
            }

            spriteBatch.End();
        }

        // Draws every item in the DrawList without offset
        private void DrawStationairy()
        {
            spriteBatch.Begin();

            var DrawItems = from Item in DrawStaticList
                            orderby Item.ZIndex ascending
                            select Item;

            foreach (IViewable DrawItem in DrawItems)
            {
                Texture2D texture = this.Content.Load<Texture2D>(DrawItem.Texture);
                spriteBatch.Draw(texture, new Rectangle((int)DrawItem.Coords.x, (int)DrawItem.Coords.y, (int)(DrawItem.Width * TileSize), (int)(DrawItem.Height * TileSize)), DrawItem.Color);
            }

            spriteBatch.End();
        }

        // ====================================================================================================== V

        // Returns everything that is in the view
        public List<IViewable> GetOnScreen(List<IViewable> totalList, Viewport viewport, Matrix inverseMatrix)
        {
            List<IViewable> drawList = new List<IViewable>();
            
            Vector2 TopLeft = Vector2.Transform(new Vector2(viewport.X, viewport.Y), inverseMatrix);
            
            Vector2 BottomRight = Vector2.Transform(new Vector2(viewport.X + viewport.Width, viewport.Y + viewport.Height), inverseMatrix);

            foreach (var item in totalList)
            {
                if ( item.Coords.x < (TopLeft.X / TileSize) - item.Width || item.Coords.y < (TopLeft.Y / TileSize) - item.Height || item.Coords.x > BottomRight.X / TileSize || item.Coords.y > BottomRight.Y / TileSize ) continue;
                drawList.Add(item);
            }

            return drawList;
        }

        // Gets the cells that are in the view 
        public void GetCellsOnScreen()
        {
            Vector2 CameraPosition = new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);
            Vector2 realCameraPosition = Vector2.Transform(CameraPosition, Camera.GetInverseViewMatrix());

            Vector2 CameraBottomPosition = new Vector2(GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height);
            Vector2 RealCameraPos2 = Vector2.Transform(CameraBottomPosition, Camera.GetInverseViewMatrix());

            DrawList.Clear();

            foreach (var Chunk in gameModel.World.WorldModel.ChunkGrid)
            {
                foreach (var item2 in Chunk.Value.WorldChunkModel.grid)
                {
                    if (item2.worldCellView.Coords.x < (realCameraPosition.X / TileSize) - 1
                        || item2.worldCellView.Coords.y < (realCameraPosition.Y / TileSize) - 1
                        || item2.worldCellView.Coords.x > RealCameraPos2.X / TileSize
                        || item2.worldCellView.Coords.y > RealCameraPos2.Y / TileSize
                        ) continue;
                    DrawList.Add(item2.worldCellView);
                }
            }
        }

        // Gets the chunks that are in the view 
        public void GetChunksOnScreen()
        {
            Vector2 CameraPosition = new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);
            Vector2 realCameraPosition = Vector2.Transform(CameraPosition, Camera.GetInverseViewMatrix());

            Vector2 CameraBottomPosition = new Vector2(GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height);
            Vector2 RealCameraPos2 = Vector2.Transform(CameraBottomPosition, Camera.GetInverseViewMatrix());

            DrawList.Clear();

            foreach (var Chunk in gameModel.World.WorldModel.ChunkGrid)
            {
                if (Chunk.Key.x < (realCameraPosition.X / TileSize / WorldChunkModel.ChunkSize) - 1
                        || Chunk.Key.y < (realCameraPosition.Y / TileSize / WorldChunkModel.ChunkSize) - 1
                        || Chunk.Key.x > RealCameraPos2.X / TileSize / WorldChunkModel.ChunkSize
                        || Chunk.Key.y > RealCameraPos2.Y / TileSize / WorldChunkModel.ChunkSize
                        ) continue;

                foreach (var item2 in Chunk.Value.WorldChunkModel.grid)
                {
                    DrawList.Add(item2.worldCellView);
                }
            }
        }

    }
}
