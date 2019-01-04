using System;
using System.Collections.Generic;
using kbs2.World;

namespace kbs2.utils
{
    public class BuildingShapeCalculator
    {
        public static List<Coords> GetShapeFromString(string shapeString)
        {
            List<Coords> shape = new List<Coords>();
            char[] array = shapeString.ToCharArray();

            Coords coords = new Coords
            {
                x = 0,
                y = 0
            };

            foreach (char c in array)
            {
                switch (c)
                {
                    case 'x':
                    case 'o':
                        if (c == 'x') shape.Add(coords);
                        coords.x++;
                        break;
                    case ';':
                        coords.x = 0;
                        coords.y++;
                        break;
                    default:
                        throw new ArgumentException($"Invalid char '{c}'"); //NOTE temp solution
                        break;
                }
            }

            return shape;
        }
    }
}