using kbs2.UserInterface.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.UserInterface.MenuBar
{
    public class MenuButton : IButton
    {
        public string Name { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string Texture { get; set; }
        public FloatCoords Coords { get; set; }
        public int ZIndex { get; set; }
        public Color Colour { get; set; }
    }
}
