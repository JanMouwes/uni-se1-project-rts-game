using kbs2.GamePackage;
using Microsoft.Xna.Framework;
using System.Timers;

namespace kbs2.Desktop.GamePackage.EventArgs
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