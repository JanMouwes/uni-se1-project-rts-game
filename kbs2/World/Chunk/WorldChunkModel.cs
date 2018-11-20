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

            this.coords = coords;
        }
    }
}