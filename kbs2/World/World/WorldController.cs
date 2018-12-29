using kbs2.Faction.FactionMVC;
using kbs2.utils;
using kbs2.World;
using kbs2.World.World;
using kbs2.World.Chunk;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using System.Collections.Generic;
using kbs2.World.Enums;
using kbs2.World.Cell;
using kbs2.World.Structs;

namespace kbs2.Desktop.World.World
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

        public void AddBuilding(BuildingDef defenition, Building_Controller building)
        {
            
            foreach(Coords coords in defenition.BuildingShape)
            {
                Coords actual = coords + building.Model.TopLeft;
                // calc coordinates of chunk the cell is in
                Coords chunkcoords = new Coords
                {
                    x = actual.x / WorldChunkModel.ChunkSize,
                    y = actual.y / WorldChunkModel.ChunkSize
                };
                // calc location of cell within chunk
                Coords relativecoords = new Coords
                {
                    x = ModulusUtils.mod( actual.x , WorldChunkModel.ChunkSize),
                    y = ModulusUtils.mod( actual.y , WorldChunkModel.ChunkSize)
                };
                // add building to the cells its on
                WorldModel.ChunkGrid[chunkcoords].WorldChunkModel.grid[relativecoords.x, relativecoords.y].worldCellModel.BuildingOnTop = building;
                // add cells to the building
                building.Model.LocationCells.Add(WorldModel.ChunkGrid[chunkcoords].WorldChunkModel.grid[relativecoords.x, relativecoords.y].worldCellModel);
            }
            // add building to buildinglist
            WorldModel.buildings.Add(building);
        }

        public void RemoveBuilding(Building_Controller building)
        {
            WorldModel.buildings.Remove(building);
        }

        public void RemoveBUC(BUCController building)
        {
            WorldModel.UnderConstruction.Remove(building);
        }

        public void AddBuildingUnderCunstruction(BuildingDef defenition, BUCController building)
        {

            foreach (Coords coords in defenition.BuildingShape)
            {
                Coords actual = coords + building.BUCModel.TopLeft;
                // calc coordinates of chunk the cell is in
                Coords chunkcoords = new Coords
                {
                    x = actual.x / WorldChunkModel.ChunkSize,
                    y = actual.y / WorldChunkModel.ChunkSize
                };
                // calc location of cell within chunk
                Coords relativecoords = new Coords
                {
                    x = ModulusUtils.mod(actual.x, WorldChunkModel.ChunkSize),
                    y = ModulusUtils.mod(actual.y, WorldChunkModel.ChunkSize)
                };
                // add building to the cells its on
                WorldModel.ChunkGrid[chunkcoords].WorldChunkModel.grid[relativecoords.x, relativecoords.y].worldCellModel.BuildingOnTop = building;
                // add cells to the building
                building.BUCModel.LocationCells.Add(WorldModel.ChunkGrid[chunkcoords].WorldChunkModel.grid[relativecoords.x, relativecoords.y].worldCellModel);
            }
            // add building to buildinglist
            WorldModel.UnderConstruction.Add(building);
        }

        public WorldCellController GetCellFromCoords(Coords coords)
        {
            Coords chunkCoords = WorldPositionCalculator.ChunkCoordsOfCellCoords((FloatCoords)coords);
            return WorldModel.ChunkGrid.ContainsKey(chunkCoords) ? WorldModel.ChunkGrid[chunkCoords].WorldChunkModel.grid[ModulusUtils.mod(coords.x,20), ModulusUtils.mod(coords.y,20)] : null;
        }


        // check if coordsrange contains building or illigal terain
        public bool checkTerainCells(List<Coords> coords, List<TerrainType> whiteList)
        {
            foreach(Coords coord in coords)
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