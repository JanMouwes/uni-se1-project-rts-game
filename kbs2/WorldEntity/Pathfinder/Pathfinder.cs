using System;
using System.Collections.Generic;
using kbs2.Desktop.World.World;
using kbs2.Unit.Model;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.WorldEntity.Structs;
using kbs2.World.Structs;
using kbs2.WorldEntity.Location;


public class Pathfinder
{

    private WorldModel worldModel;
    public int Limit { get; set; }
    public List<Coords> CheckedCells;

    private static Func<double, double, double> pythagoras = (x, y) => Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
    private static Func<double, double, double> getDistance = (x, y) => x > y ? x - y : y - x;
    private Func<Coords, Coords, double> getDistance2d = (a, b) => pythagoras(getDistance(a.x, b.x), getDistance(a.y, b.y));


    public Pathfinder(WorldModel worldModel,int limit)
	{
        this.worldModel = worldModel;
        Limit = limit;
	}

    // returns a path to the target that does not contain obstacles
    public List<FloatCoords> FindPath(FloatCoords TargetFloatCoords, Location_Model unit)
    {
        WeightDictionarys weightDictionaries = new WeightDictionarys(true);

        Coords targetIntCoords = (Coords)TargetFloatCoords;

        CellWeight unitLocation;
        unitLocation.AbsoluteDistanceToTarget = getDistance2d(unit.coords, targetIntCoords);
        unitLocation.AbsoluteDistanceToUnit = 0;
        unitLocation.DistanceToUnit = 0;

        weightDictionaries.CellsWithWeight.Add(unit.coords ,unitLocation);
        weightDictionaries.BorderCellsWithWeight.Add(unit.coords, unitLocation);


        for (int i = 0; i<Limit*2*Limit*2*2;i++) // backup plan to keep the search area within a limit
        {

            double lowestWeight = double.MaxValue;
            Coords lowestCoords = new Coords();
            bool isset = false;

            // find the bordercell with the lowest weight this cell is the most likely to be in the path to the target
            foreach (KeyValuePair<Coords, CellWeight> entry in weightDictionaries.BorderCellsWithWeight) 
            {
                if (entry.Value.Weight < lowestWeight)
                {
                    lowestWeight = entry.Value.Weight;
                    lowestCoords = entry.Key;
                    isset = true;
                }
            }

            if (!isset)
            {
                // no path found
                return null;
            }

            // find weight of the nieghbours of the cell
            CalculateWeight(lowestCoords, targetIntCoords, unit, weightDictionaries);
            // remove cell from bordercells since the nieghbours are added it is no longer a border
            weightDictionaries.BorderCellsWithWeight.Remove(lowestCoords);


            if (weightDictionaries.CellsWithWeight.ContainsKey(targetIntCoords)) // check if target cell has been found
            {
                // path found
                break;
            }
        }

        List<FloatCoords> route = CellToFloatCoords(DefineRoute(weightDictionaries, targetIntCoords, unit));

        route[0] = unit.floatCoords;
        route[route.Count - 1] = TargetFloatCoords;

        route = MinimizeWaypoints(route);

        return route;
    }




    // finds the route based on the cells that have there weight calculated
    private List<Coords> DefineRoute(WeightDictionarys CheckedCells, Coords TargetCoords , Location_Model unit)
    {
        List<Coords> RouteCells = new List<Coords>();
        RouteCells.Add(TargetCoords);

        //Delete all bordercoords, they are not needed.
        foreach (KeyValuePair<Coords, CellWeight> cell in CheckedCells.BorderCellsWithWeight){
            if (!(cell.Key == TargetCoords))
            {	
                CheckedCells.CellsWithWeight.Remove(cell.Key);
            }
        }

        //Makes the actual route by repeating while last coords in the routecells is not the unit location
        while (RouteCells[RouteCells.Count - 1] != unit.coords) 
        {
            Coords current = RouteCells[RouteCells.Count - 1]; 

            Coords[] Neighbours = new Coords[8];
            GetDiagonalNeighbours().CopyTo(Neighbours,0);
            GetHorizontalNeighbours().CopyTo(Neighbours, 4);
            
            CellWeight lowest = new CellWeight
            {
                DistanceToUnit = float.MaxValue,
                AbsoluteDistanceToTarget = float.MaxValue
            };
            Coords lowestcoords = new Coords();

            //get neighbours from current and replace lowestcoords if its lower
            for (int i = 0; i < 8; i++)
            {
                Coords TempCoords = current + Neighbours[i];
                if(CheckedCells.CellsWithWeight[TempCoords].Weight < lowest.Weight){
                    lowestcoords = TempCoords;
                }
                if (CheckedCells.CellsWithWeight[TempCoords].Weight == lowest.Weight)
                {
                    if(CheckedCells.CellsWithWeight[TempCoords].DistanceToUnit < lowest.DistanceToUnit){
                        lowestcoords = TempCoords;
                    }
                }
            }

            RouteCells.Add(lowestcoords);
        }
        return RouteCells;
      }


