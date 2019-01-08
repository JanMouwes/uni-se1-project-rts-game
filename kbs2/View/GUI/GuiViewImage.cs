using System.Collections.Generic;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.View.GUI
{
    public interface IGuiViewImage : IViewImage
    {
        void Click();

        List<IGuiViewImage> GetContents();
    }

    public abstract class GuiViewImage : IGuiViewImage
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

        public double Rotation { get; }
        public virtual FloatCoords Coords { get; protected set; }
        public virtual float Width { get; protected set; }
        public virtual float Height { get; protected set; }
        public virtual string Texture { get; protected set; }


        public abstract void Click();

        public virtual List<IGuiViewImage> GetContents() => new List<IGuiViewImage>();
    }
}