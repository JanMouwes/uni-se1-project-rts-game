using System;
using kbs2.Faction.FactionMVC;
using kbs2.World;
using kbs2.WorldEntity.Building.BuildingMVC;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.WorldEntity.Building
{
    public class ConstructingBuildingFactory : IDisposable
    {
        public const string ConstructionImageSource = "Construction";

        private Faction_Controller faction;

        public ConstructingBuildingFactory(Faction_Controller faction)
        {
            this.faction = faction;
        }

        public ConstructingBuildingController CreateBUC(ConstructingBuildingDef structureDef)
        {
            return CreateNewBUC(structureDef, faction);
        }

        public static ConstructingBuildingController CreateNewBUC(ConstructingBuildingDef def,
            Faction_Controller factionController)
        {
            ConstructingBuildingController building = new ConstructingBuildingController(def)
            {
                ConstructingBuildingModel =
                {
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


        public static ConstructingBuildingController CreateNewBUCAt(IStructureDef def, Coords TopLeft, Faction_Controller faction_Controller)
        {
            ConstructingBuildingController constructingBuildingController = CreateNewBUC((ConstructingBuildingDef) def, faction_Controller);

            constructingBuildingController.ConstructingBuildingModel = new ConstructingBuildingModel() {StartCoords = TopLeft};

            return constructingBuildingController;
        }

        public void Dispose()
        {
        }
    }
}