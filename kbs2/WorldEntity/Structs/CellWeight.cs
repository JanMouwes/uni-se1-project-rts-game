using System;
namespace kbs2.WorldEntity.Structs
{
    public struct CellWeight
    {
        public double AbsoluteDistanceToTarget;
        public double AbsoluteDistanceToUnit;
        public double DistanceToUnit;
        public double Weight => AbsoluteDistanceToTarget + DistanceToUnit;
    }
}
