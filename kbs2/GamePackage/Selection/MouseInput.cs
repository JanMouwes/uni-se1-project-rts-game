using System;
using kbs2.GamePackage.EventArgs;
using Microsoft.Xna.Framework.Input;

namespace kbs2.GamePackage
{

	public class MouseInput
	{
		
		public MouseState PreviousMouseState { get; set; }
		public ButtonState MouseInputStatus { get; set; } 

		public MouseInput()
		{
			
			PreviousMouseState = new MouseState();
		}



		public void OnMouseStateChange(object sender, EventArgsWithPayload<MouseState> mouseEvent)
		{
			
			if (mouseEvent.Value.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released)
			{
				MouseInputStatus = mouseEvent.Value.LeftButton;

			}
			if (mouseEvent.Value.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed)
			{
				MouseInputStatus = mouseEvent.Value.RightButton;

			}

			PreviousMouseState = mouseEvent.Value;
		}
	}
}