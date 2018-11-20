using System;
namespace kbs2.World
{
    public struct Coords
    {
		public int x;
		public int y;


        public static Coords operator +(Coords value1,Coords value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }
    }
}
