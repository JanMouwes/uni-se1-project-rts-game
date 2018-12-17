using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.GamePackage;
using kbs2.GamePackage.Interfaces;
using kbs2.UserInterface.BottomBar;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.UserInterface
{
	public class BottomBarView : IViewImage
	{
		public GameController gameController { get; set; }
        public BottomBarModel Model { get; set; }

		public Coords coords => new Coords
		{
			x = (int) (gameController.GraphicsDevice.Viewport.Width * .15),
			y = (int) (gameController.GraphicsDevice.Viewport.Height * .81)
		};

		public FloatCoords Coords { get { return (FloatCoords)coords; } set {; } }
		public float Height { get { return (int)(gameController.GraphicsDevice.Viewport.Height * .19); } set {; } }
		public float Width { get { return (int)(gameController.GraphicsDevice.Viewport.Width * .70); } set {; } }
		public string Texture { get { return "bottombarmid"; } set {; } }
		public Color Colour { get { return Color.White; } set {; } }
		public int ZIndex { get { return 1; } set {; } }


		public BottomBarView(GameController gameController)
		{
			this.gameController = gameController;
            Model = new BottomBarModel(this);
		}
	}
}
