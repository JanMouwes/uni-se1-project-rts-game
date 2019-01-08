using System.Collections.Generic;
using System.Linq;
using System.Threading;
using kbs2.GamePackage.EventArgs;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Chunk;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Pathfinder.Exceptions;

namespace kbs2.WorldEntity.Location.LocationMVC
{
    public class Location_Controller
    {
        private Thread pathfinderThread;
        public Pathfinder.Pathfinder Pathfinder;
        public LocationModel LocationModel;
        public Queue<FloatCoords> Waypoints = new Queue<FloatCoords>();

        public delegate void EnterNewChunk(object sender, EventArgsWithPayload<Coords> eventArgs);
        public event EnterNewChunk chunkChanged;

        public delegate void MoveCompleteDelegate(object sender, EventArgsWithPayload<FloatCoords> eventArgs);

        public event MoveCompleteDelegate MoveComplete;

        public Location_Controller(WorldController worldModel, float lx, float ly)
        {
            LocationModel = new LocationModel(lx, ly);
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

            float speed = LocationModel.Parent.UnitModel.Speed;

            if (DistanceCalculator.DiagonalDistance(Waypoints.Peek(), LocationModel.FloatCoords) < speed)
            {
                // Arrived near destination
                LocationModel.FloatCoords = Waypoints.Dequeue();

                if (!Waypoints.Any()) MoveComplete?.Invoke(this, new EventArgsWithPayload<FloatCoords>(LocationModel.FloatCoords));

                return;
            }

            float xDifference = (float) DistanceCalculator.CalcDistance(LocationModel.FloatCoords.x, Waypoints.Peek().x);
            float yDifference = (float) DistanceCalculator.CalcDistance(LocationModel.FloatCoords.y, Waypoints.Peek().y);

            // calculate new coords
            float diagonalDifference = (float) DistanceCalculator.Pythagoras(xDifference, yDifference);
            float v = diagonalDifference / speed;

            FloatCoords difference = new FloatCoords
            {
                x = xDifference / v,
                y = yDifference / v
            };

            difference.x = Waypoints.Peek().x < LocationModel.FloatCoords.x ? -difference.x : difference.x;

            difference.y = Waypoints.Peek().y < LocationModel.FloatCoords.y ? -difference.y : difference.y;

            FloatCoords tempCoords = new FloatCoords()
            {
                x = LocationModel.FloatCoords.x + difference.x,
                y = LocationModel.FloatCoords.y + difference.y
            };

            if(WorldPositionCalculator.ChunkCoordsOfCellCoords(tempCoords) != WorldPositionCalculator.ChunkCoordsOfCellCoords(LocationModel.FloatCoords))
            {
                EventArgsWithPayload<Coords> args = new EventArgsWithPayload<Coords>(WorldPositionCalculator.ChunkCoordsOfCellCoords(tempCoords));
                chunkChanged?.Invoke(this, args);
            }


            LocationModel.FloatCoords = tempCoords;
        }
    }
}