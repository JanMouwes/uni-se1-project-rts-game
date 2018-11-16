using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.GamePackage;

namespace kbs2.Desktop.GamePackage.Game
{
    public delegate void GameStateObserver(object sender, GameStateEventArgs eventArgs);

    public class GameController
    {
        //private GameTimer Timer;

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