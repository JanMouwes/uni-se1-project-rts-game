using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage.SelectionBoxMVC
{
    public class SelectionBoxModel
    {
        public MouseState PreviousMouseState { get; set; }
        public FloatCoords InitXYCoord { get; set; }
        public RectangleF SelectionBox { get; set; }

        public SelectionBoxModel()
        {
            PreviousMouseState = new MouseState();
            InitXYCoord = new FloatCoords { x = -1, y = -1 };
            SelectionBox = new RectangleF(-1, -1, 0, 0);
        }
    }
}
