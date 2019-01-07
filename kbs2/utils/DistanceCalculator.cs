using kbs2.World.Structs;
using System;

namespace kbs2.utils
{
    public static class DistanceCalculator
    {
        /// <summary>
        /// Outputs C, when A^2 + B^2 = C^2
        /// </summary>
        /// <param name="a">A</param>
        /// <param name="b">B</param>
        /// <returns>C</returns>
        public static double Pythagoras(double a, double b) => Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

        /// <summary>
        /// Calculates distance between two points
        /// </summary>
        /// <param name="x">Point 1</param>
        /// <param name="y">Point 2</param>
        /// <returns>Positive value</returns>
        public static double CalcDistance(double x, double y) => x > y ? x - y : y - x;

        /// <summary>
        /// Calculates diagonal distance between two coordinates.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Positive distance</returns>
        public static double DiagonalDistance(FloatCoords a, FloatCoords b) => Pythagoras(CalcDistance(a.x, b.x), CalcDistance(a.y, b.y));
    }
}