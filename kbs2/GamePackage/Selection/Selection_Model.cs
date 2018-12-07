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
        public RectangleF SelectionBox { get; set; }
        public MouseState PreviousMouseState { get; set; }

        public Selection_Model()
        {
            SelectionBox = new RectangleF();
            PreviousMouseState = new MouseState();
        }
    }
}
