using System;
using kbs2.Desktop.View.MapView;
using kbs2.GamePackage;
using kbs2.World.TerrainDef;
using kbs2.World;

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
            GameController game = new GameController(GameSpeed.Regular, GameState.Running);

            using (game)
                game.Run();
        }
    }
}