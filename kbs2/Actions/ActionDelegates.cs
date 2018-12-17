using kbs2.Actions.ActionModels;
using kbs2.Unit.Unit;
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
            Spawn_Model model = (Spawn_Model)actionModel;
            
            DBController.OpenConnection("DefDex");
            UnitDef unitdef = DBController.GetDefinitionFromUnit(model.Index);
            DBController.CloseConnection();
            Unit_Controller unit = UnitFactory.CreateNewUnit(unitdef, target, model.World.WorldModel);
            model.spawner.SpawnUnit(unit, model.faction);
        }

        public void SpawnBuilding(IActionModel actionModel, FloatCoords target)
        {
            Spawn_Model model = (Spawn_Model)actionModel;

            DBController.OpenConnection("DefDex");
            BuildingDef buildingDef = DBController.GetDefinitionBuilding(model.Index);
            DBController.CloseConnection();
           
            //model.spawner.SpawnUnit(, model.faction);

            List<Coords> buidlingcoords = new List<Coords>();
            foreach (Coords stuff in buildingDef.BuildingShape)
            {
                buidlingcoords.Add((Coords)target + stuff);
            }


            // TODO get from db
            List<TerrainType> whitelist = new List<TerrainType>();
            whitelist.Add(TerrainType.Grass);
            whitelist.Add(TerrainType.Default);


            if (model.World.checkTerainCells(buidlingcoords, whitelist))
            {
                BUCController BUC = BUCFactory.CreateNewBUC(buildingDef, (Coords)target, model.faction);
                model.spawner.SpawnBUC(BUC, buildingDef, model.ConstructionTime, model.faction);
            }
        }

    }
}
