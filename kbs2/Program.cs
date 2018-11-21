using System;
using System.Data.SQLite;

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
            DBConnection dB = new DBConnection();
            dB.OpenConnection("DefDex");

            using (var game = new Game1())
                game.Run();
        }
    }
}
