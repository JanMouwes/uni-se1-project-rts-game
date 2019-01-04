using System;

namespace kbs2.GamePackage.DayCycle
{
    public struct IngameTime
    {
        public int CurrentDay;
        public int CurrentHour;
        public int CurrentMinute;

        public override string ToString() => $"Day: {CurrentDay}, Hour: {CurrentHour}, Min: {CurrentMinute}";
    }
}