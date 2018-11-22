using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.World.Cell;

namespace kbs2.World.Chunk
{
    public class WorldChunkModel
    {
        public const int ChunkSize = 20;

        public WorldCellModel[,] grid;
        public Coords coords;

        public WorldChunkModel(Coords coords)
        {
            this.grid = new WorldCellModel[ChunkSize, ChunkSize];

            int relXIndex = 0;
            int relYIndex = 0;

            for (int i = 0; i < Math.Pow(ChunkSize, 2); i++)
            {
                //    Calculate chunk-relative X and Y (between 0 and ChunkSize)
                relXIndex = (int) i % ChunkSize;
                relYIndex = (int) Math.Floor((double) (i / ChunkSize));

                WorldCellModel worldCellModel = new WorldCellModel
                {
                    RealCoords = new Coords()
                    {
                        x = coords.x * ChunkSize + relXIndex,
                        y = coords.y * ChunkSize + relYIndex
                    }
                };
                grid[relXIndex, relYIndex] = worldCellModel;
            }

            this.coords = coords;
        }
    }
}