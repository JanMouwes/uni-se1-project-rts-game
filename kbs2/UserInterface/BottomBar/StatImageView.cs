using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.UserInterface.BottomBar
{
    public class StatImageView : IViewImage
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public string Texture { get; set; }
        public FloatCoords Coords { get; set; }
        public int ZIndex { get; set; }
        public Color Colour { get; set; }

        public StatImageView(FloatCoords coords, IViewImage entity)
        {
            Coords = coords;
            Width = 30;
            Height = 30;
            Texture = entity.Texture;
            ZIndex = 1001;
            Colour = Color.White;
        }
    }
}
