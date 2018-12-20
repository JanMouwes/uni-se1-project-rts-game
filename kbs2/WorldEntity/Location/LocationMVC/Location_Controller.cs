using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.World;
using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.World.World;

namespace kbs2.WorldEntity.Location
{
    public class Location_Controller
    {
        public Pathfinder pathfinder;
        public Location_Model LocationModel;
        public List<FloatCoords> Waypoints;
        public WorldController worldController;

        


        static Func<double, double, double> pythagoras = (x, y) => Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        static Func<double, double, double> getDistance = (x, y) => x > y ? x - y : y - x;

        Func<FloatCoords, FloatCoords, double> getDistance2d = (a, b) =>
            pythagoras(getDistance(a.x, b.x), getDistance(a.y, b.y));

        public Location_Controller(WorldController worldController, float lx, float ly)
        {
            LocationModel = new Location_Model(lx, ly);
            pathfinder = new Pathfinder(worldController.WorldModel, 500);
            Waypoints = new List<FloatCoords>();
            this.worldController = worldController;
        }

        public void MoveTo(FloatCoords target, bool isQueueKeyPressed) //[Review] This can be a Lambda expression
        {
            List<FloatCoords> points = pathfinder.FindPath(target, LocationModel);
            points.RemoveAt(0);
            if (isQueueKeyPressed)
            {
                Waypoints.AddRange(points);
            }
            else
            {
                Waypoints = points;
            }
        }

        

        public void Ontick(object sender, OnTickEventArgs eventArgs)
        {
            if (Waypoints.Count > 0)
            {
                float speed = LocationModel.parent.UnitModel.Speed;

                if (getDistance2d(Waypoints[0], LocationModel.floatCoords) < speed)
                {
                    LocationModel.floatCoords = Waypoints[0];
                    Waypoints.RemoveAt(0);
                }
                else
                {
                    float xdifference = (float) getDistance(LocationModel.floatCoords.x, Waypoints[0].x);
                    float ydifference = (float) getDistance(LocationModel.floatCoords.y, Waypoints[0].y);
                    // calculate new coords
                    float diagonaldifference = (float) pythagoras(xdifference, ydifference);
                    float v = diagonaldifference / speed;

                    FloatCoords difference = new FloatCoords();
                    difference.x = xdifference / v;
                    difference.y = ydifference / v;

                    if (Waypoints[0].x < LocationModel.floatCoords.x)
                    {
                        LocationModel.floatCoords.x -= difference.x;
                    }
                    else
                    {
                        LocationModel.floatCoords.x += difference.x;
                    }

                    if (Waypoints[0].y < LocationModel.floatCoords.y)
                    {
                        LocationModel.floatCoords.y -= difference.y;
                    }
                    else
                    {
                        LocationModel.floatCoords.y += difference.y;
                    }
                }
            }
        }
    }
}