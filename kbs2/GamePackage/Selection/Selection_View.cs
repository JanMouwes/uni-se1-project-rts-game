using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage.Selection
{
    public class Selection_View
    {
        public string LineTexture { get; set; }
        public RectangleF SelectionBox { get; set; }

        public Selection_View(string lineText)
        {
            LineTexture = lineText;
            SelectionBox = new RectangleF(-1, -1, 0, 0);
        }
    }   
}
