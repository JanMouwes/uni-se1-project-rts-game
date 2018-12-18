using kbs2.Actions.ActionModels;
using kbs2.World;
using kbs2.World.Enums;
using kbs2.World.Structs;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Unit;
using kbs2.WorldEntity.Unit.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Actions
{
    public class ActionDelegates
    {
        public delegate void GameAction(IActionModel actionModel, FloatCoords target);

        public void SpawnUnit(IActionModel actionModel, FloatCoords target)
        {
            SpawnActionModel spawnActionModel = (SpawnActionModel) actionModel;

            DBController.OpenConnection("DefDex");
            UnitDef unitdef = DBController.GetDefinitionFromUnit(spawnActionModel.Id);
            DBController.CloseConnection();
            UnitController unit = UnitFactory.CreateNewUnit(unitdef, target, spawnActionModel.World.WorldModel);
            spawnActionModel.Spawner.SpawnUnit(unit, spawnActionModel.Faction);
        }

        public void SpawnBuilding(IActionModel actionModel, FloatCoords target)
        {
            SpawnActionModel spawnActionModel = (SpawnActionModel) actionModel;

            DBController.OpenConnection("DefDex");
            BuildingDef buildingDef = DBController.GetDefinitionBuilding(spawnActionModel.Id);
            DBController.CloseConnection();

            //model.spawner.SpawnUnit(, model.faction);

            List<Coords> buidlingcoords = new List<Coords>();
            foreach (Coords stuff in buildingDef.BuildingShape)
            {
                buidlingcoords.Add((Coords) target + stuff);
            }


            // TODO get from db
            List<TerrainType> whitelist = new List<TerrainType>();
            whitelist.Add(TerrainType.Grass);
            whitelist.Add(TerrainType.Default);


            if (!spawnActionModel.World.AreTerrainCellsLegal(buidlingcoords, whitelist)) return;
            
            ConstructingBuildingController constructingBuilding = ConstructingBuildingFactory.CreateNewBUCAt(buildingDef, (Coords) target, spawnActionModel.Faction);
            spawnActionModel.Spawner.SpawnConstructingBuilding(constructingBuilding, spawnActionModel.ConstructionTime);
        }
    }
}