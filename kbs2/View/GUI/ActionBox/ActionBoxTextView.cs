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
    public class ActionBoxTextView : IText
    {
        public FloatCoords Coords { get; set; }
        public string SpriteFont { get; set; }
        public string Text { get; set; }
        public Color Color { get; set; }
        public int ZIndex { get; set; }

        public ActionBoxTextView(FloatCoords loc)
        {
            Coords = loc;
            SpriteFont = "BuildingTimer";
            Text = "Raichu";
            Color = Color.LightGreen;
            ZIndex = 501;
        }

    }
}
