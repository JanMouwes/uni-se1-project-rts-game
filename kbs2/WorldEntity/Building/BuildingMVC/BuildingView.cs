using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;

namespace kbs2.WorldEntity.Building.BuildingMVC
{
    public class BuildingView : IViewImage
    {
        public BuildingModel BuildingModel { get; set; }

        //    FIXME move to Model.
        public float Height { get; }
        public float Width { get; }

        public FloatCoords Coords => (FloatCoords) BuildingModel.TopLeft;

        public string Texture { get; }
        public Color Colour => Color.White;
        public int ZIndex => 2;

        public ViewMode ViewMode { get; set; }

        // sets height, width and image for a buildingview
        public BuildingView(string imageSrc, float height, float width)
        {
            Height = height;
            Width = width;
            Texture = imageSrc;
            ViewMode = ViewMode.Fog;
        }
    }
}