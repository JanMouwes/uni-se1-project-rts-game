using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.World;
using kbs2.World.Structs;

namespace kbs2.utils
{
    public class CoordsConsoleUtil
    {
        public static void EchoCoordsList(IEnumerable<Coords> coords)
        {
            EchoCoordsList(coords.Select(point => (FloatCoords) point).ToList());
        }

        public static void EchoCoordsList(IEnumerable<FloatCoords> coords)
        {
            Console.WriteLine("<coords>");
            foreach (Coords coordOnRay in coords)
            {
                Console.WriteLine(coordOnRay);
            }

            Console.WriteLine("</coords>");
        }
    }
}