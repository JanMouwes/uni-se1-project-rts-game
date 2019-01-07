namespace kbs2.WorldEntity.Structs
{
    public struct CellWeightValues
    {
        public double AbsoluteDistanceToTarget;
        public double AbsoluteDistanceToUnit;
        public double DistanceToUnit;
        public double Weight => AbsoluteDistanceToTarget + DistanceToUnit;

        public static bool operator >(CellWeightValues firstValues, CellWeightValues secondValues) => firstValues.Weight > secondValues.Weight;

        public static bool operator <(CellWeightValues firstValues, CellWeightValues secondValues) => firstValues.Weight < secondValues.Weight;
        
        public static bool operator >=(CellWeightValues firstValues, CellWeightValues secondValues) => firstValues.Weight >= secondValues.Weight;

        public static bool operator <=(CellWeightValues firstValues, CellWeightValues secondValues) => firstValues.Weight <= secondValues.Weight;
    }
}