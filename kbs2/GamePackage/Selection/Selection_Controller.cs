using kbs2.GamePackage.Selection;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
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

        public List<Unit_Controller> SelectedUnits { get; set; }
		

		public Selection_Controller(string lineTexture, MouseState mouseState)
        {
            Model = new Selection_Model(mouseState);
            View = new Selection_View(lineTexture);
			SelectedUnits = new List<Unit_Controller>();
        }

        public void DrawSelectionBox(List<Unit_Controller> List, MouseState CurMouseState, Matrix viewMatrix, int tileSize)
        {
            if(CurMouseState.LeftButton == ButtonState.Pressed && Model.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                View.SelectionBox = new Rectangle(CurMouseState.X, CurMouseState.Y, 0, 0);
                CheckClicked(List, CurMouseState, viewMatrix, tileSize);
            }

            if(CurMouseState.LeftButton == ButtonState.Pressed)
            {
                View.SelectionBox = new Rectangle(View.SelectionBox.X, View.SelectionBox.Y, CurMouseState.X - View.SelectionBox.X, CurMouseState.Y - View.SelectionBox.Y);
            }

            if(CurMouseState.LeftButton == ButtonState.Released)
            {
                View.SelectionBox = new Rectangle(-1, -1, 0, 0);
            }   

            Model.PreviousMouseState = CurMouseState;
        }

        public void CheckClicked(List<Unit_Controller> List, MouseState CurMouseState, Matrix viewMatrix, int tileSize)
        {
			KeyboardState state = Keyboard.GetState();

			if (CurMouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 mousePosition = new Vector2(CurMouseState.X, CurMouseState.Y);
                Vector2 worldPosition = Vector2.Transform(mousePosition, Matrix.Invert(viewMatrix));
				
				RectangleF MouselickPosition = new RectangleF((worldPosition.X / tileSize), (worldPosition.Y / tileSize), 0.01f, 0.01f);

				if (SelectedUnits.Count() > 0)
				{
					SelectedUnits.Clear();

				}
				else
				{
					foreach (Unit_Controller unit in List)
					{

						RectangleF UnitClickBox = unit.CalcClickBox();
						if (MouselickPosition.Intersects(UnitClickBox))
						{
							unit.UnitView.ImageSrc = "pikachu_idle";
							SelectedUnits.Add(unit);
						}
						if (state.IsKeyDown(Keys.LeftControl) && MouselickPosition.Intersects(UnitClickBox))
						{
							unit.UnitView.ImageSrc = "pikachu_idle";
							SelectedUnits.Add(unit);
						}


					}
				}

			}
        }

		public void CheckClicked(List<Building_Controller> List, MouseState CurMouseState, Matrix viewMatrix, int tileSize)
		{

		}
	}
}
