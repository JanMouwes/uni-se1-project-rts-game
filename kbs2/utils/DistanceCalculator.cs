using kbs2.World;
using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.utils
{
    

    public static class DistanceCalculator
    {
        public static Func<double, double, double> pythagoras = (x, y) => Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        public static Func<double, double, double> getDistance = (x, y) => x > y ? x - y : y - x;

        public static Func<FloatCoords, FloatCoords, double> getDistance2d = (a, b) => pythagoras(getDistance(a.x, b.x), getDistance(a.y, b.y));
    }
}
