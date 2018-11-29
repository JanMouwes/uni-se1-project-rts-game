using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.World.Structs
{
    public struct FloatCoords
    {
        public float x;
        public float y;

        public static FloatCoords operator +(FloatCoords value1, FloatCoords value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }

        public static FloatCoords operator +(FloatCoords value1, Coords value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }

        public static FloatCoords operator -(FloatCoords value1, FloatCoords value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }

        public static FloatCoords operator -(FloatCoords value1, Coords value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }

        public static bool operator ==(FloatCoords value1, FloatCoords value2) =>
            (value1.x == value2.x && value1.y == value2.y);


        public static bool operator !=(FloatCoords value1, FloatCoords value2) =>
            !(value1.x == value2.x && value1.y == value2.y);
    }
}