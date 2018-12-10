using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.View.GUI.ActionBox
{
    public class ActionBoxController
    {
        public ActionBoxModel BoxModel { get; set; }
        public ActionBoxView BoxView { get; set; }

        public ActionBoxController(FloatCoords loc)
        {
            BoxModel = new ActionBoxModel(loc);
            BoxView = new ActionBoxView(loc);
        }
    }
}
