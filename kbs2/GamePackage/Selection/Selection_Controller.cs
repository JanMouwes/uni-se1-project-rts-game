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
	//public delegate void MouseEventObserver<TPayloadType>(object sender, EventArgs_WithPayload<TPayloadType> eventArgs);

	public class Selection_Controller
    {
		

		public Selection_Model Model { get; set; }
        public Selection_View View { get; set; }

		public MouseState CurMouseState { get; set; }

		public List<Unit_Controller> SelectedUnits { get; set; }

		public Selection_Controller(string lineTexture, MouseState mouseState)
        {
            Model = new Selection_Model(mouseState);
            View = new Selection_View(lineTexture);
			SelectedUnits = new List<Unit_Controller>();
        }
		
		public MouseState MouseActivity()
		{
			return Mouse.GetState();
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
            // Bottom left to Top right
            if(boxPosition2.Y < 0 && boxPosition2.X > 0)
            {
                boxDragPosition = new RectangleF(
                    (worldPosition.X / tileSize), 
                    ((worldPosition.Y + (boxPosition2.Y / zoom)) / tileSize), 
                    (boxPosition2.X / tileSize) / zoom, 
                    ((boxPosition2.Y / tileSize) * -1) / zoom
                );
            }
            // Top right to Bottom left
            else if(boxPosition2.Y > 0 && boxPosition2.X < 0)
            {
                boxDragPosition = new RectangleF(
                    ((worldPosition.X + (boxPosition2.X / zoom)) / tileSize), 
                    (worldPosition.Y / tileSize), 
                    ((boxPosition2.X / tileSize) * -1) / zoom, 
                    (boxPosition2.Y / tileSize) / zoom
                );
            }
            // Bottom right to Top left
            else if(boxPosition2.Y < 0 && boxPosition2.X < 0)
            {
                boxDragPosition = new RectangleF(
                    ((worldPosition.X + (boxPosition2.X / zoom)) / tileSize), 
                    ((worldPosition.Y + (boxPosition2.Y / zoom)) / tileSize), 
                    ((boxPosition2.X / tileSize) * -1) / zoom, 
                    ((boxPosition2.Y / tileSize) * -1) / zoom
                );
            }
            // Top left to Bottom right
            else 
            {
                boxDragPosition = new RectangleF(
                    (worldPosition.X / tileSize), 
                    (worldPosition.Y / tileSize), 
                    (boxPosition2.X / tileSize) / zoom, 
                    (boxPosition2.Y / tileSize) / zoom
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
                        if (SelectedUnits.Count > 0)
                        {
                            SelectedUnits[0].LocationController.MoveTo(new FloatCoords() { x = 20f, y = 20f });
                        }
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

		// TODO: delegate eventargspayload toevoegen en shit

		//public event MouseActivity<MouseState> MouseStateChange;




		public void CheckClicked(List<Building_Controller> List, MouseState CurMouseState, Matrix viewMatrix, int tileSize)
		{

		}
		
		
	}
}