using kbs2.World;
using kbs2.WorldEntity.Building.BuildingMVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Building
{
    public static class BuildingFactory
    {
        public static Building_Controller CreateNewBuilding(BuildingDef def, Coords TopLeft)
        {
            Building_Controller building_Controller = new Building_Controller();

            Building_Model model = new Building_Model(TopLeft);
            building_Controller.Model = model;
            BuildingView view = new BuildingView(def.imageSrc, def.height, def.width)
            {
                BuildingModel = model
            };
            building_Controller.View = view;
            

            return building_Controller;
        }
    }
}
