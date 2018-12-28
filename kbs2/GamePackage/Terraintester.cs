using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage
{
    public class Terraintester : IViewText
    {
        public FloatCoords Coords
        {
            get => new FloatCoords()
            {
                x = 0,
                y = 100
            };
            set => throw new NotImplementedException();
        }

        public string SpriteFont
        {
            get => "BuildingTimer";
            set => throw new NotImplementedException();
        }

        public string Text { get; set; }

        public Color Colour
        {
            get => Color.Blue;
            set => throw new NotImplementedException();
        }

        public int ZIndex
        {
            get => 2;
            set => throw new NotImplementedException();
        }
    }
}
