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
			Console.WriteLine(mouseEvent.Value.LeftButton);

			if (mouseEvent.Value.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released)
			{
				MouseInputStatus = mouseEvent.Value.LeftButton;
				Console.WriteLine("=================================> DIPSHIT PRESSED <================================");

			}
			if (mouseEvent.Value.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed)
			{
				MouseInputStatus = mouseEvent.Value.RightButton;
				Console.WriteLine("=================================> DIPSHIT <================================");

			}

			PreviousMouseState = mouseEvent.Value;
		}
	}
}