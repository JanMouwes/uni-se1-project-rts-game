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
        public List<Selection_View> Box { get; set; }

        public Selection_Model()
        {
            PreviousMouseState = new MouseState();
            SelectionBox = new RectangleF(-1, -1, 0, 0);
            Box = new List<Selection_View>();
        }
    }
}
