using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Unit.Unit;
using kbs2.World;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.WorldEntity.Unit
{
	public static class UnitFactory
	{

		public static Unit_Controller CreateNewUnit(UnitDef def, Coords TopLeft)
		{
			Unit_Controller UnitController = new Unit_Controller();




			Unit_Model model = new Unit_Model(TopLeft);
			building_Controller.Model = model;
			BuildingView view = new BuildingView(def.imageSrc, def.height, def.width)
			{
				BuildingModel = model
			};
			building_Controller.View = view;


			return UnitController;
		}
	}
}
