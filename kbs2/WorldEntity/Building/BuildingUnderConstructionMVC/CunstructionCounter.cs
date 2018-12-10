using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Building.BuildingUnderConstructionMVC
{
    public class CunstructionCounter : IText
    {
        public BUCController BUCController { get; set; }
        public FloatCoords Coords {
            get
            { return new FloatCoords()
                {
                    x = (BUCController.BUCModel.TopLeft.x + (BUCController.BUCView.Width / 2f)-((Text.Length /2f)*0.35f)),
                    y = (BUCController.BUCModel.TopLeft.y + (BUCController.BUCView.Height / 2f)-0.4f)

                };
            }
            set => throw new NotImplementedException(); }
        public string SpriteFont { get { return "BuildingTimer"; } set => throw new NotImplementedException(); }
        public string Text { get; set; }
        public Color Color { get { return Color.Blue; } set => throw new NotImplementedException(); }
        public int ZIndex { get { return 1; } set => throw new NotImplementedException(); }
    }
}
