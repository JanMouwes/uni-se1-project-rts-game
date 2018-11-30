using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace kbs2.GamePackage.EventArgs
{
	public class MouseEventArgs : System.EventArgs
	{
		public MouseState mouseState { get; }

		public MouseEventArgs(MouseState State)
		{
			mouseState = State;
		}
	}
}
