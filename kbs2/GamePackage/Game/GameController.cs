using System.Timers;
using kbs2.Desktop.GamePackage.EventArgs;

namespace kbs2.GamePackage.Game
{
    public delegate void GameStateObserver(object sender, GameStateEventArgs eventArgs);

    public class GameController
    {
        private Timer GameTimer; //TODO

        public GameSpeed Speed { get; set; }

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
    }
}