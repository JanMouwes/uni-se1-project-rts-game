using kbs2.Faction.FactionMVC;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Chunk;
using kbs2.WorldEntity.Building;

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

		public void LoadChunk(Coords coords) => WorldModel.ChunkGrid[coords].Load();

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
	}
}