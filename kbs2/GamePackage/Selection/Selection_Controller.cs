using kbs2.GamePackage.Selection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage
{
    public class Selection_Controller
    {
        public Selection_Model Model { get; set; }
        public Selection_View View { get; set; }

        public Selection_Controller(string lineTexture, MouseState mouseState)
        {
            Model = new Selection_Model(mouseState);
            View = new Selection_View(lineTexture);
        }

        public void DrawSelectionBox(MouseState CurMouseState)
        {
            if(CurMouseState.LeftButton == ButtonState.Pressed && Model.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                View.Selection = new Rectangle(CurMouseState.X, CurMouseState.Y, 0, 0);
            }

            if(CurMouseState.LeftButton == ButtonState.Pressed)
            {
                View.Selection = new Rectangle(View.Selection.X, View.Selection.Y, CurMouseState.X - View.Selection.X, CurMouseState.Y - View.Selection.Y);
            }

            if(CurMouseState.LeftButton == ButtonState.Released)
            {
                View.Selection = new Rectangle(-1, -1, 0, 0);
            }

            Model.PreviousMouseState = CurMouseState;
        }
    }
}
