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
            controller.WorldChunkModel.grid = new WorldCellController[WorldChunkModel.ChunkSize, WorldChunkModel.ChunkSize];

            for (int i = 0; i < Math.Pow(WorldChunkModel.ChunkSize, 2); i++)
            {
                //    Calculate chunk-relative X and Y (between 0 and ChunkSize)
                int relXIndex = (int)i % WorldChunkModel.ChunkSize;
                int relYIndex = (int)Math.Floor(i / (double)WorldChunkModel.ChunkSize);

                controller.WorldChunkModel.grid[relXIndex, relYIndex] = WorldCellFactory.GetNewCell(new Structs.FloatCoords { x = chunkCoords.x * WorldChunkModel.ChunkSize + relXIndex, y = chunkCoords.y * WorldChunkModel.ChunkSize + relYIndex }, TerrainDef.TerrainDef.TerrainDictionairy[terrainType]);
            }

            //int xIndex = 0, yIndex = 0; 
            foreach (WorldCellModel worldCellModel in controller.WorldChunkModel.grid)
            {
                worldCellModel.BaseTerrain = worldCellModel.Terrain = terrainType;
                worldCellModel.ParentChunk = controller.WorldChunkModel;
            }

            return controller;
        }
    }
}