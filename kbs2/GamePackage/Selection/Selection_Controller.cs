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

		public List<Unit_Controller> SelectedUnits { get; set; }

		public Selection_Controller(string lineTexture)
        {
            View = new Selection_View(lineTexture, new FloatCoords { x = -1, y = -1 }, 0, 0);
            Model = new Selection_Model();
			SelectedUnits = new List<Unit_Controller>();
        }
		
		
        public void DrawSelectionBox(List<Unit_Controller> List, MouseState CurMouseState, Matrix viewMatrix, int tileSize, float zoom)
        {
            if(CurMouseState.LeftButton == ButtonState.Pressed && Model.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                Model.SelectionBox = new RectangleF(CurMouseState.X, CurMouseState.Y, 0, 0);
                Vector2 boxPosition = new Vector2(CurMouseState.X, CurMouseState.Y);
                Vector2 worldPosition = Vector2.Transform(boxPosition, Matrix.Invert(viewMatrix));
                View.Coords = new FloatCoords() { x = worldPosition.X, y = worldPosition.Y };
                View.Width = 0;
                View.Height = 0;
                AdjustViewBox(CurMouseState, viewMatrix, tileSize, zoom);
            }

            if(CurMouseState.LeftButton == ButtonState.Pressed)
            {
                Model.SelectionBox = new RectangleF(Model.SelectionBox.X, Model.SelectionBox.Y, CurMouseState.X - Model.SelectionBox.X, CurMouseState.Y - Model.SelectionBox.Y);
                View.Coords = new FloatCoords() { x = Model.SelectionBox.X, y = Model.SelectionBox.Y };
                View.Width = CurMouseState.X - Model.SelectionBox.X;
                View.Height = CurMouseState.Y - Model.SelectionBox.Y;
                AdjustViewBox(CurMouseState, viewMatrix, tileSize, zoom);
            }

            if (CurMouseState.LeftButton == ButtonState.Released && Model.PreviousMouseState.LeftButton == ButtonState.Pressed)
                CheckClickedBox(List, viewMatrix, tileSize, zoom);

            if (CurMouseState.LeftButton == ButtonState.Released && Model.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                Model.SelectionBox = new RectangleF(-1f, -1f, 0f, 0f);
                View.Coords = new FloatCoords() { x = -1, y = -1 };
                View.Width = 0;
                View.Height = 0;
                AdjustViewBox(CurMouseState, viewMatrix, tileSize, zoom);
            }

            Model.PreviousMouseState = CurMouseState;
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

            Vector2 boxPosition2 = new Vector2(Model.SelectionBox.Width, Model.SelectionBox.Height);
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

        public void AdjustViewBox(MouseState CurMouseState, Matrix viewMatrix, int tileSize, float zoom)
        {
            Model.ViewList.Clear();

            Vector2 boxPosition = new Vector2(CurMouseState.X, CurMouseState.Y);
            Vector2 worldPositionXY = Vector2.Transform(boxPosition, Matrix.Invert(viewMatrix));

            Vector2 boxPositionWH = new Vector2(View.Width, View.Height);

            Model.Top = new RectangleF(((worldPositionXY.X - View.Width) / tileSize), ((worldPositionXY.Y - View.Height) / tileSize), boxPositionWH.X / tileSize, 0.2f);
            Model.Left = new RectangleF(((worldPositionXY.X - View.Width) / tileSize), ((worldPositionXY.Y - View.Height) / tileSize), 0.2f, boxPositionWH.Y / tileSize);
            Model.Right = new RectangleF(worldPositionXY.X / tileSize, ((worldPositionXY.Y - View.Height) / tileSize), 0.2f, boxPositionWH.Y / tileSize);
            Model.Bottom = new RectangleF(worldPositionXY.X / tileSize, worldPositionXY.Y / tileSize, boxPositionWH.X / tileSize, 0.2f);


            

            Model.ViewList.Add(new Selection_View(View.Texture, new FloatCoords() { x = ((worldPositionXY.X - View.Width) / tileSize), y = ((worldPositionXY.Y - View.Height) / tileSize) }, (boxPositionWH.X / tileSize), 0.2f));
            Model.ViewList.Add(new Selection_View(View.Texture, new FloatCoords() { x = ((worldPositionXY.X - View.Width) / tileSize), y = ((worldPositionXY.Y - View.Height) / tileSize) }, 0.2f, (boxPositionWH.Y / tileSize)));
            Model.ViewList.Add(new Selection_View(View.Texture, new FloatCoords() { x = worldPositionXY.X / tileSize, y = ((worldPositionXY.Y - View.Height) / tileSize) }, 0.2f, (boxPositionWH.Y / tileSize)));
            Model.ViewList.Add(new Selection_View(View.Texture, new FloatCoords() { x = (worldPositionXY.X - View.Width) / tileSize, y = worldPositionXY.Y / tileSize }, (boxPositionWH.X / tileSize), 0.2f));


            Console.WriteLine(Model.Top);
            Console.WriteLine(Model.Left);
            Console.WriteLine(Model.Right);
            Console.WriteLine(Model.Bottom);
        }

        //Drawing the horizontal and vertical selectionbox
        public void DrawHorizontalLine(int PositionY)
        {
            if (View.Height > 0)
            {
                for (int i = -2; i <= View.Height; i += 10)
                {
                    if (View.Height - i >= 0)
                    {
                        //Model.Box.Add(new Selection_View(View.Texture, new FloatCoords() { x = PositionY, y = View.Coords.y + i }, 10, 5));
                        //spriteBatch.Draw(texture, new Rectangle(PositionX, View.Coords.y + i, 10, 5),
                        //    new Rectangle(0, 0, texture.Width, texture.Height), Color.White, MathHelper.ToRadians(90),
                        //    new Vector2(0, 0), SpriteEffects.None, 0);
                    }
                }
            }

            else if (View.Height < 0)
            {
                for (int i = 0; i >= View.Height; i -= 10)
                {
                    if (View.Height - i <= 0)
                    {
                        //Model.Box.Add(new Selection_View(View.Texture, new FloatCoords() { x = PositionY - 10, y = View.Coords.y + i }, 10, 5));
                        //spriteBatch.Draw(texture, new Rectangle(PositionX - 10, Selection.View.Selection.Y + i, 10, 5),
                        //    Color.White);
                    }
                }
            }
        }
        
        public void DrawVerticalLine(int PositionX)
        {
            if (View.Width > 0)
            {
                for (int i = 0; i <= View.Width - 10; i += 10)
                {
                    if (View.Width - i >= 0)
                    {
                        //Model.Box.Add(new RectangleF(View.Texture, new FloatCoords() { x = View.Coords.x + i, y = PositionX }, 10, 5));
                    }
                }
            }
            else if (View.Width < 0)
            {
                for (int i = -10; i >= View.Width; i -= 10)
                {
                    if (View.Width - i <= 0)
                    {
                        //Model.Box.Add(new Selection_View(View.Texture, new FloatCoords() { x = View.Coords.x + i, y = PositionX }, 10, 5));
                        //spriteBatch.Draw(texture, new Rectangle(Selection.View.Selection.X + i, PositionY, 10, 5),
                        //    Color.White);
                    }
                }
            }
        }
    }
}