using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.UserInterface.MenuBar
{
    public class MenuBarModel
    {
        public List<MenuButton> MenuButtons { get; set; }
        public RightButtonBar Bar { get; set; }

        public MenuBarModel(RightButtonBar parent)
        {
            Bar = parent;
            MenuButtons = new List<MenuButton>();
        }
    }
}
