using kbs2.GamePackage;

namespace kbs2.Desktop.GamePackage.EventArgs
{
    public class GameStateEventArgs : System.EventArgs
    {
        public GameState GameState { get; }

        public GameStateEventArgs(GameState gameState)
        {
            GameState = gameState;
        }
    }
}