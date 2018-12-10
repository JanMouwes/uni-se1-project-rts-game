using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.GamePackage.EventArgs;
using Microsoft.Xna.Framework.Input;

namespace kbs2.GamePackage.MouseInput
{
	public class MouseInput
	{

		public MouseState PreviousMouseState { get; set; }

		public MouseInput()
		{
			PreviousMouseState = new MouseState();
		}

		public void OnMouseStateChange(object sender, EventArgsWithPayload<MouseState> mouseEvent)
		{
			Console.WriteLine(mouseEvent.Value.LeftButton);

			if (mouseEvent.Value.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released)
			{
				Console.WriteLine("=================================> DIPSHIT PRESSED <================================");

			}
			if (mouseEvent.Value.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed)
			{
				Console.WriteLine("=================================> DIPSHIT <================================");

			}

			PreviousMouseState = mouseEvent.Value;
		}
	}
}
