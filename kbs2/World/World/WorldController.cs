using System.Collections.Generic;
using kbs2.utils;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.Enums;
using kbs2.World.Structs;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingMVC;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.World.World
{
    public class WorldController
    {
        public WorldModel WorldModel { get; set; }

        public void Load( /*Faction faction*/)
        {
            //TODO
        }

        public void Unload()
        {
            //TODO
        }

        //    Loads chunk at given coordinate
        public void LoadChunk(Coords coords) => WorldModel.ChunkGrid[coords].Load();

        //    Unloads chunk at given coordinate
        public void UnloadChunk(Coords coords) => WorldModel.ChunkGrid[coords].UnLoad();

        public void AddBuilding(IStructure building)
        {
            foreach (Coords coords in building.Def.BuildingShape)
            {
                Coords actual = coords + building.StartCoords;
                // calc coordinates of chunk the cell is in
                Coords chunkcoords = new Coords
                {
                    x = actual.x / WorldChunkModel.ChunkSize,
                    y = actual.y / WorldChunkModel.ChunkSize
                };
                // calc location of cell within chunk
                Coords relativeCoords = new Coords
                {
                    x = ModulusUtils.mod(actual.x, WorldChunkModel.ChunkSize),
                    y = ModulusUtils.mod(actual.y, WorldChunkModel.ChunkSize)
                };
                // add building to the cells its on
                WorldModel.ChunkGrid[chunkcoords].WorldChunkModel.grid[relativeCoords.x, relativeCoords.y]
                    .worldCellModel.BuildingOnTop = building;
                // add cells to the building
                building.OccupiedCells.Add(WorldModel.ChunkGrid[chunkcoords].WorldChunkModel
                    .grid[relativeCoords.x, relativeCoords.y].worldCellModel);
            }

            // add building to buildinglist
            WorldModel.Structures.Add(building);
        }

        public void RemoveStructure(IStructure structure)
        {
            switch (structure)
            {
                case BuildingController controller:
                    RemoveBuilding(controller);
                    break;
                case ConstructingBuildingController buildingController:
                    RemoveBUC(buildingController);
                    break;
            }
        }

        private void RemoveBuilding(IStructure building)
        {
            WorldModel.Structures.Remove(building);
        }

        private void RemoveBUC(ConstructingBuildingController building)
        {
            WorldModel.UnderConstruction.Remove(building);
        }

        public void AddBuildingUnderConstruction(ConstructingBuildingDef def, ConstructingBuildingController building)
        {
            foreach (Coords coords in def.BuildingShape)
            {
                Coords actual = coords + building.ConstructingBuildingModel.StartCoords;
                // calc coordinates of chunk the cell is in
                Coords chunkCoords = WorldPositionCalculator.ChunkCoordsOfCellCoords((FloatCoords) actual);

                // calc location of cell within chunk
                Coords relativeCoords = new Coords
                {
                    x = ModulusUtils.mod(actual.x, WorldChunkModel.ChunkSize),
                    y = ModulusUtils.mod(actual.y, WorldChunkModel.ChunkSize)
                };

                WorldCellModel worldCell = WorldModel.ChunkGrid[chunkCoords].WorldChunkModel
                    .grid[relativeCoords.x, relativeCoords.y].worldCellModel;

                // add building to the cells its on
                worldCell.BuildingOnTop = building;

                // add cells to the building
                building.ConstructingBuildingModel.LocationCells.Add(worldCell);
            }

            // add building to buildinglist
            WorldModel.UnderConstruction.Add(building);
        }

        public WorldCellController GetCellFromCoords(Coords coords)
        {
            Coords chunkCoords = WorldPositionCalculator.ChunkCoordsOfCellCoords((FloatCoords) coords);
            if (WorldModel.ChunkGrid.ContainsKey(chunkCoords))
            {
                return WorldModel.ChunkGrid[chunkCoords].WorldChunkModel
                    .grid[ModulusUtils.mod(coords.x, 20), ModulusUtils.mod(coords.y, 20)];
            }

            return null;
        }


        // check if coords-range contains building or illegal terrain
        public bool AreTerrainCellsLegal(List<Coords> coords, List<TerrainType> whiteList)
        {
            foreach (Coords coord in coords)
            {
                WorldCellModel cell = GetCellFromCoords(coord).worldCellModel;
                if (cell.BuildingOnTop != null)
                {
                    return false;
                }

                if (!(whiteList.Contains(cell.Terrain)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}