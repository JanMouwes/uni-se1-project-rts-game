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
	public class Unit_View : IViewable
	{
		public Unit_Model UnitModel { get; set; }

        public string ImageSrcPri { get; set; }
        public string ImageSrcSec { get; set; }

		public FloatCoords Coords { get; set; }
		
		public float Width { get; set; }
		public float Height { get; set; }
		public string Texture { get; set; }
		public Color Color { get; set; } = Color.White;
		public int ZIndex { get; set; } = 2;

		public Unit_View(string imageSrc, float width, float height, string texture)
        {
            ImageSrcPri = imageSrc;
            ImageSrcSec = "shadow";
			Width = width;
			Height = height;
			Texture = texture;
        }
	}
}
