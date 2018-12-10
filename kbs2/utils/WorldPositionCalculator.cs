using System;
using kbs2.World;
using kbs2.World.Chunk;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.utils
{
    public static class WorldPositionCalculator
    {
        //    Transforms window-coords in pixels to map-coords in pixels (draw-coords)
        public static Coords TransformWindowCoords(Coords windowCoordsInPixels, Matrix matrix)
        {
            Vector2 transformed = Vector2.Transform(new Vector2(windowCoordsInPixels.x, windowCoordsInPixels.y), Matrix.Invert(matrix));

            return new Coords
            {
                x = (int) transformed.X,
                y = (int) transformed.Y
            };
        }

        //    Calculates cell-coords from draw-coords.
        public static Coords DrawCoordsToCellCoords(Coords drawCoords, int tileSize) => new Coords
        {
            x = drawCoords.x / tileSize,
            y = drawCoords.y / tileSize
        };

        //    Calculates chunk-coords from cell-coords  
        public static Coords ChunkCoordsOfCellCoords(FloatCoords cellCoords) => new Coords
        {
            x = (int) Math.Floor((double) cellCoords.x / WorldChunkModel.ChunkSize),
            y = (int) Math.Floor((double) cellCoords.y / WorldChunkModel.ChunkSize)
        };
    }
}