using System;
using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.WorldEntity.Building.BuildingUnderConstructionMVC
{
    public class ConstructingBuildingView : IViewImage
    {
        public ConstructingBuildingModel Model { get; set; }
        public string ImageSrc { get; }


        // Implementation IViewable
        public FloatCoords Coords => (FloatCoords) Model.StartCoords;

        public float Width => Model.BuildingDef.ViewValues.Width;

        public float Height => Model.BuildingDef.ViewValues.Height;

        public string Texture => ImageSrc;

        public Color Colour => Color.White;

        public int ZIndex => 2;


        // constructor
        public ConstructingBuildingView(string imageSrc)
        {
            ImageSrc = imageSrc;
        }
    }
}