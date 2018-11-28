using kbs2.Unit.Model;
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
	public class Unit_View
	{
        public string ImageSrc { get; set; }
        

        public Unit_View(string imageSrc)
        {
            ImageSrc = imageSrc;
        }

        public string Draw()
        {
            return ImageSrc;   
        }
	}
}
