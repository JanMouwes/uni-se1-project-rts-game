using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.World;
using kbs2.World.Enums;
using kbs2.World.Structs;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.WorldEntity.Location
{
	public class LocationModel
	{
        public UnitController Parent;
		public Coords Coords => (Coords)FloatCoords;
		public FloatCoords FloatCoords;
        public List<TerrainType> UnwalkableTerrain;

        public LocationModel(float locationX, float locationY)
        {
            FloatCoords.x = locationX;
            FloatCoords.y = locationY;

            UnwalkableTerrain = new List<TerrainType>();
        }
    }
}
