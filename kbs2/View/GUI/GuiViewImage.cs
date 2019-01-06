using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.View.GUI
{
    public abstract class GuiViewImage : IViewImage
    {
        public int ZIndex => 2;
        public Color Colour => Color.White;
        public ViewMode ViewMode => ViewMode.Full;

        protected GuiViewImage(FloatCoords location, ViewValues viewValues)
        {
            Coords = location;
            Width = viewValues.Width;
            Height = viewValues.Height;
            Texture = viewValues.Image;
        }

        public virtual FloatCoords Coords { get; protected set; }
        public virtual float Width { get; protected set; }
        public virtual float Height { get; protected set; }
        public virtual string Texture { get; protected set; }
    }
}