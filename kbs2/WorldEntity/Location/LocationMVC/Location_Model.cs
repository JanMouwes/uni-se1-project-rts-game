using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.WorldEntity.Location
{
	public class Location_Model
	{
        public Unit_Controller parrent;
		public Coords coords;
		public FloatCoords floatCoords;
        public List<TerrainType> UnwalkableTerrain;

        public Location_Model(float locationX, float locationY)
        {
            floatCoords.x = locationX;
            floatCoords.y = locationY;
        }
    }
}
