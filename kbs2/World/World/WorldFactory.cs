using kbs2.Desktop.World.World;
using kbs2.World.Chunk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.World.World
{
    public static class WorldFactory
    {
        public static WorldController GetNewWorld()
        {
            WorldController world = new WorldController();
            world.worldModel = new WorldModel
            {
                ChunkGrid = new Dictionary<Coords, WorldChunkController>()
            };

            return world;
        }
    }
}
