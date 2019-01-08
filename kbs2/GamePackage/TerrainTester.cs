using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;

namespace kbs2.GamePackage
{
    public class TerrainTester : IViewText
    {
        public double Rotation { get; }
        public FloatCoords Coords { get; set; }

        public string SpriteFont
        {
            get => "BuildingTimer";
            set => throw new NotImplementedException();
        }

        public string Text { get; set; }

        public Color Colour { get; set; } = Color.Blue;

        public int ZIndex
        {
            get => 2;
            set => throw new NotImplementedException();
        }

        public ViewMode ViewMode => ViewMode.Full;

        public TerrainTester(FloatCoords coords)
        {
            Coords = coords;
        }
    }
}