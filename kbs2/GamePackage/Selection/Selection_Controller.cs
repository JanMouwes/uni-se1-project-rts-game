using kbs2.GamePackage.EventArgs;
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

        public void DrawSelectionBox(List<Unit_Controller> List, MouseState CurMouseState, Matrix viewMatrix, int tileSize, float zoom)
        {
            if(CurMouseState.LeftButton == ButtonState.Pressed && Model.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                View.SelectionBox = new RectangleF(CurMouseState.X, CurMouseState.Y, 0, 0);
            }

            if(CurMouseState.LeftButton == ButtonState.Pressed)
            {
                View.SelectionBox = new RectangleF(View.SelectionBox.X, View.SelectionBox.Y, CurMouseState.X - View.SelectionBox.X, CurMouseState.Y - View.SelectionBox.Y);
            }

            if (CurMouseState.LeftButton == ButtonState.Released && Model.PreviousMouseState.LeftButton == ButtonState.Pressed)
            {
                CheckClickedBox(List, CurMouseState, viewMatrix, tileSize, zoom);
            }

            if (CurMouseState.LeftButton == ButtonState.Released && Model.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                View.SelectionBox = new RectangleF(-1f, -1f, 0f, 0f);
            }

            Model.PreviousMouseState = CurMouseState;
        }
        // Checks if the unit is selected on screen with the left mouse button (drag and click) and adds it to the SelectedUnits list
        public void CheckClickedBox(List<Unit_Controller> List, MouseState CurMouseState, Matrix viewMatrix, int tileSize, float zoom)
        {
            // Gets the current stats of the keyboard
            KeyboardState state = Keyboard.GetState();
            
            RectangleF boxDragPosition = new RectangleF(0, 0, 0, 0);
            // Transforms the mouse cursor's position on the screen to the map matrix's position
            Vector2 boxPosition = new Vector2(View.SelectionBox.X, View.SelectionBox.Y);
            Vector2 worldPosition = Vector2.Transform(boxPosition, Matrix.Invert(viewMatrix));

            Vector2 boxPosition2 = new Vector2(View.SelectionBox.Width, View.SelectionBox.Height);
            // Makes a float rectangle according to which way you drag your box with your mouse
            if(boxPosition2.Y < 0 && boxPosition2.X > 0)
            {
                boxDragPosition = new RectangleF(
                    (worldPosition.X / tileSize), 
                    ((worldPosition.Y + boxPosition2.Y) / tileSize), 
                    (boxPosition2.X / tileSize), 
                    ((boxPosition2.Y / tileSize) * -1)
                );
            }
            else if(boxPosition2.Y > 0 && boxPosition2.X < 0)
            {
                boxDragPosition = new RectangleF(
                    ((worldPosition.X + boxPosition2.X) / tileSize), 
                    (worldPosition.Y / tileSize), 
                    ((boxPosition2.X / tileSize) * -1), 
                    (boxPosition2.Y / tileSize)
                );
            }
            else if(boxPosition2.Y < 0 && boxPosition2.X < 0)
            {
                boxDragPosition = new RectangleF(
                    ((worldPosition.X + boxPosition2.X) / tileSize), 
                    ((worldPosition.Y + boxPosition2.Y) / tileSize), 
                    ((boxPosition2.X / tileSize) * -1), 
                    ((boxPosition2.Y / tileSize) * -1)
                );
            }
            else 
            {
                boxDragPosition = new RectangleF(
                    (worldPosition.X / tileSize), 
                    (worldPosition.Y / tileSize), 
                    (boxPosition2.X / tileSize), 
                    (boxPosition2.Y / tileSize)
                );
            }

            Console.WriteLine($"boxDragPosition: {boxDragPosition}");

            int count = 0;
            // Goes by every unit in the map and checks if the selection box intersects with any of the unit's hitboxes
            foreach (Unit_Controller unit in List)
            {
                // Check for the intersection between selection box and unit box (rectangles)
                if (boxDragPosition.Intersects(unit.CalcClickBox()))
                {
                    // Checks if you have pressed Left Ctrl
                    if (state.IsKeyDown(Keys.LeftControl))
                    {
                        // Checks if a unit is already selected and if so deletes it from the list otherwise it adds the unit to the list
                        if (unit.UnitModel.Selected)
                        {
                            unit.UnitModel.Selected = false;
                            unit.UnitView.ImageSrcSec = "shadow";
                            SelectedUnits.Remove(unit);
                            count++;
                        } else
                        {
                            unit.UnitModel.Selected = true;
                            unit.UnitView.ImageSrcSec = "shadowselected";
                            SelectedUnits.Add(unit);
                            count++;
                        }
                    } else
                    {
                        // Clears the list and adds the selected unit
                        

                        unit.UnitModel.Selected = true;
                        unit.UnitView.ImageSrcSec = "shadowselected";
                        SelectedUnits.Add(unit);

                        count++;
                    }
                }
            }

            // If no unit is selected clears the entire selection list of units
            if(count == 0)
            {
                // Deselects all units in the selected unit list
                ClearSelectedList();
            }
        }

        public void ClearSelectedList()
        {
            foreach(Unit_Controller unit in SelectedUnits)
            {
                unit.UnitModel.Selected = false;
                unit.UnitView.ImageSrcSec = "shadow";
            }

            SelectedUnits.Clear();
        }

        public void CheckClicked(List<Building_Controller> List, MouseState CurMouseState, Matrix viewMatrix, int tileSize)
		{

		}

		public event EventHandler Clicky;

		protected virtual void OnClicky(MouseEventArgs e)
		{
			EventHandler handler = Clicky;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}