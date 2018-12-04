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

        public WorldCellController[,] grid;
        public Coords ChunkCoords;

        public WorldChunkModel(Coords chunkCoords)
        {
            this.grid = new WorldCellController[ChunkSize, ChunkSize];

            //    Generates cells using chunk-relative X and Y to calculate World-relative coordinates for the cells.
            for (int i = 0; i < Math.Pow(ChunkSize, 2); i++)
            {
                //    Calculate chunk-relative X and Y (between 0 and ChunkSize)
                int relXIndex = (int) i % ChunkSize;
                int relYIndex = (int) Math.Floor(i / (double) ChunkSize);

                WorldCellModel worldCellModel = new WorldCellModel
                {
                    RealCoords = new Coords()
                    {
                        x = chunkCoords.x * ChunkSize + relXIndex,
                        y = chunkCoords.y * ChunkSize + relYIndex
                    }
                };
                WorldCellController controller = new WorldCellController {worldCellModel = worldCellModel};
                grid[relXIndex, relYIndex] = controller;
            }

            this.ChunkCoords = chunkCoords;
        }
    }
}