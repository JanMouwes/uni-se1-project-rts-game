using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace kbs2.GamePackage.Selection
{
    public class Selection_View : IViewImage
    {
        public FloatCoords Coords { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string Texture { get; set; }
        public Color Colour { get; set; }
        public int ZIndex { get; set; }

        public ViewMode ViewMode => ViewMode.Full;

        //constructor
        public Selection_View()
        {
            Coords = new FloatCoords() { x = -1, y = -1};
            Width = 0;
            Height = 0;
            Texture = "PurpleLine";
            Colour = Color.White;
            ZIndex = 1000;
        }
    }
}
