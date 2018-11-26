﻿using System;
using kbs2.Desktop.View.MapView;

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
            using (var game = new MapView())
                game.Run();
        }
    }
}
