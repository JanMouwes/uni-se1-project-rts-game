using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.World.Chunk
{
    public class WorldChunkController
    {
        public WorldChunkModel WorldChunkModel { get; set; }


        public void Load()
        {
        }

        public void UnLoad()
        {
        }

        private void LoadFromFile(string fileName)
        {
        }

        public WorldChunkController(Coords coords)
        {
            this.WorldChunkModel = new WorldChunkModel(coords);
        }
    }
}