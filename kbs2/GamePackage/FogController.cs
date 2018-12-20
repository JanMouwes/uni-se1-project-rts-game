using kbs2.Faction.FactionMVC;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Building.BuildingMVC;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Unit.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage
{
    public class FogController
    {
        public Faction_Controller faction { get; set; }
        public WorldController worldController { get; set; }

        public void UpdateViewModes(ViewMode mode)
        {
            foreach(UnitController unit in faction.FactionModel.Units)
            {
                UpdateViewMode(mode, unit.viewrange, unit.center);
            }
            foreach(BuildingController building in faction.FactionModel.Buildings)
            {
                UpdateViewMode(mode, building.viewrange, building.center);
            }
            foreach(ConstructingBuildingController building in faction.FactionModel.BUCs)
            {
                UpdateViewMode(mode, building.viewrange, building.center);
            }
        }



        public void UpdateViewMode(ViewMode mode , int viewrange, FloatCoords coords)
        {
            for (int x = (viewrange) * -1; x <= viewrange; x++)
            {
                for (int y = (viewrange) * -1; y <= viewrange; y++)
                {
                    Coords tempcoords = (Coords)new FloatCoords { x = x + coords.x, y = y + coords.y };
                    if (!(DistanceCalculator.getDistance2d((FloatCoords)tempcoords, coords) < viewrange)) continue;
                    WorldCellController cellController = worldController.GetCellFromCoords(tempcoords);
                    if (cellController == null) continue;
                    cellController.ChangeViewMode(mode);
                    if (cellController.worldCellModel.BuildingOnTop == null) continue;
                    if (cellController.worldCellModel.BuildingOnTop.GetType() == typeof(BuildingController))
                    {
                        ((BuildingController)cellController.worldCellModel.BuildingOnTop).View.ViewMode = mode;
                    }
                    if (cellController.worldCellModel.BuildingOnTop.GetType() == typeof(ConstructingBuildingController))
                    {
                        ((ConstructingBuildingController)cellController.worldCellModel.BuildingOnTop).ConstructingBuildingView.ViewMode = mode;
                    }
                }
            }
        }
    }
}
