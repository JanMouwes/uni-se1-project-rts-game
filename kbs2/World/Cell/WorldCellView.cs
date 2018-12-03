using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.World.Cell
{
    public class WorldCellView : IViewable
    {
        public FloatCoords Coords { get; set; }
        public string Texture { get; set; }
        public float Width { get; set; } = 1;
        public float Height { get; set; } = 1;
        public Color Color { get; set; } = Color.White;
        public int ZIndex { get; set; } = 1;

        public WorldCellView(FloatCoords coords, string texture)
        {
            Coords = coords;
            Texture = texture;
        }

        public WorldCellView(FloatCoords coords, string texture, float width, float height, Color color, int zIndex) : this(coords, texture)
        {
            Width = width;
            Height = height;
            Color = color;
            ZIndex = zIndex;
        }
    }
}
