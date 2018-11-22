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

            for(int x = -2; x < 2; x++)
            {
                for (int y = -2; y < 2; y++)
                {
                    Coords coords = new Coords { x = x, y = y };
                    WorldChunkController chunkController = WorldChunkFactory.ChunkOfTerrainType(coords, TerrainType.Sand);
                    world.worldModel.ChunkGrid.Add(coords, chunkController);
                }
            }

            return world;
        }

        public static WorldController GetNewEmptyWorld()
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
