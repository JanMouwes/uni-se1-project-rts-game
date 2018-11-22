using kbs2.World;
using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Location
{
	public class Location_Controller
	{
        public Pathfinder pathfinder;
		public Location_Model LocationModel;
        public List<FloatCoords> Waypoints;


        static Func<double, double, double> pythagoras = (x, y) => Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        static Func<double, double, double> getDistance = (x, y) => x > y ? x - y : y - x;
        Func<FloatCoords, FloatCoords, double> getDistance2d = (a, b) => pythagoras(getDistance(a.x, b.x), getDistance(a.y, b.y));

        public Location_Controller()
		{

		}
		public void MoveTo(FloatCoords target)
		{
            Waypoints.AddRange(pathfinder.FindPath(target, LocationModel));
		}
        public void Ontick() //TODO subscribe to ontick event
        {
            if(Waypoints.Count > 0)
            {
                float speed = LocationModel.parrent.UnitModel.UnitDef.Speed;

                if (getDistance2d(Waypoints[0], LocationModel.floatCoords) < speed)
                {
                    LocationModel.floatCoords = Waypoints[0];
                    Waypoints.RemoveAt(0);
                }
                else
                {

                    float xdifference = (float)getDistance(LocationModel.floatCoords.x, Waypoints[0].x);
                    float ydifference = (float)getDistance(LocationModel.floatCoords.y, Waypoints[0].y);

                    float diagonaldifference = (float)pythagoras(xdifference, ydifference);
                    FloatCoords difference = new FloatCoords();
                    difference.x = diagonaldifference / speed * xdifference;
                    difference.x = diagonaldifference / speed * xdifference;
                    
                    LocationModel.floatCoords += difference;
                }
                
                LocationModel.coords = (Coords)LocationModel.floatCoords;
            }
        }
	}
}
