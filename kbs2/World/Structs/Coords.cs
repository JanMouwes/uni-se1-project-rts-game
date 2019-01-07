using kbs2.World.Structs;

namespace kbs2.World
{
    public struct Coords
    {
        public int x;
        public int y;

        public static explicit operator FloatCoords(Coords value1)
        {
            FloatCoords value2;
            value2.x = value1.x;
            value2.y = value1.y;
            return value2;
        }

        public static explicit operator Coords(FloatCoords value1)
        {
            Coords value2;
            value2.x = (int) value1.x;
            value2.y = (int) value1.y;
            return value2;
        }

        public static Coords operator +(Coords value1, Coords value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }

        public static Coords operator +(Coords value1, FloatCoords value2)
        {
            value1.x += (int) value2.x;
            value1.y += (int) value2.y;
            return value1;
        }

        public static Coords operator -(Coords value1, Coords value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }

        public static Coords operator -(Coords value1, FloatCoords value2)
        {
            value1.x -= (int) value2.x;
            value1.y -= (int) value2.y;
            return value1;
        }

        public static bool operator ==(Coords value1, Coords value2) => value1.x == value2.x && value1.y == value2.y;


        public static bool operator !=(Coords value1, Coords value2) => !(value1.x == value2.x && value1.y == value2.y);


        public override string ToString() => $"X: {x}, Y: {y}";
    }
}