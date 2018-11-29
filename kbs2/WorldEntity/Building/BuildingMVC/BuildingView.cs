using System;
namespace kbs2.WorldEntity.Building.BuildingMVC
{
    public class BuildingView
    {
        public Building_Model BuildingModel { get; set; }
        public string ImageSrc { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }

        public BuildingView(string imageSrc, float height, float width)
        {
            Height = height;
            Width = width;
            ImageSrc = imageSrc;
        }

        public string Draw()
        {
            return ImageSrc;
        }
    }
}
