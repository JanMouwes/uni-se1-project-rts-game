using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.Faction.FactionMVC;
using kbs2.World;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;
using kbs2.WorldEntity.Structures.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Structures.Defs;

namespace kbs2.WorldEntity.Structures
{
    public class ConstructingBuildingFactory : IDisposable
    {
        public const string CONSTRUCTION_IMAGE_SOURCE = "Construction";

        private Faction_Controller faction;

        public ConstructingBuildingFactory(Faction_Controller faction)
        {
            this.faction = faction;
        }

        public ConstructingBuildingController CreateConstructingBuildingControllerOf(BuildingDef structureDef)
        {
            ConstructingBuildingDef def = new ConstructingBuildingDef(structureDef, (int) structureDef.ConstructionTime)
            {
                BuildingShape = structureDef.BuildingShape,
                ViewValues = new ViewValues(CONSTRUCTION_IMAGE_SOURCE, structureDef.Width, structureDef.Height)
            };
            return CreateBUC(def);
        }

        public ConstructingBuildingController CreateBUC(ConstructingBuildingDef structureDef)
        {
            ConstructingBuildingController building = new ConstructingBuildingController(structureDef)
            {
                ConstructingBuildingModel =
                {
                    FactionController = faction
                }
            };


            ConstructingBuildingView view = new ConstructingBuildingView(CONSTRUCTION_IMAGE_SOURCE)
            {
                Model = building.ConstructingBuildingModel
            };
            building.ConstructingBuildingView = view;

            return building;
        }

        [Obsolete]
        private static ConstructingBuildingController CreateNewBUC(ConstructingBuildingDef def,
            Faction_Controller factionController)
        {
            ConstructingBuildingController building = new ConstructingBuildingController(def)
            {
                ConstructingBuildingModel =
                {
                    FactionController = factionController
                }
            };


            ConstructingBuildingView view = new ConstructingBuildingView(CONSTRUCTION_IMAGE_SOURCE)
            {
                Model = building.ConstructingBuildingModel
            };
            building.ConstructingBuildingView = view;

            return building;
        }

        [Obsolete]
        public static ConstructingBuildingController CreateNewBUCAt(IStructureDef def, Coords topLeft, Faction_Controller factionController)
        {
            ConstructingBuildingController constructingBuildingController = CreateNewBUC((ConstructingBuildingDef) def, factionController);

            constructingBuildingController.ConstructingBuildingModel = new ConstructingBuildingModel() {StartCoords = topLeft};

            return constructingBuildingController;
        }

        public void Dispose()
        {
            faction = null;
        }
    }
}