using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.World;
using kbs2.World.Structs;

namespace kbs2.utils
{
    public class CoordsCalculator
    {
        private readonly FloatCoords coords;


        public CoordsCalculator(FloatCoords worldCoordsController)
        {
            coords = worldCoordsController;
        }


        /// <summary>
        /// Returns absolute distance between target and coords
        /// </summary>
        /// <param name="target">Target-coords</param>
        /// <returns>Absolute distance as double</returns>
        public double DistanceToFloatCoords(FloatCoords target) => DistanceCalculator.DiagonalDistance(coords, target);

        /// <summary>
        /// The single neighbours of a coordinate
        /// </summary>

        #region Single neighbour methods

        public FloatCoords NorthNeighbour => new FloatCoords() {x = coords.x, y = coords.y + 1};

        public FloatCoords NorthEastNeighbour => new FloatCoords() {x = coords.x + 1, y = coords.y + 1};

        public FloatCoords EastNeighbour => new FloatCoords() {x = coords.x + 1, y = coords.y};

        public FloatCoords SouthEastNeighbour => new FloatCoords() {x = coords.x + 1, y = coords.y - 1};

        public FloatCoords SouthNeighbour => new FloatCoords() {x = coords.x, y = coords.y - 1};

        public FloatCoords SouthWestNeighbour => new FloatCoords() {x = coords.x - 1, y = coords.y - 1};

        public FloatCoords WestNeighbour => new FloatCoords() {x = coords.x - 1, y = coords.y};

        public FloatCoords NorthWestNeighbour => new FloatCoords() {x = coords.x - 1, y = coords.y + 1};

        #endregion

        #region Multiple neighbour methods

        /// <summary>
        /// Returns all of the coords' diagonal neighbours, diagonal and not
        ///   | r |  
        /// r | c | r
        ///   | r |  
        /// </summary>
        /// <returns></returns>
        public FloatCoords[] GetNonDiagonalNeighbours()
        {
            FloatCoords[] horizontalNeighbours = new FloatCoords[4]
            {
                NorthNeighbour,
                EastNeighbour,
                SouthNeighbour,
                WestNeighbour
            };

            return horizontalNeighbours;
        }

        /// <summary>
        /// Returns all of the coords' diagonal neighbours, diagonal and not
        /// r |   | r
        ///   | c |  
        /// r |   | r
        /// </summary>
        /// <returns></returns>
        public FloatCoords[] GetDiagonalNeighbours()
        {
            FloatCoords[] diagonalNeighbours = new FloatCoords[4]
            {
                NorthEastNeighbour,
                SouthEastNeighbour,
                SouthWestNeighbour,
                NorthWestNeighbour
            };

            return diagonalNeighbours;
        }

        /// <summary>
        /// <para>Returns all of the coords' neighbours, diagonal and not diagonal</para>
        /// <para>r | r | r</para>
        /// <para>r | c | r</para>
        /// <para>r | r | r</para>
        /// </summary>
        /// <returns>Array of coords' neighbouring coords</returns>
        public FloatCoords[] GetNeighbours()
        {
            FloatCoords[] returnArray = new FloatCoords[8];

            GetNonDiagonalNeighbours().CopyTo(returnArray, 0);
            GetDiagonalNeighbours().CopyTo(returnArray, 4);

            return returnArray;
        }

        /// <summary>
        /// Finds common neighbours between two coordinates
        /// </summary>
        /// <param name="otherCoords">Other Coords</param>
        /// <returns>List of common neighbours</returns>
        public List<FloatCoords> GetCommonNeighboursWith(Coords otherCoords)
        {
            CoordsCalculator otherCoordsCalculator = new CoordsCalculator((FloatCoords) otherCoords);

            FloatCoords[] myNeighbours = GetNeighbours();

            return otherCoordsCalculator.GetNeighbours().Where(neighbour => myNeighbours.Contains(neighbour)).ToList();
        }

        #endregion

        #region Vector-methods

        /// <summary>
        /// Gets all coords of cells on a ray with destination
        /// </summary>
        /// <param name="destination">Ray's end node</param>
        /// <returns>List of coords the ray passes through</returns>
        public List<Coords> GetCoordsOnRayWith(FloatCoords destination)
        {
            void AddCellsBetweenVerticalPoints(double startPoint, double endPoint, int xCoord, ref List<Coords> outputList)
            {
                //    Switch start- and endpoints if necessary
                if (startPoint > endPoint)
                {
                    startPoint = endPoint + startPoint;
                    endPoint = Math.Ceiling(startPoint - endPoint);
                    startPoint = Math.Floor(startPoint - endPoint);
                }

                for (int i = (int) startPoint - 1; i <= endPoint + 1; i++)
                {
                    Coords currentPoint = new Coords() {x = xCoord, y = i};
                    CoordsCalculator coordsCalculator = new CoordsCalculator((FloatCoords) currentPoint);

                    List<Coords> cells = new List<Coords> {currentPoint};
                    cells.AddRange(coordsCalculator.GetNeighbours().Select(neighbour => (Coords) neighbour));

                    foreach (Coords cell in cells)
                    {
                        if (outputList.Contains(cell)) continue;

                        outputList.Add(cell);
                    }
                }
            }

            List<Coords> coordsOnRay = new List<Coords>();

            FloatCoords startCoords = this.coords;

            //    Switch start- and endpoints if necessary
            if (startCoords.x > destination.x)
            {
                FloatCoords x = startCoords;
                startCoords = destination;
                destination = x;
            }

            double directionCoefficient = (double) destination.x > (double) startCoords.x ? ((double) destination.y - startCoords.y) / ((double) destination.x - startCoords.x) : (double) destination.y - startCoords.y;

            FloatCoords previousCoords;
            FloatCoords currentCoords = startCoords;

            while (currentCoords.x <= destination.x)
            {
                previousCoords = currentCoords;

                currentCoords.x++;
                currentCoords.y += (float) directionCoefficient;

                AddCellsBetweenVerticalPoints(previousCoords.y, currentCoords.y, (int) previousCoords.x, ref coordsOnRay);
            }

            return coordsOnRay;
        }

        #endregion
    }
}