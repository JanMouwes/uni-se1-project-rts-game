using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.WorldEntity.Structures.BuildingMVC
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
        public BuildingView(BuildingModel model)
        {
            BuildingModel = model;

            ViewValues viewValues = model.Def.ViewValues;
            
            Height = viewValues.Height;
            Width = viewValues.Width;
            Texture = viewValues.Image;
            ViewMode = ViewMode.Fog;
        }
    }
}