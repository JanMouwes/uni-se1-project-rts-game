using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kbs2.UserInterface
{
	class BottomBarView : IViewImage
	{
		public GraphicsDevice GraphicsDevice { get; set; }
		public Coords coords => new Coords
		{
			x = (int) (GraphicsDevice.Viewport.Width * .15),
			y = (int) (GraphicsDevice.Viewport.Height * .81)
		};

		public FloatCoords Coords { get { return (FloatCoords)coords; } set {; } }
		public float Height { get { return (int)(GraphicsDevice.Viewport.Height * .19); } set {; } }
		public float Width { get { return (int)(GraphicsDevice.Viewport.Width * .70); } set {; } }
		public string Texture { get { return "bottombarmid"; } set {; } }
		public Color Colour { get { return Color.White; } set {; } }
		public int ZIndex { get { return 1; } set {; } }

        public ViewMode ViewMode => ViewMode.Full;

        public BottomBarView(GraphicsDevice GraphicsDevice)
		{
			this.GraphicsDevice = GraphicsDevice;
		}
	}
}
