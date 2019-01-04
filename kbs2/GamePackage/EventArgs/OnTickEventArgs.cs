using Microsoft.Xna.Framework;

namespace kbs2.GamePackage.EventArgs
{
    public class OnTickEventArgs : System.EventArgs
    {
        public GameTime GameTime { get; }
        public int Day;

        public OnTickEventArgs(GameTime gameTime)
        {
            GameTime = gameTime;
        }
    }
}