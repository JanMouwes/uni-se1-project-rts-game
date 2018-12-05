using System;
using System.Timers;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Desktop.World.World;
using kbs2.GamePackage.EventArgs;
using kbs2.World.World;
using Microsoft.Xna.Framework;

namespace kbs2.GamePackage
{
    public delegate void GameSpeedObserver(object sender, GameSpeedEventArgs eventArgs);

    public delegate void GameStateObserver(object sender, GameStateEventArgs eventArgs);

    public class GameController
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

        public GameController(GameSpeed gameSpeed, GameState gameState)
        {
            this.GameSpeed = gameSpeed;
            this.GameState = gameState;

            gameView = new GameView(gameModel);

            gameModel.World = WorldFactory.GetNewWorld();
            //gameModel.Factions = FactionFactory.GetNewFaction();

            gameModel.Selection = new Selection_Controller("PurpleLine");

            GameTimer = new Timer(TickIntervalMilliseconds);
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