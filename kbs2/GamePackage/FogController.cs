using kbs2.Faction.FactionMVC;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.GamePackage.EventArgs;
using kbs2.WorldEntity.Structures.BuildingMVC;
using kbs2.WorldEntity.Structures.BuildingUnderConstructionMVC;

namespace kbs2.GamePackage
{
    public class FogController
    {
        public Faction_Controller faction { get; set; }
        public WorldController worldController { get; set; }

        public FogController(Faction_Controller faction, WorldController worldController)
        {
            this.faction = faction;
            this.worldController = worldController;

//            foreach (UnitController unit in faction.FactionModel.Units)
//            {
//                unit.OnMove += (sender, newLocation) => UpdateViewMode(ViewMode.Fog, unit.ViewRange, newLocation.Value);
//                unit.OnMove += (sender, newLocation) => UpdateViewMode(ViewMode.Full, unit.ViewRange, newLocation.Value);
//            }
        }

        /// <summary>
        /// set everything in line of sight of units and buldings in the faction on the viewmode of your input
        /// </summary>
        /// <param name="mode"></param>
        public void UpdateViewModes(ViewMode mode)
        {
            // line of sight units
            foreach (UnitController unit in faction.FactionModel.Units)
            {
            }

            // lino of sight buildings
            foreach (IStructure<IStructureDef> building in faction.FactionModel.Buildings)
            {
                UpdateViewMode(mode, building.ViewRange, building.Centre);
            }

            // set units in line of sight to full view
            UpdateUnits();
        }


        /// <summary>
        /// sets everything within viewrange from the coords to the specified viewmode
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="viewrange"></param>
        /// <param name="coords"></param>
        public void UpdateViewMode(ViewMode mode, int viewrange, FloatCoords coords)
        {
            // loop from -viewrange to + viewrange
            for (int x = (viewrange) * -1; x <= viewrange; x++)
            {
                for (int y = (viewrange) * -1; y <= viewrange; y++)
                {
                    // set coords relative to the given coords
                    Coords tempcoords = (Coords) new FloatCoords {x = x + coords.x, y = y + coords.y};
                    // check if the coords are within viewrange
                    if (!(DistanceCalculator.DiagonalDistance((FloatCoords) tempcoords, coords) < viewrange)) continue;
                    // get the cell from the tempcoords
                    WorldCellController cellController = worldController.GetCellFromCoords(tempcoords);
                    // check if the cellcontroller exists
                    if (cellController == null) continue;
                    // set the viewmode
                    cellController.ChangeViewMode(mode);
                    // check if there is a building in the cell
                    if (cellController.worldCellModel.BuildingOnTop == null) continue;
                    // set viewmode of the building on the cell
                    if (cellController.worldCellModel.BuildingOnTop.GetType() == typeof(BuildingController))
                    {
                        ((BuildingController) cellController.worldCellModel.BuildingOnTop).BuildingView.ViewMode = mode;
                    }

                    // set viewmode of the ConstructingBuilding on the cell
                    if (cellController.worldCellModel.BuildingOnTop.GetType() == typeof(ConstructingBuildingController))
                    {
                        ((ConstructingBuildingController) cellController.worldCellModel.BuildingOnTop).ConstructingBuildingView.ViewMode = mode;
                    }
                }
            }
        }

        /// <summary>
        /// check for all units if they are on a visible cell and change viewmode acordingly
        /// </summary>
        public void UpdateUnits()
        {
            foreach (UnitController unit in worldController.WorldModel.Units)
            {
                if (worldController.GetCellFromCoords((Coords) unit.FloatCoords).worldCellView.ViewMode == ViewMode.Full)
                {
                    unit.UnitView.ViewMode = ViewMode.Full;
                }
                else
                {
                    unit.UnitView.ViewMode = ViewMode.None;
                }
            }
        }

        /// <summary>
        /// Makes everything visible on screen
        /// </summary>
        public void UpdateEverythingVisible()
        {
            foreach (UnitController unit in worldController.WorldModel.Units)
            {
                unit.UnitView.ViewMode = ViewMode.Full;
            }

            foreach (KeyValuePair<Coords, WorldChunkController> grid in worldController.WorldModel.ChunkGrid)
            {
                foreach (var cell in grid.Value.WorldChunkModel.grid)
                {
                    cell.worldCellModel.ViewMode = ViewMode.Full;
                }
            }
        }

        public void Update(object sender, OnTickEventArgs eventArgs)
        {
            UpdateViewModes(ViewMode.Fog);
            UpdateViewModes(ViewMode.Full);
        }
    }
}