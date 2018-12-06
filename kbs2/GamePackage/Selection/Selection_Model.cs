using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage.Selection
{
    public class Selection_Model
    {
        public MouseState PreviousMouseState { get; set; }
        public RectangleF SelectionBox { get; set; }
        public RectangleF Top { get; set; }
        public RectangleF Left { get; set; }
        public RectangleF Right { get; set; }
        public RectangleF Bottom { get; set; }
        public List<Selection_View> ViewList { get; set; }

        public Selection_Model()
        {
            PreviousMouseState = new MouseState();

            SelectionBox = new RectangleF(-1, -1, 0, 0);

            ViewList = new List<Selection_View>();

            Top = new RectangleF(-1, -1, 0, 0);
            Left = new RectangleF(-1, -1, 0, 0);
            Right = new RectangleF(-1, -1, 0, 0);
            Bottom = new RectangleF(-1, -1, 0, 0);
        }
    }
}
