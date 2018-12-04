using System;
using kbs2.Desktop.View.MapView;
using System.Data.SQLite;
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
            // Fill the Dictionairy
            TerrainDef.TerrainDictionairy.Add(TerrainType.Grass, "grass");

            GameController Controller = new GameController(GameSpeed.Regular, GameState.Running);
            Controller.CellChunkCheckered();
            Controller.RandomPattern();

            using (Controller.gameView)
                Controller.gameView.Run();
        }
    }
}
