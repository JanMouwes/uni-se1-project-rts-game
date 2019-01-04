using System;
using System.Collections.Generic;
using kbs2.utils;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.Enums;
using kbs2.World.Structs;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.BuildingMVC;
using kbs2.WorldEntity.Structures.BuildingUnderConstructionMVC;

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

        public void AddStructure(IStructure<IStructureDef> building)
        {
            foreach (Coords coords in building.Def.BuildingShape)
            {
                Coords actual = coords + building.StartCoords;

                WorldCellController cell = GetCellFromCoords(actual);

                // add building to the cells its on
                cell.worldCellModel.BuildingOnTop = building;
                // add cells to the building
                building.OccupiedCells.Add(cell.worldCellModel);
            }

            // add building to buildinglist
            WorldModel.Structures.Add(building);
        }

        public void RemoveStructure(IStructure<IStructureDef> structure)
        {
            foreach (WorldCellModel occupiedCell in structure.OccupiedCells)
            {
                occupiedCell.BuildingOnTop = null;
            }

            WorldModel.Structures.Remove(structure);
        }

        public WorldCellController GetCellFromCoords(Coords coords)
        {
            Coords chunkCoords = WorldPositionCalculator.ChunkCoordsOfCellCoords((FloatCoords) coords);
            Coords relativeChunkCoords = WorldPositionCalculator.RelativeChunkCoords(coords);
            return WorldModel.ChunkGrid.ContainsKey(chunkCoords)
                ? WorldModel.ChunkGrid[chunkCoords].WorldChunkModel.grid[relativeChunkCoords.x, relativeChunkCoords.y]
                : null;

            //FIXME throw new CellNotFoundException();
        }


        // check if coords-range contains building or illegal terrain
        public bool AreTerrainCellsLegal(IEnumerable<Coords> coordsList, List<TerrainType> whiteList)
        {
            foreach (Coords coords in coordsList)
            {
                WorldCellModel cell = GetCellFromCoords(coords).worldCellModel;
                if (cell.BuildingOnTop != null)
                {
                    return false;
                }

                if (!whiteList.Contains(cell.Terrain))
                {
                    return false;
                }
            }

            return true;
        }
    }
}