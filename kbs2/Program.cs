using System;
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
            GameController game = new GameController(null, GameSpeed.Regular, GameState.Running);

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