    public Coords[] GetHorizontalNeighbours()

    {
        Coords[] HorizontalNeigbours = new Coords[4]; 
        HorizontalNeigbours[0] = new Coords { x = 1, y = 0 };
        HorizontalNeigbours[1] = new Coords { x = -1, y = 0 };
        HorizontalNeigbours[2] = new Coords { x = 0, y = 1 };
        HorizontalNeigbours[3] = new Coords { x = 0, y = -1 };
        return HorizontalNeigbours;
    }

    public Coords[] GetDiagonalNeighbours()
    {
        Coords[] DiagonalNeigbours = new Coords[4];
        DiagonalNeigbours[0] = new Coords { x = 1, y = 1 };
        DiagonalNeigbours[1] = new Coords { x = -1, y = 1 };
        DiagonalNeigbours[2] = new Coords { x = -1, y = 1 };
        DiagonalNeigbours[3] = new Coords { x = -1, y = -1 };
        return DiagonalNeigbours;

    }

        // sets the weightvalues of all the neighbours of a cell
        public void CalculateWeight(Coords currentCell, Coords TargetCoords, Location_Model unit, WeightDictionarys weightDictionarys)
    {
        Coords[] Neighbours = new Coords[8];
        GetHorizontalNeighbours().CopyTo(Neighbours, 0);
        GetDiagonalNeighbours().CopyTo(Neighbours, 4);


        for (int i = 0; i<8; i++) // check non diagonal neighbours first to check if any of the diagonal neighbours are obstructed
        {
            Coords coords = currentCell + Neighbours[i];
            SetWeightCell(currentCell, coords, TargetCoords, unit, weightDictionarys);
        }
    }

    // sets the weightvalues of a single cell and ads them to a dictionary
    public void SetWeightCell(Coords CurrentCoords, Coords NeighbourCoords, Coords TargetCoords, Location_Model unit, WeightDictionarys weightDictionarys)
    {
        if (weightDictionarys.ObstacleList.Contains(NeighbourCoords)) //check if cell is a known obstacle
        {
            return;
        }
        if (weightDictionarys.CellsWithWeight.ContainsKey(NeighbourCoords) && !weightDictionarys.BorderCellsWithWeight.ContainsKey(NeighbourCoords)) // check if cell weight is alread known
        {
            // if cell is a bordercell it is possible a shorter route to it will be found
            return;
        }
        if (CheckDiagonalsBlocked(CurrentCoords, NeighbourCoords, weightDictionarys))// check if the diagonal nieghbour is reachable from currnetcell
        {
            return;
        }

        // get coords of the chunk that contains the neighbourcell
        Coords chunkCoords = new Coords
        {
            x = NeighbourCoords.x / worldModel.ChunkSize,
            y = NeighbourCoords.y / worldModel.ChunkSize
        };
        // get coords of the neighbourcell in relation to the chunk
        int coordsInChunkx = NeighbourCoords.x % worldModel.ChunkSize;
        int coordsInChunky = NeighbourCoords.y % worldModel.ChunkSize;

        // get the actial cell from the worldmodel
        WorldCellModel cell = worldModel.ChunkGrid[chunkCoords].worldChunkModel.grid[coordsInChunkx, coordsInChunky];

        if (CellIsObstacle(cell, unit)) // check if cell is obstacle for this unit
        {
            weightDictionarys.ObstacleList.Add(NeighbourCoords); // add obstacle to obstaclelist
            return;
        }


        CellWeight cellWeight;

        // set weightvalues
        cellWeight.AbsoluteDistanceToTarget = getDistance2d(NeighbourCoords, TargetCoords) * 10;
        cellWeight.AbsoluteDistanceToUnit = getDistance2d(NeighbourCoords, unit.coords) * 10; 


        if (cellWeight.AbsoluteDistanceToTarget > Limit || cellWeight.AbsoluteDistanceToUnit > Limit) // check if cell is within limit
        {
            return;
        }

        // if cell is a diagonal add 14 instead of 10
        cellWeight.DistanceToUnit = CurrentCoords.x == NeighbourCoords.x || CurrentCoords.y == NeighbourCoords.y
            ? weightDictionarys.CellsWithWeight[CurrentCoords].DistanceToUnit + 10
            : weightDictionarys.CellsWithWeight[CurrentCoords].DistanceToUnit + 14;

        if (weightDictionarys.BorderCellsWithWeight.ContainsKey(NeighbourCoords)) //if Nieghbourcell already has a weight overwrite it if the new one is lower
        {
            if (weightDictionarys.CellsWithWeight[NeighbourCoords].DistanceToUnit > cellWeight.DistanceToUnit)
            {
                weightDictionarys.BorderCellsWithWeight[NeighbourCoords] = cellWeight;
                weightDictionarys.CellsWithWeight[NeighbourCoords] = cellWeight;
            }
            return;
        }

        // add new cell to the weightdictionairy
        weightDictionarys.BorderCellsWithWeight.Add(NeighbourCoords, cellWeight);
        weightDictionarys.CellsWithWeight.Add(NeighbourCoords, cellWeight);
    }

