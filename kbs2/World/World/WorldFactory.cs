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
            // Initialize World 
            WorldController world = new WorldController();

            // Initialize WorldModel and ChunkGrid in World so you can add chunks to this grid
            world.WorldModel = new WorldModel
            {
                ChunkGrid = new Dictionary<Coords, WorldChunkController>()
            };

            // For loop to add chunks from -2 to +2 in both x and y directions. this makes for a 5x5 chunkmap.
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    // Sets the coords for the new chunk
                    Coords coords = new Coords {x = x, y = y};

                    // Initializes a new chunk with the set coords and a basic terrain type
                    WorldChunkController chunkController = WorldChunkFactory.ChunkOfDefaultTerrain(coords);

                    // Adds the new chunk to the worldgrid
                    world.WorldModel.ChunkGrid.Add(coords, chunkController);
                }
            }

            // Returns the worldController with a 5x5 chunkGrid initialized
            return world;
        }

        public static WorldController GetNewEmptyWorld()
        {
            // Initialize World 
            WorldController world = new WorldController();

            // Initialize WorldModel and ChunkGrid in World so you can add chunks to this grid
            world.WorldModel = new WorldModel
            {
                ChunkGrid = new Dictionary<Coords, WorldChunkController>()
            };

            // Returns the initialized worldController without chunks loaded
            return world;
        }
    }
}