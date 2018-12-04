using System;
using kbs2.Desktop.View.MapView;
using System.Data.SQLite;
using kbs2.GamePackage;

namespace kbs2.Desktop
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GameController Controller = new GameController(GameSpeed.Regular, GameState.Running);
            using (Controller.gameView)
                Controller.gameView.Run();
        }
    }
}
