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
        public static BuildingController CreateNewBuilding(BuildingDef def)
        {
            BuildingController buildingController = new BuildingController(def);

            BuildingView view = new BuildingView(def.Image, def.Height, def.Width)
            {
                BuildingModel = buildingController.Model
            };
            buildingController.View = view;

            return buildingController;
        }
    }
}