using System.Collections.Generic;
using kbs2.View.GUI;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace kbs2.UserInterface
{
	class LeftButtonBar : IGuiViewImage
	{
		public GraphicsDevice GraphicsDevice { get; set; }
		public Coords coords => new Coords
		{
			x = 0,
			y = 0
		};


		public double Rotation { get; }
		public FloatCoords Coords { get { return (FloatCoords)coords; } set {; } }
		public float Height { get { return (int)(GraphicsDevice.Viewport.Width *.03); } set {; } }
		public float Width { get { return (int)(GraphicsDevice.Viewport.Width * .12); } set {; } }
		public string Texture { get { return "topbarleft"; } set {; } }

		public Color Colour { get { return Color.White; } set {; } }
		public int ZIndex { get { return 1; } set {; } }

        public ViewMode ViewMode => ViewMode.Full;
		
		public void Click(MouseState mouseState)
		{
			
		}

		public List<IGuiViewImage> GetContents()=> new List<IGuiViewImage>();

		public LeftButtonBar(GraphicsDevice graphicsDevice)
		{
			this.GraphicsDevice = graphicsDevice;
		}
	}
}
