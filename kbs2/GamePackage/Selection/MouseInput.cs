using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework.Input;

namespace kbs2.GamePackage
{

    public class MouseInput
    {

        public MouseState PreviousMouseState { get; set; }
        public ButtonState MouseInputStatus { get; set; }
        public GameModel gameModel { get; set; }
        public Selection_Controller Selection { get; set; }

		public MouseInput(GameController game)
		{
            gameModel = game.gameModel;
			PreviousMouseState = new MouseState();
            Selection = new Selection_Controller(game, "PurpleLine");
		}



		public void OnMouseStateChange(object sender, EventArgsWithPayload<MouseState> mouseEvent)
		{
			
			if (mouseEvent.Value.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released)
			{
				MouseInputStatus = mouseEvent.Value.LeftButton;
                MouseState temp = Mouse.GetState();
                GuiOrMap(new FloatCoords { x = temp.X, y = temp.Y });

            }
			if (mouseEvent.Value.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed)
			{
				MouseInputStatus = mouseEvent.Value.RightButton;
                MouseState temp = Mouse.GetState();
                GuiOrMap(new FloatCoords { x = temp.X, y = temp.Y });

            }
            

			PreviousMouseState = mouseEvent.Value;
		}

        public void GuiOrMap(FloatCoords mousecoords)
        {
            List<IViewImage> Clicked = (from Item in gameModel.GuiItemList
                                        where mousecoords.x>= Item.Coords.x && mousecoords.y >= Item.Coords.y
                                        && mousecoords.x <= Item.Coords.x + Item.Width && mousecoords.y <= Item.Coords.y + Item.Height
                                        select Item).ToList();

            if(Clicked.Count > 0)
            {
                //TODO geef door aan gui
                Console.WriteLine("gui Click");
            }
            else
            {

                Selection.ButtonPressed(mousecoords);
            }
        }
	}
}