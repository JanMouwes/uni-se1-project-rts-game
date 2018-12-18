using System;
using kbs2.Faction.FactionMVC;
using kbs2.World;
using kbs2.WorldEntity.Building.BuildingMVC;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.WorldEntity.Building
{
    public class ConstructingBuildingFactory
    {
        private Faction_Controller faction;

        public ConstructingBuildingController CreateBUCOf(IStructure structure)
        {
            return CreateNewBUC(structure.Def, faction);
        }

        public static ConstructingBuildingController CreateNewBUC(IStructureDef def,
            Faction_Controller factionController)
        {
            ViewValues viewValues = def.ViewValues;

            ConstructingBuildingController building = new ConstructingBuildingController
            {
                ConstructingBuildingModel =
                {
                    BuildingDef = def,
                    FactionController = factionController
                }
            };


            ConstructingBuildingView view = new ConstructingBuildingView("Construction")
            {
                Model = building.ConstructingBuildingModel
            };
            building.ConstructingBuildingView = view;

            return building;
        }


        public static ConstructingBuildingController CreateNewBUCAt(IStructureDef def, Coords TopLeft,
            Faction_Controller faction_Controller)
        {
            ConstructingBuildingController constructingBuildingController = CreateNewBUC(def, faction_Controller);

            constructingBuildingController.ConstructingBuildingModel = new ConstructingBuildingModel() {StartCoords = TopLeft};

            return constructingBuildingController;
        }
    }
}