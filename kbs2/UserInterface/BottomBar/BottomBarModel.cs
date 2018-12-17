using kbs2.Unit.Model;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.UserInterface.BottomBar
{
	public class BottomBarModel
	{
        public List<BottomBarStatView> StatViews { get; set; }
        public BottomBarView MainView { get; set; }

        public BottomBarModel(BottomBarView mainView)
        {
            MainView = mainView;
            StatViews = new List<BottomBarStatView>();
        }
	}
}
