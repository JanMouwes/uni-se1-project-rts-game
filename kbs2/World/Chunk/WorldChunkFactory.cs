using System;
using kbs2.World.Cell;

namespace kbs2.World.Chunk
{
    public static class WorldChunkFactory
    {
        public static WorldChunkController ChunkOfTerrainType(Coords chunkCoords, TerrainType terrainType)
        {
            Func<int, int, int> rowIndex = (d2Index, rowSize) => d2Index % rowSize;
            Func<int, int, int> colIndex = (d2Index, rowSize) => (int) Math.Floor((double) (d2Index / rowSize));

            WorldChunkController controller = new WorldChunkController(chunkCoords);

            int xIndex = 0, yIndex = 0;
            foreach (WorldCellModel worldCellModel in controller.worldChunkModel.grid)
            {
                worldCellModel.BaseTerrain = terrainType;
                worldCellModel.Terrain = terrainType;

                worldCellModel.RealCoords = new Coords()
                {
                    x = chunkCoords.x * WorldChunkModel.ChunkSize + xIndex,
                    y = chunkCoords.y * WorldChunkModel.ChunkSize + yIndex
                };

                //    If x would pass ChunkSize-limit, reset to zero. Else, add one
                //    If x has been reset, increment yIndex by 1.
                xIndex = xIndex == WorldChunkModel.ChunkSize ? 0 : xIndex + 1;
                yIndex = xIndex != 0 ? yIndex : yIndex + 1;
            }

            return controller;
        }
    }
}