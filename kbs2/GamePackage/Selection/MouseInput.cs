using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using kbs2.WorldEntity.Unit.MVC;
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
            gameModel = game.GameModel;
			PreviousMouseState = new MouseState();
            Selection = new Selection_Controller(game, "PurpleLine");
		}



		public void OnMouseStateChange(object sender, EventArgsWithPayload<MouseState> mouseEvent)
		{
			
			if (mouseEvent.Value.LeftButton != PreviousMouseState.LeftButton)
            { 
				MouseInputStatus = mouseEvent.Value.LeftButton;
                MouseState temp = Mouse.GetState();
                GuiOrMap(new FloatCoords { x = temp.X, y = temp.Y },mouseEvent.Value,true);

            }
			if (mouseEvent.Value.RightButton != PreviousMouseState.RightButton)
			{
				MouseInputStatus = mouseEvent.Value.RightButton;
                MouseState temp = Mouse.GetState();
                GuiOrMap(new FloatCoords { x = temp.X, y = temp.Y }, mouseEvent.Value,false);

            }
            

			PreviousMouseState = mouseEvent.Value;
		}

        public void GuiOrMap(FloatCoords mousecoords,MouseState mouseState,bool leftButton)
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
                if (mouseState.LeftButton == ButtonState.Pressed && leftButton) 
                {
                    Selection.ButtonPressed(mousecoords);
                }
                if (mouseState.LeftButton == ButtonState.Released && leftButton) 
                {
                    Selection.ButtonRelease(Keyboard.GetState().IsKeyDown(Keys.LeftShift));
                }
                if(mouseState.RightButton == ButtonState.Pressed && !leftButton)
                {
                    Selection.move(Keyboard.GetState().IsKeyDown(Keys.LeftShift));
                }
                
            }
        }


        
	}
}