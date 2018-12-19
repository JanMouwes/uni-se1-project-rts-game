using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;
namespace kbs2.WorldEntity.Building.BuildingMVC
{
    public class BuildingView : IViewImage
    {
        public BuildingModel BuildingModel { get; set; }
        public string ImageSrc { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public FloatCoords Coords { get { return (FloatCoords)BuildingModel.TopLeft; } set {; } }
        public string Texture { get { return ImageSrc; } set {; } }
        public Color Colour { get { return Color.White; } set {; } }
        public int ZIndex { get { return 2; } set {; } }

        // sets height, width and image for a buildingview
        public BuildingView(string imageSrc, float height, float width)
        {
            Height = height;
            Width = width;
            ImageSrc = imageSrc;
        }
    }
}
