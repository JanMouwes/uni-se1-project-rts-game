using kbs2.Faction.FactionMVC;
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

        public void AddBuilding(BuildingDef defenition, Coords TopLeft, Faction_Controller faction)
        {
            Building_Controller building = BuildingFactory.CreateNewBuilding(defenition, TopLeft);
            faction.AddBuildingToFaction(building);

            foreach(Coords coords in defenition.BuildingShape)
            {
                Coords actual = coords + TopLeft;

                Coords chunkcoords = new Coords
                {
                    x = actual.x / WorldChunkModel.ChunkSize,
                    y = actual.y / WorldChunkModel.ChunkSize
                };
                Coords relativecoords = new Coords
                {
                    x = actual.x % WorldChunkModel.ChunkSize,
                    y = actual.y % WorldChunkModel.ChunkSize
                };
                WorldModel.ChunkGrid[chunkcoords].WorldChunkModel.grid[relativecoords.x, relativecoords.y].BuildingOnTop = building;
            }
        }
	}
}