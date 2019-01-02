using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Desktop.World.World;
using kbs2.GamePackage.EventArgs;
using kbs2.utils;
using kbs2.World.Structs;
using kbs2.WorldEntity.Pathfinder;
using kbs2.WorldEntity.Pathfinder.Exceptions;

namespace kbs2.WorldEntity.Location.LocationMVC
{
    public class Location_Controller
    {
        private Thread pathfinderThread;
        public Pathfinder.Pathfinder Pathfinder;
        public Location_Model LocationModel;
        public Queue<FloatCoords> Waypoints = new Queue<FloatCoords>();

        public delegate void MoveCompleteDelegate(object sender, EventArgsWithPayload<FloatCoords> eventArgs);

        public event MoveCompleteDelegate MoveComplete;

        public Location_Controller(WorldController worldModel, float lx, float ly)
        {
            LocationModel = new Location_Model(lx, ly);
            Pathfinder = new Pathfinder.Pathfinder(worldModel);
        }

        //TODO find better name for this method.
        public void MoveTo(FloatCoords target, bool isQueueKeyPressed) //[Review] This can be a Lambda expression
        {
            pathfinderThread?.Abort();

            ThreadStart threadStart = () =>
            {
                List<FloatCoords> points;
                try
                {
                    points = Pathfinder.FindPath(target, LocationModel);
                }
                catch (NoPathFoundException exception)
                {
                    return;
                }

                if (isQueueKeyPressed)
                {
                    points.ForEach(coords => Waypoints.Enqueue(coords));
                }
                else
                {
                    Waypoints = new Queue<FloatCoords>(points);
                }
            };

            pathfinderThread = new Thread(threadStart);
            pathfinderThread.Start();
        }


        public void Update(object sender, OnTickEventArgs eventArgs)
        {
            if (!Waypoints.Any()) return;

            float speed = LocationModel.parent.UnitModel.Speed;

            if (DistanceCalculator.DiagonalDistance(Waypoints.Peek(), LocationModel.floatCoords) < speed)
            {
                // Arrived near destination
                LocationModel.floatCoords = Waypoints.Dequeue();

                if (!Waypoints.Any()) MoveComplete?.Invoke(this, new EventArgsWithPayload<FloatCoords>(LocationModel.floatCoords));

                return;
            }

            float xDifference = (float) DistanceCalculator.CalcDistance(LocationModel.floatCoords.x, Waypoints.Peek().x);
            float yDifference = (float) DistanceCalculator.CalcDistance(LocationModel.floatCoords.y, Waypoints.Peek().y);

            // calculate new coords
            float diagonalDifference = (float) DistanceCalculator.Pythagoras(xDifference, yDifference);
            float v = diagonalDifference / speed;

            FloatCoords difference = new FloatCoords
            {
                x = xDifference / v,
                y = yDifference / v
            };

            difference.x = Waypoints.Peek().x < LocationModel.floatCoords.x ? -difference.x : difference.x;
            LocationModel.floatCoords.x = LocationModel.floatCoords.x + difference.x;

            difference.y = Waypoints.Peek().y < LocationModel.floatCoords.y ? -difference.y : difference.y;
            LocationModel.floatCoords.y = LocationModel.floatCoords.y + difference.y;
        }
    }
}