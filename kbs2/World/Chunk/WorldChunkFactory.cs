using System;
using kbs2.World.Cell;

namespace kbs2.World.Chunk
{
    public static class WorldChunkFactory
    {
        public static WorldChunkController ChunkOfTerrainType(Coords chunkCoords, TerrainType terrainType)
        {
            WorldChunkController controller = new WorldChunkController(chunkCoords);

            foreach (WorldCellModel worldCellModel in controller.WorldChunkModel.grid)
            {
                worldCellModel.BaseTerrain = worldCellModel.Terrain = terrainType;
                worldCellModel.ParentChunk = controller.WorldChunkModel;
            }

            return controller;
        }
    }
}