using System;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.WorldEntity.Structures.BuildingUnderConstructionMVC
{
    public class ConstructionCounter : IViewText
    {
        public ConstructingBuildingController ConstructingBuildingController { get; set; }

        public FloatCoords Coords
        {
            get => new FloatCoords()
            {
                x = ConstructingBuildingController.ConstructingBuildingModel.StartCoords.x
                    + ConstructingBuildingController.ConstructingBuildingView.Width / 2f
                    - Text.Length / 2f * 0.35f,
                y = ConstructingBuildingController.ConstructingBuildingModel.StartCoords.y
                    + (ConstructingBuildingController.ConstructingBuildingView.Height / 2f)
                    - 0.4f
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
            get => 1;
            set => throw new NotImplementedException();
        }

        public ViewMode ViewMode => ConstructingBuildingController.ConstructingBuildingView.ViewMode;
    }
}