using System;
using System.Collections.Generic;
using kbs2.Desktop.View.Camera;
using kbs2.Desktop.View.EventArgs;
using kbs2.Desktop.World.World;
using kbs2.GamePackage;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.Structs;
using kbs2.World.TerrainDef;
using kbs2.World.World;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace kbs2.Desktop.View.MapView
{
    public delegate void ZoomObserver(object sender, ZoomEventArgs eventArgs);

    public class MapView : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private CameraController Camera;
        private WorldController World;
        private Selection_Controller Selection;
        private List<Unit_Controller> UnitList;

        // Calculate the size (Width) of a tile
        public int TileSize => (int) (GraphicsDevice.Viewport.Width / Camera.CameraModel.TileCount);

        // Calculates the height of a cell
        public int CellHeight => (int) (Camera.CameraModel.TileCount / GraphicsDevice.Viewport.AspectRatio);

        // Constructor
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
            // Add initialization logic here
            UnitList = new List<Unit_Controller>();

            Unit_Controller Pichu = new Unit_Controller("pichu_idle", 0.3f, 0.3f, 1f, 0.2f);
            Unit_Controller Pikachu = new Unit_Controller("pikachu_idle", 0.6f, 0.6f, 2f, 0.4f);
            Unit_Controller Raichu = new Unit_Controller("raichu_idle", 0.8f, 0.8f, 4f, 0.4f);
            Unit_Controller Rayquaza = new Unit_Controller("rayquaza_idle", 3.5f, 3.5f, 8f, 0.4f);

            UnitList.Add(Pichu);
            UnitList.Add(Pikachu);
            UnitList.Add(Raichu);
            UnitList.Add(Rayquaza);

            // initialize world
            World = WorldFactory.GetNewWorld();

            // initialize camera
            Camera = new CameraController(GraphicsDevice);

            // initialize selection
            Selection = new Selection_Controller("PurpleLine", Mouse.GetState());

            // Sets the defenition of terraintextures
            TerrainDef.TerrainDictionairy.Add(TerrainType.Sand, "Sand");

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
            // Exit game if escape is pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Updates camera according to the pressed buttons
            Camera.MoveCamera();

            //check for if a unit was clicked or not
            MouseState dipshit = Mouse.GetState();

            float x = Camera.GetViewMatrix().M41;
            float y = Camera.GetViewMatrix().M42;

            //Selection.CheckClicked(UnitList, Mouse.GetState(), Camera.GetViewMatrix(), TileSize);
            

            // Draws a selection box according to the selected area
            Selection.DrawSelectionBox(UnitList, Mouse.GetState(), Camera.GetViewMatrix(), TileSize, Camera.CameraModel.Zoom);

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

            // Draws cells in that in the chunks that are in the camera's view
            DrawCells();

            // Draws the units on screen
            DrawUnits();

            // Draws the selection box when you select and drag
            DrawSelection();

            // Calls the game's draw function
            base.Draw(gameTime);
        }

        // Delegate that decides what tilecolor function should be called
        public delegate Color TileColourDelegate(WorldCellModel cell);

        // Holds the function that is called when the delegate is called
        public TileColourDelegate TileColour;

        private Color NotCheckered => Color.White;
        
        // Draws the chunks in a Checkered pattern for easy debugging
        private Color ChunkCheckered(WorldCellModel cell) =>
            Math.Abs(cell.ParentChunk.ChunkCoords.x) % 2 ==
            (Math.Abs(cell.ParentChunk.ChunkCoords.y) % 2 == 1 ? 1 : 0)
                ? Color.Gray
                : Color.Yellow;

        // Draws the cells in a Checkered pattern for easy debugging
        private Color CellCheckered(WorldCellModel cell) =>
            Math.Abs(cell.RealCoords.x) % 2 == ((Math.Abs(cell.RealCoords.y) % 2 == 1) ? 1 : 0)
                ? Color.Gray
                : Color.Yellow;

        // Draws the chunks and cells in a Checkered pattern for easy debugging
        private Color CellChunkCheckered(WorldCellModel cell) =>
            Math.Abs(cell.ParentChunk.ChunkCoords.x) % 2 ==
            (Math.Abs(cell.ParentChunk.ChunkCoords.y) % 2 == 1 ? 1 : 0)
                ? Math.Abs(cell.RealCoords.x) % 2 == ((Math.Abs(cell.RealCoords.y) % 2 == 1) ? 1 : 0)
                    ? Color.Gray
                    : Color.Yellow
                : Math.Abs(cell.RealCoords.x) % 2 == ((Math.Abs(cell.RealCoords.y) % 2 == 1) ? 1 : 0)
                    ? Color.Green
                    : Color.Red;

        // Draws a random pattern on the cells
        private Color RandomColour(WorldCellModel cell) =>
            new Random(cell.RealCoords.y * cell.RealCoords.x).Next(0, 2) == 1 ? Color.Gray : Color.Pink;

        //    Draw-position relative to x or y coordinate provided
        private int CellDrawInt(double realCoord) => (int) Math.Round(realCoord * TileSize);

        //    Draw-position relative to x or y coordinate provided
        private int CellDrawPosition(double realCoord) => (int) Math.Round(realCoord * TileSize);

        //    Draw-position relative to x or y coordinate provided
        private Coords CellDrawCoords(FloatCoords floatCoords) => CellDrawCoords(floatCoords.x, floatCoords.y);

        //    Draw-position relative to x or y coordinate provided
        private Coords CellDrawCoords(float x, float y) => new Coords
            {x = (int) Math.Round(x * TileSize), y = (int) Math.Round(y * TileSize)};

        // Draws the cells on the screen according to the given chunks in the camera view
        private void DrawCells()
        {
            // Start spritebatch for drawing
            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            // draw each tile in the chunks in the chunkGrid
            foreach (KeyValuePair<Coords, WorldChunkController> chunkGrid in GetChunksOnScreen())
            {
                foreach (WorldCellModel cell in chunkGrid.Value.WorldChunkModel.grid)
                {
                    // Sets the x and y for the current tile
                    int y = CellDrawPosition(cell.RealCoords.y);
                    int x = CellDrawPosition(cell.RealCoords.x);

                    // Gets the texture according to the terrain type of the cell
                    Texture2D texture = this.Content.Load<Texture2D>(TerrainDef.TerrainDictionairy[cell.Terrain]);

                    // Defines the Color of the cell (for debugging)
                    Color colour = TileColour != null ? TileColour(cell) : CellChunkCheckered(cell);

                    // Draws the texture of the cell on the location coords with the size of the tile and the color
                    spriteBatch.Draw(texture, new Rectangle(x, y, TileSize, TileSize), colour);
                }
            }

            // Close cells draw batch
            spriteBatch.End();
        }

        // Calculates wich chunks are in the camera's view and returns them in a list
        public Dictionary<Coords, WorldChunkController> GetChunksOnScreen()
        {
            // This function is for testing and is still in progress
            Dictionary<Coords, WorldChunkController> chunksOnScreen = new Dictionary<Coords, WorldChunkController>();
            chunksOnScreen = World.WorldModel.ChunkGrid;
            float x = Camera.GetViewMatrix().M41;
            float y = Camera.GetViewMatrix().M42;
            //Console.WriteLine($"X: {x}");
            //Console.WriteLine($"Y: {y}");
            return chunksOnScreen;
        }

        // Draws the selection box arround the selected area
        public void DrawSelection()
        {
            // Begin drawing without an offset
            spriteBatch.Begin();

            DrawHorizontalLine((int) Selection.View.SelectionBox.Y);
            DrawHorizontalLine((int) (Selection.View.SelectionBox.Y + Selection.View.SelectionBox.Height));
            DrawVerticalLine((int) (Selection.View.SelectionBox.X));
            DrawVerticalLine((int) (Selection.View.SelectionBox.X + Selection.View.SelectionBox.Width));

            // End drawing of the selection box
            spriteBatch.End();
        }

        // Todo: Add comments from here on down
        public void DrawHorizontalLine(int PositionY)
        {
            Texture2D texture = Content.Load<Texture2D>(Selection.View.LineTexture);
            if (Selection.View.SelectionBox.Width > 0)
            {
                for (int i = 0; i <= Selection.View.SelectionBox.Width - 10; i += 10)
                {
                    if (Selection.View.SelectionBox.Width - i >= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle((int) (Selection.View.SelectionBox.X + i), PositionY, 10, 5),
                            Color.White);
                    }
                }
            }
            else if (Selection.View.SelectionBox.Width < 0)
            {
                for (int i = -10; i >= Selection.View.SelectionBox.Width; i -= 10)
                {
                    if (Selection.View.SelectionBox.Width - i <= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle((int) (Selection.View.SelectionBox.X + i), PositionY, 10, 5),
                            Color.White);
                    }
                }
            }
        }

        public void DrawVerticalLine(int PositionX)
        {
            Texture2D texture = Content.Load<Texture2D>(Selection.View.LineTexture);
            if (Selection.View.SelectionBox.Height > 0)
            {
                for (int i = -2; i <= Selection.View.SelectionBox.Height; i += 10)
                {
                    if (Selection.View.SelectionBox.Height - i >= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle(PositionX, (int) (Selection.View.SelectionBox.Y + i), 10, 5),
                            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, MathHelper.ToRadians(90),
                            new Vector2(0, 0), SpriteEffects.None, 0);
                    }
                }
            }

            else if (Selection.View.SelectionBox.Height < 0)
            {
                for (int i = 0; i >= Selection.View.SelectionBox.Height; i -= 10)
                {
                    if (Selection.View.SelectionBox.Height - i <= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle(PositionX - 10, (int) (Selection.View.SelectionBox.Y + i), 10, 5),
                            Color.White);
                    }
                }
            }
        }

        public void DrawUnits()
        {
            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());
            Coords drawPos = CellDrawCoords(0.05f, 0.5f);

            foreach(Unit_Controller unit in UnitList)
            {
                drawPos = CellDrawCoords(unit.UnitModel.LocationModel.floatCoords);
                
                spriteBatch.Draw(Content.Load<Texture2D>(unit.UnitView.ImageSrcSec),
                   new Rectangle(
                       (int)(drawPos.x - TileSize * unit.UnitModel.Width * .5),
                       (int)(drawPos.y - TileSize * unit.UnitModel.Height * -.01),
                       (int)(TileSize * unit.UnitModel.Width), (int)(TileSize * (unit.UnitModel.Height * 0.5))), Color.White
                );
                spriteBatch.Draw(Content.Load<Texture2D>(unit.UnitView.ImageSrcPri),
                    new Rectangle(
                        (int)(drawPos.x - TileSize * unit.UnitModel.Width * .5),
                        (int)(drawPos.y - TileSize * unit.UnitModel.Height * .5),
                        (int)(TileSize * unit.UnitModel.Width), (int)(TileSize * unit.UnitModel.Height)), Color.White
                );
            }

            spriteBatch.End();
        }
    }
}