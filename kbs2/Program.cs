using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using kbs2.Desktop.View.MapView;
using kbs2.GamePackage;
using kbs2.GamePackage.GameSaveManager;
using kbs2.World.TerrainDef;
using kbs2.World;
using kbs2.WorldEntity.Building;

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
            
//            using (DataBaseContext dbContext = new DataBaseContext())
//            {
//                List<BuildingDef> defs = (from buildingDef in dbContext.BuildingDef select buildingDef).ToList();
//                foreach (BuildingDef buildingDef in defs)
//                {
//                    Console.WriteLine(buildingDef.BuildingShape);
//                }
//            }

            using (game)
                game.Run();
        }
    }
}