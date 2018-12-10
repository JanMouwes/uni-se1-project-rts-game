using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Selection;
using kbs2.World.Structs;
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

		public Selection_Controller(string lineTexture)
        {
            Model = new Selection_Model();
			SelectedUnits = new List<Unit_Controller>();
        }

		public void OnMouseStateChange(object sender, EventArgsWithPayload<MouseState> mouseEvent)
		{
            Console.WriteLine(mouseEvent.Value.LeftButton);

			if (mouseEvent.Value.LeftButton == ButtonState.Pressed && Model.PreviousMouseState.LeftButton == ButtonState.Released)
			{
				Console.WriteLine("=================================> DIPSHIT PRESSED <================================");

			}
            if (mouseEvent.Value.RightButton == ButtonState.Released && Model.PreviousMouseState.RightButton == ButtonState.Pressed)
            {
                Console.WriteLine("=================================> DIPSHIT <================================");

            }

            Model.PreviousMouseState = mouseEvent.Value;
        }

		// Checks if the unit is selected on screen with the left mouse button (drag and click) and adds it to the SelectedUnits list
		public void CheckClickedBox(List<Unit_Controller> List, Matrix viewMatrix, int tileSize, float zoom)
        {
            // Gets the current stats of the keyboard
            KeyboardState state = Keyboard.GetState();
            
            RectangleF boxDragPosition = new RectangleF(0, 0, 0, 0);

            // Transforms the mouse cursor's position on the screen to the map matrix's position
            Vector2 boxPosition = new Vector2(Model.SelectionBox.X, Model.SelectionBox.Y);
            Vector2 worldPosition = Vector2.Transform(boxPosition, Matrix.Invert(viewMatrix));
            Vector2 boxWH = new Vector2(View.Width, View.Height);

            // Bottom left to Top right
            if(boxWH.Y < 0 && boxWH.X > 0)
            {
                boxDragPosition = new RectangleF(
                    (worldPosition.X / tileSize), 
                    ((worldPosition.Y + (boxWH.Y / zoom)) / tileSize), 
                    (boxWH.X / tileSize) / zoom, 
                    ((boxWH.Y / tileSize) * -1) / zoom
                );
            }
            // Top right to Bottom left
            else if(boxWH.Y > 0 && boxWH.X < 0)
            {
                boxDragPosition = new RectangleF(
                    ((worldPosition.X + (boxWH.X / zoom)) / tileSize), 
                    (worldPosition.Y / tileSize), 
                    ((boxWH.X / tileSize) * -1) / zoom, 
                    (boxWH.Y / tileSize) / zoom
                );
            }
            // Bottom right to Top left
            else if(boxWH.Y < 0 && boxWH.X < 0)
            {
                boxDragPosition = new RectangleF(
                    ((worldPosition.X + (boxWH.X / zoom)) / tileSize), 
                    ((worldPosition.Y + (boxWH.Y / zoom)) / tileSize), 
                    ((boxWH.X / tileSize) * -1) / zoom, 
                    ((boxWH.Y / tileSize) * -1) / zoom
                );
            }
            // Top left to Bottom right
            else 
            {
                boxDragPosition = new RectangleF(
                    (worldPosition.X / tileSize), 
                    (worldPosition.Y / tileSize), 
                    (boxWH.X / tileSize) / zoom, 
                    (boxWH.Y / tileSize) / zoom
                );
            }

            Console.WriteLine($"boxDragPosition: {boxDragPosition}");
            // Checks if the control key is not pressed and clears the selectedlist
            if (!state.IsKeyDown(Keys.LeftControl))
                ClearSelectedList();

            // Goes by every unit in the map and checks if the selection box intersects with any of the unit's hitboxes
            foreach (Unit_Controller unit in List)
            {
                // Check for the intersection between selection box and unit box (rectangles)
                if (!boxDragPosition.Intersects(unit.CalcClickBox())) continue;
                // Checks if you have pressed Left Ctrl
                if (state.IsKeyDown(Keys.LeftControl))
                {
                    // Checks if a unit is already selected and if so deletes it from the list otherwise it adds the unit to the list
                    if (unit.UnitModel.Selected)
                    {
                        SelectedUnits.Remove(unit);
                    } else
                    {
                        SelectedUnits.Add(unit);
                    }
                    unit.UnitView.ImageSrcSec = unit.UnitModel.Selected ? "shadow" : "shadowselected";
                    unit.UnitModel.Selected = !unit.UnitModel.Selected;
                }
                else
                {
                    // Clears the list and adds the selected unit
                    unit.UnitModel.Selected = true;
                    unit.UnitView.ImageSrcSec = "shadowselected";
                    SelectedUnits.Add(unit);
                }
            }
        }
        // Draws selection box
        public void DrawSelectionBox(List<Unit_Controller> List, MouseState CurMouseState, Matrix matrix, int tileSize, float zoom)
        {
            if (CurMouseState.LeftButton == ButtonState.Pressed && Model.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                Model.SelectionBox = new RectangleF(CurMouseState.X, CurMouseState.Y, 0, 0);
                View.Coords = new FloatCoords() { x = CurMouseState.X, y = CurMouseState.Y};
            }

            if (CurMouseState.LeftButton == ButtonState.Pressed && Model.PreviousMouseState.LeftButton == ButtonState.Pressed)
            {
                View.Coords = new FloatCoords() { x = CalcSelectionBox(CurMouseState, matrix, tileSize).X, y = CalcSelectionBox(CurMouseState, matrix, tileSize).Y };
                View.Width = CalcSelectionBox(CurMouseState, matrix, tileSize).Width - Model.SelectionBox.X;
                View.Height = CalcSelectionBox(CurMouseState, matrix, tileSize).Height - Model.SelectionBox.Y;
            }

            if (CurMouseState.LeftButton == ButtonState.Released && Model.PreviousMouseState.LeftButton == ButtonState.Pressed)
            {
                CheckClickedBox(List, matrix, tileSize, zoom);
            }

            if (CurMouseState.LeftButton == ButtonState.Released && Model.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                ResetSelectionBox();
            }

            Model.PreviousMouseState = CurMouseState;
        }

        public void ResetSelectionBox()
        {
            View.Coords = new FloatCoords() { x = -1, y = -1 };
            View.Width = 0;
            View.Height = 0;
        }

        public RectangleF CalcSelectionBox(MouseState mouse, Matrix matrix, int tileSize)
        {
            // Bottom Left to Top Right 
            if(mouse.Y < 0 && mouse.X > 0)
            {
                Vector2 temp = Vector2.Transform(new Vector2(mouse.X, mouse.Y), Matrix.Invert(matrix));

                return new RectangleF(
                    View.Coords.x / tileSize,
                    View.Coords.y + (temp.Y / tileSize),
                    temp.X / tileSize,
                    (temp.Y / tileSize) * -1
                );
            }
            // Top Right to Bottom Left
            else if(mouse.Y > 0 && mouse.X < 0)
            {
                Vector2 temp = Vector2.Transform(new Vector2(mouse.X, mouse.Y), Matrix.Invert(matrix));

                return new RectangleF(
                    View.Coords.x + (temp.X / tileSize),
                    View.Coords.y  / tileSize,
                    (temp.X / tileSize) * -1,
                    temp.Y / tileSize
                );
            }
            // Bottom Right to Top Left
            else if(mouse.X < 0 && mouse.Y < 0)
            {
                Vector2 temp = Vector2.Transform(new Vector2(mouse.X, mouse.Y), Matrix.Invert(matrix));

                return new RectangleF(
                    View.Coords.x + (temp.X / tileSize),
                    View.Coords.y + (temp.Y / tileSize),
                    (temp.X / tileSize) * -1,
                    (temp.Y / tileSize) * -1
                );
            }
            // Top Left to Bottom Right
            else
            {
                Vector2 temp = Vector2.Transform(new Vector2(mouse.X, mouse.Y), Matrix.Invert(matrix));

                return new RectangleF(
                    View.Coords.x / tileSize,
                    View.Coords.y / tileSize,
                    temp.X / tileSize,
                    temp.Y / tileSize
                );
            }
            
        }

        // If RMB is clicked move units to mouse location
        public void MoveAction(MouseState CurMouseState, Matrix viewMatrix, int tileSize, float zoom)
        {
            if (CurMouseState.RightButton == ButtonState.Pressed && Model.PreviousMouseState.RightButton == ButtonState.Pressed)
            {
                if (SelectedUnits.Count > 0)
                {
                    Vector2 pointerPosition = new Vector2(CurMouseState.X, CurMouseState.Y);
                    Vector2 realPointerPosition = Vector2.Transform(pointerPosition, Matrix.Invert(viewMatrix));

                    foreach (Unit_Controller unit in SelectedUnits)
                    {
                        unit.LocationController.MoveTo(new FloatCoords() { x = (realPointerPosition.X / tileSize) / zoom, y = (realPointerPosition.Y / tileSize) / zoom});
                    }
                }
            }
        }
        // Clears the SelectedUnit list and puts every selected unit on false
        public void ClearSelectedList()
        {
            foreach(Unit_Controller unit in SelectedUnits)
            {
                unit.UnitModel.Selected = false;
                unit.UnitView.ImageSrcSec = "shadow";
            }

            SelectedUnits.Clear();
        }
    }
}