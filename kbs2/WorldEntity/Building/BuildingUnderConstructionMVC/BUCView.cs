using System;
using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.WorldEntity.Building.BuildingUnderConstructionMVC
{
    public class BUCView : IViewable
    {
        public BUCModel model { get; set; }
        public string ImageSrc { get; set; }

        public FloatCoords Coords { get =>(FloatCoords)model.TopLeft; set => throw new NotImplementedException(); }
        public float Width { get => model.BuildingDef.width; set => throw new NotImplementedException(); }
        public float Height { get => model.BuildingDef.height; set => throw new NotImplementedException(); }
        public string Texture { get { return ImageSrc; } set {; } }
        public Color Color { get { return Color.White; } set {; } }
        public int ZIndex { get { return 2; } set {; } }

        public BUCView(string imageSrc, float height, float width)
        {
            Height = height;
            Width = width;
            ImageSrc = imageSrc;
        }

    }

}
