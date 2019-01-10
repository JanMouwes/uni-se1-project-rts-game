using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.Actions
{
    public class MapActionAnimationItem : Unit_Controller
    {
        public double Rotation { get; }
        public FloatCoords Coords { get; }
        public int ZIndex => 2;
        public Color Colour => Color.White;
        public ViewMode ViewMode => ViewMode.Full;
        public float Width { get; }
        public float Height { get; }
        public string Texture { get; }

        public MapActionAnimationItem(FloatCoords coords, float width, float height, string texture, double rotation)
        {
            Coords = coords;
            Width = width;
            Height = height;
            Texture = texture;
            Rotation = rotation;
        }
    }
}