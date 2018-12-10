using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage.Selection
{
    public class Selection_View : IViewable
    {
        public FloatCoords Coords { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string Texture { get; set; }
        public Color Color { get; set; }
        public int ZIndex { get; set; }

        public Selection_View()
        {
            Coords = new FloatCoords() { x = -1, y = -1};
            Width = 0;
            Height = 0;
            Texture = "PurpleLine";
            Color = Color.White;
            ZIndex = 1000;
        }
    }
}
