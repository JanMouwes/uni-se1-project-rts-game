using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.World.Cell
{
    public class WorldCellView : IViewImage
    {
        public double Rotation { get; }
        public FloatCoords Coords => (FloatCoords) model.RealCoords;
        public string Texture { get; set; }
        public float Width { get; set; } = 1;
        public float Height { get; set; } = 1;
        public Color Colour { get; set; } = Color.White;
        public int ZIndex { get; set; } = 1;

        private WorldCellModel model;

        public ViewMode ViewMode => model.ViewMode;

        public WorldCellView(WorldCellModel model, string texture)
        {
            this.model = model;
            Texture = texture;
        }

        public WorldCellView(WorldCellModel model, string texture, float width, float height, Color color, int zIndex) : this(model, texture)
        {
            Width = width;
            Height = height;
            Colour = color;
            ZIndex = zIndex;
        }
    }
}