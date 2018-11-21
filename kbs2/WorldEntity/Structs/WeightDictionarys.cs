using System;
using System.Collections.Generic;
using kbs2.World;

namespace kbs2.WorldEntity.Structs
{
    public struct WeightDictionarys
    {
        public WeightDictionarys(bool shit)// bool does nothing but is required
        {
            CellsWithWeight = new Dictionary<Coords, CellWeight>();
            BorderCellsWithWeight = new Dictionary<Coords, CellWeight>();
            ObstacleList = new List<Coords>();
        }

        public Dictionary<Coords, CellWeight> CellsWithWeight;
        public Dictionary<Coords, CellWeight> BorderCellsWithWeight;
        public List<Coords> ObstacleList;
    }
}
