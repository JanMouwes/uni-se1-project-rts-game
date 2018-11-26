using kbs2.GamePackage.Selection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

        private void DrawHorizontalLine(int PositionY)
        {
            if (View.Selection.Width > 0)
            {
                for (int i = 0; i <= View.Selection.Width - 10; i += 10)
                {
                    if (View.Selection.Width - i >= 0)
                    {
                        //spriteBatch.Draw(Controller.View.LineTexture, new Rectangle(View.Selection.X + i, PositionY, 10, 5), Color.White);
                    }
                }
            }
            else if (View.Selection.Width < 0)
            {
                for (int i = -10; i >= View.Selection.Width; i -= 10)
                {
                    if (View.Selection.Width - i <= 0)
                    {
                        //spriteBatch.Draw(Controller.View.LineTexture, new Rectangle(View.Selection.X + i, PositionY, 10, 5), Color.White);
                    }
                }
            }
        }

        private void DrawVerticalLine(int PositionX)
        {
            if (View.Selection.Height > 0)
            {
                for (int i = -2; i <= View.Selection.Height; i += 10)
                {
                    if (View.Selection.Height - i >= 0)
                    {
                        //spriteBatch.Draw(Controller.View.LineTexture, new Rectangle(PositionX, View.Selection.Y + i, 10, 5),
                        //new Rectangle(0, 0, mDottedLine.Width, mDottedLine.Height), Color.White, MathHelper.ToRadians(90), new Vector2(0, 0), SpriteEffects.None, 0);
                    }
                }
            }
            else if (View.Selection.Height < 0)
            {
                for (int i = 0; i >= View.Selection.Height; i -= 10)
                {
                    if (View.Selection.Height - i <= 0)
                    {
                        //spriteBatch.Draw(Controller.View.LineTexture, new Rectangle(PositionX - 10, View.Selection.Y + i, 10, 5), Color.White);
                    }
                }
            }
        }

        /*
            DIT MOET IN DE NIEUWE GAME VIEW KOMEN!!!!!

            // Update()
            MouseState mouse = Mouse.GetState();
            CONTROLLER.DrawSelectionBox(mouse);

            // Draw()
            CONTROLLER.DrawHorizontalLine(CONTROLLER.VIEW.SelectionBox.Y);
            CONTROLLER.DrawHorizontalLine(CONTROLLER.VIEW.mSelectionBox.Y + CONTROLLER.VIEW.mSelectionBox.Height);
            CONTROLLER.DrawVerticalLine(CONTROLLER.VIEW.mSelectionBox.X);
            CONTROLLER.DrawVerticalLine(CONTROLLER.VIEW.mSelectionBox.X + CONTROLLER.VIEW.mSelectionBox.Width);
        */
    }
}
