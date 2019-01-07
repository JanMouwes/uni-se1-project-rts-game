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
        public static FloatCoords TransformWindowCoords(Coords windowCoordsInPixels, Matrix matrix)
        {
            Vector2 transformed = Vector2.Transform(new Vector2(windowCoordsInPixels.x, windowCoordsInPixels.y),
                Matrix.Invert(matrix));

            return new FloatCoords()
            {
                x = transformed.X,
                y = transformed.Y
            };
        }

        /// <summary>
        /// Transforms coordinates on the window to map-coordinates
        /// </summary>
        /// <param name="windowCoords">Coords on the window</param>
        /// <param name="viewMatrix">Transform-matrix</param>
        /// <param name="tileSize">Size of the map-tiles</param>
        /// <returns></returns>
        public static FloatCoords WindowCoordsToCellCoords(Coords windowCoords, Matrix viewMatrix, int tileSize)
        {
            return DrawCoordsToCellFloatCoords(TransformWindowCoords(windowCoords, viewMatrix), tileSize);
        }
        
        //    Calculates cell-coords from draw-coords.
        public static Coords DrawCoordsToCellCoords(Coords drawCoords, int tileSize) =>
            (Coords) DrawCoordsToCellFloatCoords((FloatCoords) drawCoords, tileSize);

        /// <summary>
        /// Calculates chunk-coords from cell-coords
        /// </summary>
        /// <param name="cellCoords">Cell-coords relative to center of the World</param>
        /// <returns>Chunk-coords relative to center of the World</returns>
        public static Coords ChunkCoordsOfCellCoords(FloatCoords cellCoords) => new Coords
        {
            x = (int) Math.Floor((double) cellCoords.x / WorldChunkModel.ChunkSize),
            y = (int) Math.Floor((double) cellCoords.y / WorldChunkModel.ChunkSize)
        };

        /// <summary>
        /// Translates coords relative to center of the map
        /// to coords relative only to the left-top of any chunk
        /// </summary>
        /// <param name="realCoords">Coords relative to center of the World</param>
        /// <returns>Coords relative to left-top of a WorldChunk</returns>
        public static Coords RelativeChunkCoords(Coords realCoords) => new Coords
        {
            x = ModulusUtils.mod(realCoords.x, WorldChunkModel.ChunkSize),
            y = ModulusUtils.mod(realCoords.y, WorldChunkModel.ChunkSize)
        };

        public static FloatCoords DrawCoordsToCellFloatCoords(FloatCoords drawCoords, int tileSize) => new FloatCoords
        {
            x = drawCoords.x / tileSize - (drawCoords.x < 0 ? 1 : 0),
            y = drawCoords.y / tileSize - (drawCoords.y < 0 ? 1 : 0)
        };
    }
}