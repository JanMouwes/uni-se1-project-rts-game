using kbs2.GamePackage.Interfaces;
using kbs2.Unit.Model;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace kbs2.WorldEntity.Unit.MVC
{
	public class Unit_View : IViewImage
	{
        public string ImageSrcShad { get; set; }

		public FloatCoords Coords { get; set; }

		public float Width { get; set; }
		public float Height { get; set; }

		public string Texture { get; set; }
		public Color Colour { get { return Color.White; } set {; } }
		public int ZIndex { get { return 2; } set {; } }

		public Unit_View(FloatCoords coords)
        {
            ImageSrcShad = "shadow";
            Coords = coords;
        }
	}
}
