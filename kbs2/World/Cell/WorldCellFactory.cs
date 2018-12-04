using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.World.Cell
{
    class WorldCellFactory
    {
        public static WorldCellController GetNewCell(FloatCoords coords, string texture)
        {
            return new WorldCellController(coords, texture);
        }
    }
}
