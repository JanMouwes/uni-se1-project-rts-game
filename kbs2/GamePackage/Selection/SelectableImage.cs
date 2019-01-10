using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using Microsoft.Xna.Framework;

namespace kbs2.GamePackage.Selection
{
    public class SelectableImage : Unit_Controller
    {
        public double Rotation { get; }
        public FloatCoords Coords { get; }
        public int ZIndex => 2;
        public Color Colour => Color.White;
        public ViewMode ViewMode => ViewMode.Full;
        public float Width { get; }
        public float Height { get; }
        public string Texture => "selection-circle";

        public SelectableImage(IWorldEntity targetable)
        {
            Width = targetable.View.Width;
            Height = targetable.View.Height;
            
            Coords = targetable.Centre - new FloatCoords()
            {
                x = Width * .5f,
                y = Height * .5f
            };
        }
    }
}