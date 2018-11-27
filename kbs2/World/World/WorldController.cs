using kbs2.World;

namespace kbs2.Desktop.World.World
{
    public class WorldController
    {
        private WorldModel worldModel;

        public void Load( /*Faction faction*/)
        {
            //TODO
        }

        public void Unload()
        {
            //TODO
        }

		public void LoadChunk(Coords coords) => worldModel.ChunkGrid[coords].Load();

		public void UnloadChunk(Coords coords) => worldModel.ChunkGrid[coords].UnLoad();


		/* outdated chunkloading
        public void LoadChunk(Coords coords) => worldModel.ChunkGrid[coords.x, coords.y].Load();

        public void UnloadChunk(Coords coords) => worldModel.ChunkGrid[coords.x, coords.y].UnLoad();
		*/
	}
}