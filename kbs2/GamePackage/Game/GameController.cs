using System.Timers;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.GamePackage.EventArgs;

namespace kbs2.GamePackage.Game
{
    public delegate void GameSpeedObserver(object sender, GameSpeedEventArgs eventArgs);

    public delegate void GameStateObserver(object sender, GameStateEventArgs eventArgs);

    public class GameController
    {
        public const int TicksPerSecond = 30;

        public static int TickInterval => 1 / TicksPerSecond;

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

        //    GameSpeed and its event
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

            GameTimer = new Timer(TickInterval);
        }
    }
}