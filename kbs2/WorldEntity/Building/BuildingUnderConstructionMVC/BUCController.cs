using System;
using kbs2.Desktop.World.World;

namespace kbs2.WorldEntity.Building.BuildingUnderConstructionMVC
{
    public class BUCController
    {

        public BUCModel BUCModel { get; set; }
        public BUCView BUCView { get; set; }
        public WorldController World { get; set; }


        public BUCController()
        {
        }


        public void SetBuilding()
        {
            Building_Controller building = BuildingFactory.CreateNewBuilding(BUCModel.BuildingDef, BUCModel.TopLeft);
            World.AddBuilding(BUCModel.BuildingDef, building);
        }
    }
}
