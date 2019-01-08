using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.Actions
{
    public class MapActionAnimationText : IViewText
    {
        public MapActionAnimationText(FloatCoords coords, string text, string spriteFont, double rotation)
        {
            Rotation = rotation;
            Text = text;
            Coords = coords;
            SpriteFont = spriteFont;
        }

        public double Rotation { get; }
        public FloatCoords Coords { get; }
        public int ZIndex => 2;
        public Color Colour { get; set; }
        public ViewMode ViewMode => ViewMode.Full;
        public string SpriteFont { get; }
        public string Text { get; }
    }
}