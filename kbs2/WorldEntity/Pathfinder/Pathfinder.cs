using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Location;
using kbs2.WorldEntity.Pathfinder.Exceptions;
using kbs2.WorldEntity.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.WorldEntity.Pathfinder
{
    public class Pathfinder
    {
        private const int DEFAULT_SEARCH_LIMIT = 500;

        /// <summary>
        /// <para>Constants related to pathfinding-animation.</para>
        /// <para>Only relevant if ENABLE_ANIMATION is true/</para>
        /// </summary>

        #region Animation constants

        private const bool ENABLE_ANIMATION = false;

        private const int ANIMATION_DELAY_MILLIS = 300;

        #endregion


        private WorldController worldController;
        private int SearchLimit { get; }
        public List<Coords> CheckedCells;


        /// <summary>
        /// Constructor for pathfinder
        /// </summary>
        /// <param name="worldController">World to find path in</param>
        /// <param name="searchLimit">Limit on searching for path. DEFAULT_SEARCH_LIMIT if omitted</param>
        public Pathfinder(WorldController worldController, int searchLimit = DEFAULT_SEARCH_LIMIT)
        {
            this.worldController = worldController;
            SearchLimit = searchLimit;
        }

        private LinkedList<FloatCoords> FindRoughPath(FloatCoords from, FloatCoords targetCoords, LocationModel unit)
        {
            CoordsCalculator unitCoordsCalculator = new CoordsCalculator(from);
            CoordsCalculator targetCoordsCalculator = new CoordsCalculator(targetCoords);

            LinkedList<FloatCoords> currentPath = new LinkedList<FloatCoords>();
            List<Coords> visitedCoords = new List<Coords>();

            FloatCoords currentCoords = from;

            bool PreviousNodeExists() => currentPath.Last?.Previous != null;
            bool IsPreviousNode(Coords target) => currentPath.Last?.Previous != null && (Coords) currentPath.Last.Previous.Value == target;
            bool IsVisited(Coords target) => visitedCoords.Contains(target);

            void RemoveBetween(FloatCoords fromWhere, FloatCoords toWhere, LinkedList<FloatCoords> source)
            {
                if (!currentPath.Contains(fromWhere)) return;
                if (!currentPath.Contains(toWhere)) return;

                LinkedListNode<FloatCoords> currentNode = currentPath.Find(fromWhere);
                while (currentNode?.Next != null && currentNode.Next.Value != toWhere)
                {
                    currentPath.Remove(currentNode.Next);
                }
            }

            while ((Coords) currentCoords != (Coords) targetCoords)
            {
                Dictionary<Coords, CellWeightValues> currentNeighbours = CalculateNeighboursWeights((Coords) currentCoords, (Coords) targetCoords, (Coords) @from, unit);

                if (currentNeighbours.Any(item => currentPath.Contains((FloatCoords) item.Key)))
                    RemoveBetween((FloatCoords) currentNeighbours.First(item => currentPath.Contains((FloatCoords) item.Key)).Key, currentCoords, currentPath);


                //    Find most probable cell,
                //    WHERE:
                //    1. NOT Cell has been visited AND isn't the previous node 
                //    2. AND isn't blocked
                List<FloatCoords> mostProbableCandidates = (
                    from KeyValuePair<Coords, CellWeightValues> pair in currentNeighbours
                    let neighbour = pair.Key
                    where !IsVisited(neighbour)
                          && !IsCellBlocked((Coords) currentCoords, neighbour, unit)
                    let cellWeightValues = pair.Value
                    orderby cellWeightValues.Weight, cellWeightValues.AbsoluteDistanceToTarget
                    select (FloatCoords) neighbour).ToList();

//                if (PreviousNodeExists()) mostProbableCandidates.Add((FloatCoords) currentNeighbours.First(item => IsPreviousNode(item.Key)).Key);

                FloatCoords mostProbableCoords = mostProbableCandidates.First();

                if (ENABLE_ANIMATION)
                {
                    worldController.GetCellFromCoords((Coords) currentCoords).worldCellView.Colour = Color.Red;
                    worldController.GetCellFromCoords((Coords) mostProbableCoords).worldCellView.Colour = Color.Blue;
                    Thread.Sleep(ANIMATION_DELAY_MILLIS);
                }

                if (!visitedCoords.Contains((Coords) mostProbableCoords))
                {
                    visitedCoords.Add((Coords) mostProbableCoords);
                }

                if (!mostProbableCandidates.Any() || currentPath.Contains(mostProbableCoords))
                {
                    LinkedListNode<FloatCoords> mostProbableNode = currentPath.Find(mostProbableCoords);
                    while (mostProbableNode?.Next != null)
                    {
                        currentPath.Remove(mostProbableNode.Next);
                    }
                }
                else if (!(unitCoordsCalculator.DistanceToFloatCoords(mostProbableCoords) > SearchLimit
                           || targetCoordsCalculator.DistanceToFloatCoords(mostProbableCoords) > SearchLimit))
                {
                    currentPath.AddLast(mostProbableCoords);
                }

                currentCoords = currentPath.Last.Value;
            }

            return currentPath;
        }

        /// <summary>
        /// returns a path to the target that avoids obstacles
        /// </summary>
        /// <param name="targetCoords">Target</param>
        /// <param name="unit">Model containing info about the unit's location</param>
        /// <returns>Path</returns>
        /// <exception cref="NoPathFoundException">Throws exception when no path found</exception>
        public List<FloatCoords> FindPath(FloatCoords targetCoords, LocationModel unit)
        {
            if (IsCellImpassable(worldController.GetCellFromCoords((Coords) targetCoords).worldCellModel, unit))
                throw new NoPathFoundException(unit.Coords, (Coords) targetCoords);

            LinkedList<FloatCoords> currentPath = FindRoughPath(unit.FloatCoords, targetCoords, unit);

            if (!currentPath.Any()) throw new NoPathFoundException(unit.Coords, (Coords) targetCoords);

            Queue<FloatCoords> waypointRoute = ReduceWaypoints(currentPath.ToList(), unit);
//            Queue<FloatCoords> cleanRoute = new Queue<FloatCoords>();
//
//            FloatCoords currentWayPoint;
//            while (waypointRoute.Count > 1)
//            {
//                currentWayPoint = waypointRoute.Dequeue();
//
//
//                LinkedList<FloatCoords> currentRoughPath;
//                int currentLength = int.MaxValue;
//                int previousLength;
//
//                do
//                {
//                    FloatCoords nextPoint = waypointRoute.Dequeue();
//                    previousLength = currentLength;
//                    currentRoughPath = FindRoughPath(currentWayPoint, nextPoint, unit);
//                    currentLength = currentRoughPath.Count;
//                } while (currentLength < previousLength);
//
//                foreach (FloatCoords floatCoords in currentRoughPath)
//                {
//                    cleanRoute.Enqueue(floatCoords);
//                }
//            }
//
//            cleanRoute = ReduceWaypoints(cleanRoute.ToList(), unit);

            return waypointRoute.ToList();
        }

        // sets the weight-values of all the neighbours of a cell
        private Dictionary<Coords, CellWeightValues> CalculateNeighboursWeights(Coords currentCell, Coords targetCoords, Coords originCoords, LocationModel unit)
        {
            CoordsCalculator coordsCalculator = new CoordsCalculator((FloatCoords) currentCell);
            FloatCoords[] neighbours = new FloatCoords[8];

            coordsCalculator.GetNeighbours().CopyTo(neighbours, 0);

            return (from neighbour in neighbours
                where worldController.GetCellFromCoords((Coords) neighbour) != null
                      && !IsCellImpassable(worldController.GetCellFromCoords((Coords) neighbour).worldCellModel, unit)
                let cellWeights = CalculateCellWeights((Coords) neighbour, originCoords, targetCoords)
                select new KeyValuePair<Coords, CellWeightValues>((Coords) neighbour, cellWeights)).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        /// <summary>
        /// Calculates algorythm's cell-weights per cell
        /// </summary>
        /// <param name="cellCoords"></param>
        /// <param name="unitCoords"></param>
        /// <param name="targetCoords"></param>
        /// <returns></returns>
        private CellWeightValues CalculateCellWeights(Coords cellCoords, Coords unitCoords, Coords targetCoords)
        {
            CoordsCalculator coordsCalculator = new CoordsCalculator((FloatCoords) cellCoords);

            return new CellWeightValues
            {
                AbsoluteDistanceToUnit = coordsCalculator.DistanceToFloatCoords((FloatCoords) unitCoords),
                AbsoluteDistanceToTarget = coordsCalculator.DistanceToFloatCoords((FloatCoords) targetCoords)
            };
        }

        /// <summary>
        /// Checks if cell is impassible by unit
        /// </summary>
        /// <param name="cell">Cell in question</param>
        /// <param name="unit">Unit that tries to pass</param>
        /// <returns>Impassibility</returns>
        private bool IsCellImpassable(WorldCellModel cell, LocationModel unit)
        {
            if (cell == null) return true;

            CoordsCalculator coordsCalculator = new CoordsCalculator((FloatCoords) cell.RealCoords);

            return coordsCalculator.DistanceToFloatCoords((FloatCoords) unit.Coords) > SearchLimit || CellIsObstacle(cell, unit);
        }

        // checks if a cell is an obstacle for the specifeid unit
        private bool CellIsObstacle(WorldCellModel cell, LocationModel unit) => unit.UnwalkableTerrain.Contains(cell.Terrain) || cell.BuildingOnTop != null;

        /// <summary>
        /// Asserts unit's traversability of all cells on a path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="unitLocationModel"></param>
        /// <returns></returns>
        private bool AreAllCellsLegal(IEnumerable<Coords> path, LocationModel unitLocationModel)
        {
            return path.Select(coords => worldController.GetCellFromCoords(coords)).All(cell => cell != null && !IsCellImpassable(cell.worldCellModel, unitLocationModel));
        }

        /// <summary>
        /// Reduces amount of waypoints in a list, makes path smoother
        /// </summary>
        /// <param name="routeTemp"></param>
        /// <param name="unitLocationModel"></param>
        /// <returns></returns>
        private Queue<FloatCoords> ReduceWaypoints(IEnumerable<FloatCoords> routeTemp, LocationModel unitLocationModel)
        {
            Queue<FloatCoords> route = new Queue<FloatCoords>(routeTemp);

            FloatCoords target = route.Last();


            Queue<FloatCoords> wayPoints = new Queue<FloatCoords>();
            wayPoints.Enqueue(unitLocationModel.FloatCoords);

            //    While not arrived at destination
            while (wayPoints.Last() != target)
            {
                FloatCoords currentCoords = wayPoints.Last();
                CoordsCalculator coordsCalculator = new CoordsCalculator(currentCoords);

                List<Coords> rayCells;
                FloatCoords? nextCoords = null;

                FloatCoords? previousNextCoords = null;
                FloatCoords? difference = null;
                FloatCoords? previousWaypointDifference = null;

                if (ENABLE_ANIMATION)
                {
                    worldController.GetCellFromCoords((Coords) currentCoords).worldCellView.Colour = Color.Purple;
                }

                bool IsDifference45DegreeDiagonal(FloatCoords coords) => Math.Abs(Math.Abs(coords.x) - Math.Abs(coords.y)) < 0.0000001;

                bool IsDirectionalNeighbour(FloatCoords? previous, FloatCoords? next, FloatCoords? delta)
                {
                    return previous != null
                           && delta != null
                           && !(next == previous + delta);
                }

                do
                {
                    if (!route.Any()) break;

                    previousNextCoords = nextCoords;
                    nextCoords = route.Dequeue();

                    if (previousNextCoords != null)
                        difference = nextCoords - previousNextCoords;

                    previousWaypointDifference = nextCoords - currentCoords;


                    if (ENABLE_ANIMATION)
                    {
                        worldController.GetCellFromCoords((Coords) nextCoords).worldCellView.Colour = Color.Green;
                        Thread.Sleep(ANIMATION_DELAY_MILLIS);
                    }
                } while (route.Count > 0
                         && (IsDirectionalNeighbour(previousNextCoords, nextCoords, difference)
                             || IsDifference45DegreeDiagonal((FloatCoords) previousWaypointDifference)));

                if (nextCoords == null) continue;

                wayPoints.Enqueue((FloatCoords) (previousNextCoords ?? nextCoords));

                if (nextCoords == target) wayPoints.Enqueue((FloatCoords) nextCoords);
            }

            return wayPoints;
        }


        // Checks if Diagonal move is possible/not blocked 
        public bool IsCellBlocked(Coords originCoords, Coords destinationCoords, LocationModel unitLocationModel)
        {
            CoordsCalculator coordsCalculator = new CoordsCalculator((FloatCoords) originCoords);

            if (!coordsCalculator.GetNeighbours().Contains((FloatCoords) destinationCoords))
                throw new ArgumentException("destinationCoords must be neighbouring originCoords");

            //checks if coords are next to each other
            if (coordsCalculator.GetNonDiagonalNeighbours().Contains((FloatCoords) destinationCoords))
            {
                return false;
            }

            Coords commonNeighbour1 = new Coords
            {
                x = originCoords.x,
                y = destinationCoords.y
            };

            Coords commonNeighbour2 = new Coords
            {
                x = destinationCoords.x,
                y = originCoords.y
            };

            WorldCellController cell1 = worldController.GetCellFromCoords(commonNeighbour1);
            WorldCellController cell2 = worldController.GetCellFromCoords(commonNeighbour2);

            //Checks if both coords are blocked by an obstacle
            return cell1 != null && IsCellImpassable(cell1.worldCellModel, unitLocationModel)
                   || cell2 != null && IsCellImpassable(cell2.worldCellModel, unitLocationModel);
        }
    }
}