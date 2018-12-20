using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Location;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.WorldEntity.Unit
{
	public static class UnitFactory
	{
		public static UnitController CreateNewUnit(UnitDef def, FloatCoords TopLeft, WorldController worldController)
        {
            UnitController UnitController = new UnitController();
            
            UnitController.UnitView.Texture = def.Image;
            UnitController.UnitView.Width = def.Width;
            UnitController.UnitView.Height = def.Height;
            UnitController.UnitModel.Speed = def.Speed;
            Location_Controller location = new Location_Controller(worldController, TopLeft.x,TopLeft.y);
            location.LocationModel.parent = UnitController;
            UnitController.LocationController = location;
            return UnitController;
        }
    }
}



