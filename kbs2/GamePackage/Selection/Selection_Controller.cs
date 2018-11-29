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

        public void CheckClickedBox(List<Unit_Controller> List, MouseState CurMouseState, Matrix viewMatrix, int tileSize, float zoom)
        {
            KeyboardState state = Keyboard.GetState();

            RectangleF boxDragPosition = new RectangleF(0, 0, 0, 0);

            Vector2 boxPosition = new Vector2(View.SelectionBox.X, View.SelectionBox.Y);
            Vector2 worldPosition = Vector2.Transform(boxPosition, Matrix.Invert(viewMatrix));

            Vector2 boxPosition2 = new Vector2(View.SelectionBox.Width, View.SelectionBox.Height);
                
            if(boxPosition2.Y < 0 && boxPosition2.X > 0)
            {
                boxDragPosition = new RectangleF((worldPosition.X / tileSize), ((worldPosition.Y + boxPosition2.Y) / tileSize), (boxPosition2.X / tileSize), ((boxPosition2.Y / tileSize) * -1));
            }
            else if(boxPosition2.Y > 0 && boxPosition2.X < 0)
            {
                boxDragPosition = new RectangleF(((worldPosition.X + boxPosition2.X) / tileSize), (worldPosition.Y / tileSize), ((boxPosition2.X / tileSize) * -1), (boxPosition2.Y / tileSize));
            }
            else if(boxPosition2.Y < 0 && boxPosition2.X < 0)
            {
                boxDragPosition = new RectangleF(((worldPosition.X + boxPosition2.X) / tileSize), ((worldPosition.Y + boxPosition2.Y) / tileSize), ((boxPosition2.X / tileSize) * -1), ((boxPosition2.Y / tileSize) * -1));
            }
            else 
            {
                boxDragPosition = new RectangleF((worldPosition.X / tileSize), (worldPosition.Y / tileSize), (boxPosition2.X / tileSize), (boxPosition2.Y / tileSize));
            }

            Console.WriteLine($"boxDragPosition: {boxDragPosition}");

            int count = 0;

            foreach (Unit_Controller unit in List)
            {
                RectangleF UnitClickBox = unit.CalcClickBox();
                if (boxDragPosition.Intersects(UnitClickBox))
                {
                    unit.UnitView.ImageSrc = "PurpleLine";
                    SelectedUnits.Add(unit);
                    count++;
                }
                else
                {
                    unit.UnitView.ImageSrc = "pikachu_idle";
                    SelectedUnits.Remove(unit);
                }
            }

            if(count == 0)
            {
                SelectedUnits.Clear();
                foreach (Unit_Controller unit in List)
                {
                    unit.UnitView.ImageSrc = "pikachu_idle";
                }
            }

            Console.WriteLine($"UNITSLIST: {SelectedUnits.Count()}");
        }

        public void CheckClicked(List<Building_Controller> List, MouseState CurMouseState, Matrix viewMatrix, int tileSize)
		{

		}
	}
}
