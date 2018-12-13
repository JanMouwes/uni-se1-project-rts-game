using System;
using kbs2.World.Cell;
using kbs2.World.Enums;

namespace kbs2.World.Chunk
{
    public static class WorldChunkFactory
    {
        public delegate WorldChunkController ChunkGenerationDelegate(Coords chunkCoords);

        public static WorldChunkController ChunkOfDefaultTerrain(Coords chunkCoords)
        {
            WorldChunkController controller = new WorldChunkController(chunkCoords);
            controller.WorldChunkModel.grid =
                new WorldCellController[WorldChunkModel.ChunkSize, WorldChunkModel.ChunkSize];

            for (int i = 0; i < Math.Pow(WorldChunkModel.ChunkSize, 2); i++)
            {
                //    Calculate chunk-relative X and Y (between 0 and ChunkSize)
                int relXIndex = (int) i % WorldChunkModel.ChunkSize;
                int relYIndex = (int) Math.Floor(i / (double) WorldChunkModel.ChunkSize);

                WorldCellController cellController = WorldCellFactory.GetNewCell(
                    new Structs.FloatCoords
                    {
                        x = chunkCoords.x * WorldChunkModel.ChunkSize + relXIndex,
                        y = chunkCoords.y * WorldChunkModel.ChunkSize + relYIndex
                    });
                cellController.worldCellModel.ParentChunk = controller.WorldChunkModel;
                controller.WorldChunkModel.grid[relXIndex, relYIndex] = cellController;
            }

            return controller;
        }

    }
}