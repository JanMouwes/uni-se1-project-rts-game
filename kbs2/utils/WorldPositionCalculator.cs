using kbs2.World;
using Microsoft.Xna.Framework;

namespace kbs2.utils
{
    public static class WorldPositionCalculator
    {
        public static Coords TransformWindowCoords(Coords position, Matrix matrix)
        {
            Vector2 transformed = Vector2.Transform(new Vector2(position.x, position.y), Matrix.Invert(matrix));

            return new Coords
            {
                x = (int) transformed.X,
                y = (int) transformed.Y
            };
        }

        public static Coords DrawCoordsToCellCoords(Coords drawCoords, int tileSize) => new Coords
        {
            x = drawCoords.x / tileSize,
            y = drawCoords.y / tileSize
        };
    }
}