using kbs2.World;

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
	}
}