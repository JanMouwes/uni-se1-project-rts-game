using kbs2.GamePackage;
using Microsoft.Xna.Framework.Input;

namespace kbs2.GamePackage.EventArgs
{
	public class MouseClickEventArgs : System.EventArgs
	{
		public MouseState MouseState { get; }
		
		public MouseClickEventArgs(MouseState mouseState)
		{
			MouseState = mouseState;
		}
	}
}
