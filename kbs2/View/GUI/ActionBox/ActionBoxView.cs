using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.View.GUI.ActionBox
{
    public class ActionBoxView : IViewImage
    {
        public FloatCoords Coords { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string Texture { get; set; }
        public Color Colour { get; set; }
        public int ZIndex { get; set; }

        public ActionBoxView(FloatCoords loc)
        {
            Coords = loc;
            Width = 2;
            Height = 1;
            Texture = "unittrainbalkje";
            Colour = Color.White;
            ZIndex = 500;
        }
    }
}
