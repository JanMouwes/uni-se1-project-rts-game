using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Location;

namespace kbs2.WorldEntity.Pathfinder
{
    public class Pathfinder
    {
        public const int DEFAULT_SEARCH_LIMIT = 500;

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
                throw new NoPathFoundException(unit.coords, (Coords) targetCoords);

            CoordsCalculator unitCoordsCalculator = new CoordsCalculator(unit.FloatCoords);
            CoordsCalculator targetCoordsCalculator = new CoordsCalculator(targetCoords);

            LinkedList<FloatCoords> currentPath = new LinkedList<FloatCoords>();
            List<Coords> visitedCoords = new List<Coords>();

            FloatCoords currentCoords = unit.FloatCoords;

            while ((Coords) currentCoords != (Coords) targetCoords)
            {
                //    Check if we took a step back and thus are on a node we were previously
                if (!visitedCoords.Contains((Coords) currentCoords))
                {
                    visitedCoords.Add((Coords) currentCoords);
                }

                Dictionary<Coords, CellWeightValues> currentNeighbours = CalculateNeighboursWeights((Coords) currentCoords, (Coords) targetCoords, unit);

                //    Find most probable cell
                FloatCoords mostProbableCoords = (
                    from KeyValuePair<Coords, CellWeightValues> pair in currentNeighbours
                    where !(visitedCoords.Contains(pair.Key) //    Coords' been visited AND isn't the previous Coords AND isn't blocked by diagonals
                            && currentPath.Last.Previous != null
                            && currentPath.Last.Previous.Value != (FloatCoords) pair.Key)
                          && !IsDiagonalPathBlocked((Coords) currentCoords, pair.Key, unit)
                    let cellWeightValues = pair.Value
                    orderby cellWeightValues.Weight, cellWeightValues.AbsoluteDistanceToTarget
                    select (FloatCoords) pair.Key).First();

                //    Take a step back if all other options invalid
                if (currentPath.Last == null
                    || !(currentPath.Last.Previous != null
                         && mostProbableCoords == currentPath.Last.Previous.Value
                         || unitCoordsCalculator.DistanceToFloatCoords(mostProbableCoords) > SearchLimit) || targetCoordsCalculator.DistanceToFloatCoords(mostProbableCoords) > SearchLimit)
                {
                    currentPath.AddLast(mostProbableCoords);
                }

                currentCoords = currentPath.Last.Value;
            }

            if (!currentPath.Any()) throw new NoPathFoundException(unit.coords, (Coords) targetCoords);

            Queue<FloatCoords> route = ReduceWaypoints(currentPath.ToList(), unit);

            return route.ToList();
        }

        // sets the weight-values of all the neighbours of a cell
        private Dictionary<Coords, CellWeightValues> CalculateNeighboursWeights(Coords currentCell, Coords targetCoords, LocationModel unit)
        {
            CoordsCalculator coordsCalculator = new CoordsCalculator((FloatCoords) currentCell);
            FloatCoords[] neighbours = coordsCalculator.GetNeighbours();

            return (from neighbour in neighbours
                where !IsCellImpassable(worldController.GetCellFromCoords((Coords) neighbour).worldCellModel, unit)
                let cellWeights = CalculateCellWeights((Coords) neighbour, unit.coords, targetCoords)
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
            CoordsCalculator coordsCalculator = new CoordsCalculator((FloatCoords) cell.RealCoords);

            return coordsCalculator.DistanceToFloatCoords((FloatCoords) unit.coords) > SearchLimit || CellIsObstacle(cell, unit);
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
            wayPoints.Enqueue(unitLocationModel.floatCoords);

            //    While not arrived at destination
            while (wayPoints.Last() != target)
            {
                FloatCoords currentCoords = wayPoints.Last();
                CoordsCalculator coordsCalculator = new CoordsCalculator(currentCoords);

                List<Coords> rayCells;
                FloatCoords nextCoords;

                do
                {
                    nextCoords = route.Dequeue();

                    rayCells = coordsCalculator.GetCoordsOnRayWith(nextCoords);
                } while (route.Count > 0 && AreAllCellsLegal(rayCells, unitLocationModel));

                wayPoints.Enqueue(nextCoords);
            }

            return wayPoints;
        }


        // Checks if Diagonal move is possible/not blocked 
        public bool IsDiagonalPathBlocked(Coords originCoords, Coords destinationCoords, LocationModel unitLocationModel)
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

            //Checks if both coords are blocked by an obstacle
            return IsCellImpassable(worldController.GetCellFromCoords(commonNeighbour1).worldCellModel, unitLocationModel)
                   || IsCellImpassable(worldController.GetCellFromCoords(commonNeighbour2).worldCellModel, unitLocationModel);
        }
    }
}