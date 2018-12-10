using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.View.GUI.ActionBox
{
    public class ActionBoxModel
    {
        public ActionBoxTextView Text { get; set; }

        public ActionBoxModel(FloatCoords loc)
        {
            Text = new ActionBoxTextView(loc);
        }
    }
}
