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

		public float Width { get => UnitModel.Width; set => Width = UnitModel.Width; }
		public float Height { get => UnitModel.Height; set => Height = UnitModel.Height; }
		public string Texture { get => UnitModel.Texture; set => Texture = UnitModel.Texture; }
		public Color Color { get => UnitModel.Color; set => Color = UnitModel.Color; }
		public int ZIndex { get => UnitModel.ZIndex; set => ZIndex = UnitModel.ZIndex; } 

		public Unit_View(string imageSrc)
        {
            ImageSrcPri = imageSrc;
            ImageSrcSec = "shadow";
        }
	}
}
