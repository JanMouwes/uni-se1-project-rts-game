using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage.EventArgs
{
	public class EventArgsWithPayload<T> : System.EventArgs
	{
		public T Value { get; set; }

		public EventArgsWithPayload(T val)
		{
			Value = val;
		}
	}
}
