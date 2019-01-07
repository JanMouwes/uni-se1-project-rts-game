namespace kbs2.GamePackage.EventArgs
{
    public class GameSpeedEventArgs : System.EventArgs
    {
        public GameSpeed GameSpeed { get; }

        public GameSpeedEventArgs(GameSpeed gameSpeed)
        {
            GameSpeed = gameSpeed;
        }
    }
}