    // checks if a cell is an obstacle for the specifeid unit

    public bool CellIsObstacle(WorldCellModel Cell, Location_Model unit)

    {
        bool r = !unit.UnwalkableTerrain.Contains(Cell.Terrain);

        // TODO check buildings

        return r;
    }

    // converts a list of coords to a list of FloatCoords
    public List<FloatCoords> CellToFloatCoords(List<Coords> coords)
    {
        List<FloatCoords> floatCoords = new List<FloatCoords>();
        foreach( Coords entry in coords)
        {
            floatCoords.Add((FloatCoords)entry);
        }
        return floatCoords;
    }

    public List<FloatCoords> MinimizeWaypoints(List<FloatCoords> RouteCoords)
    {
        List<FloatCoords> WayPoints = new List<FloatCoords>
        {
            RouteCoords[0]
        };

        while (!(WayPoints[WayPoints.Count - 1] == RouteCoords[RouteCoords.Count - 1]))
        {
            WayPoints.Add(FindNextWayPoint(RouteCoords, WayPoints));
        }
        return WayPoints;
    }

    public FloatCoords FindNextWayPoint(List<FloatCoords> RouteCoords, List<FloatCoords> WayPoints)
    {
        FloatCoords l1 = WayPoints[WayPoints.Count-1]; // start from the last waypoint added
        FloatCoords l2;

        for (int i = 1; true; i++)
        {
            if (RouteCoords.IndexOf(l1) + i <= RouteCoords.Count) //check if we reached the target
            {
                l2 = RouteCoords[RouteCoords.IndexOf(l1) + i-1];
            }
            else
            {
                return RouteCoords[RouteCoords.Count - 1]; //add target to the waypoints
            }

            // calculate the distance of the line between the two waypoints to all the points we will skip 
            for (int j = 0; j < i; j++) 
            {
                FloatCoords point = RouteCoords[RouteCoords.IndexOf(l1) + j];
                
                // calculate distance between current point an the line
                double DistancePointToLine = Math.Abs((l2.x - l1.x) * (l1.y - point.y) - (l1.x - point.x) * (l2.y - l1.y)) / Math.Sqrt(Math.Pow(l2.x - l1.x, 2) + Math.Pow(l2.y - l1.y, 2));

                if (DistancePointToLine > 0.71) // check if distance is to big 0.71 is half sqrt2 rounded up
                {
                    return RouteCoords[RouteCoords.IndexOf(l2)-1];
                }
            }
        }
    }

    // Checks if Diagonal move is possible/not blocked 
    public bool CheckDiagonalsBlocked(Coords one, Coords two, WeightDictionarys ObstaclesDictorary)
    {
        //checks if coord are not next to each other
        if (one.x == two.x || one.y == two.y)
        {
            return true;
        }

        Coords three = new Coords()
        {
            x = one.x,
            y = two.y
        };

        Coords four = new Coords()
        {
            x = two.x,
            y = one.y
        };

        //Checks if both coords are blocked by an obstacle
        return !ObstaclesDictorary.ObstacleList.Contains(three) || !ObstaclesDictorary.ObstacleList.Contains(four);
    }
}
