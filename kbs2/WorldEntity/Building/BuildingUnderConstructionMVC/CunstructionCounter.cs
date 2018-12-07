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
        public FloatCoords Coords { get { return (FloatCoords)BUCController.BUCModel.TopLeft; } set => throw new NotImplementedException(); }
        public string SpriteFont { get { return "BuildingTimer"; } set => throw new NotImplementedException(); }
        public string Text { get; set; }
        public Color Color { get { return Color.Black; } set => throw new NotImplementedException(); }
        public int ZIndex { get { return 1; } set => throw new NotImplementedException(); }
    }
}
