using System;
using System.Collections.Generic;
using kbs2.Desktop.World.World;
using kbs2.Unit.Model;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;


public struct CellWeight
{
    public double AbsoluteDistanceToTarget;
    public double AbsoluteDistanceToUnit;
    public double DistanceToUnit;
    public double Weight => AbsoluteDistanceToTarget + DistanceToUnit;
}

public struct WeightDictionarys
{
    public WeightDictionarys(bool shit)// bool does nothing but is required
    {
        CellsWithWeight = new Dictionary<Coords, CellWeight>();
        BorderCellsWithWeight = new Dictionary<Coords, CellWeight>();
        ObstacleList = new List<Coords>();
    }

    public Dictionary<Coords, CellWeight> CellsWithWeight;
    public Dictionary<Coords, CellWeight> BorderCellsWithWeight;
    public List<Coords> ObstacleList;
}

public class Pathfinder
{
    
    WorldModel worldModel;
    public int Limit { get; set; }

    static Func<double, double, double> pythagoras = (x, y) => Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
    static Func<double, double, double> getDistance = (x, y) => x > y ? x - y : y - x;
    Func<Coords, Coords, double> getDistance2d = (a, b) => pythagoras(getDistance(a.x, b.x), getDistance(a.y, b.y));


    public Pathfinder(WorldModel worldModel,int limit)
	{
        this.worldModel = worldModel;
        Limit = limit;
	}

    public List<Coords> FindPath(FloatCoords TargetCoords, Unit_Model unit)
    {
        WeightDictionarys weightDictionarys = new WeightDictionarys(true);

        //TODO add unit cell that contains unit to CellsWithWeight
        Coords targetIntCoords;
        targetIntCoords.x = (int)TargetCoords.x;
        targetIntCoords.y = (int)TargetCoords.y;

        for (int i = 0; i<Limit*2*Limit*2*2;i++)
        {

            double lowestWeight = double.MaxValue;
            Coords lowestCoords = new Coords();
            bool isset = false;
            foreach (KeyValuePair<Coords, CellWeight> entry in weightDictionarys.BorderCellsWithWeight)
            {
                if (entry.Value.Weight < lowestWeight)
                {
                    lowestWeight = entry.Value.Weight;
                    lowestCoords = entry.Key;
                    isset = true;
                }
            }

            if (isset)
            {
                CalculateWeight(lowestCoords, targetIntCoords, unit, weightDictionarys);
                weightDictionarys.BorderCellsWithWeight.Remove(lowestCoords);
            }
            else
            {
                // TODO no path found
            }

            if (weightDictionarys.CellsWithWeight.ContainsKey(targetIntCoords))
            {
                // path found
                break;
            }
        }

        return null;
    }

    private void CalculateWeight(Coords currentCell, Coords TargetCoords, Unit_Model unit, WeightDictionarys weightDictionarys)
    {
        Coords[] HorizontalNeigbours = new Coords[4];
        HorizontalNeigbours[0] = new Coords { x = 1, y = 0 };
        HorizontalNeigbours[1] = new Coords { x = -1, y = 0 };
        HorizontalNeigbours[2] = new Coords { x = 0, y = 1 };
        HorizontalNeigbours[3] = new Coords { x = 0, y = -1 };

        Coords[] DiagonalNeigbours = new Coords[4];
        DiagonalNeigbours[0] = new Coords { x = 1, y = 1 };
        DiagonalNeigbours[1] = new Coords { x = -1, y = 1 };
        DiagonalNeigbours[2] = new Coords { x = -1, y = 1 };
        DiagonalNeigbours[3] = new Coords { x = -1, y = -1 };
        
        for (int i = 0; i<4; i++)
        {
            Coords coords = currentCell + HorizontalNeigbours[i];
            SetWeightCell(currentCell, coords, TargetCoords, unit, weightDictionarys);
        }
        for (int i = 0; i < 4; i++)
        {
            Coords coords = currentCell + DiagonalNeigbours[i];
            SetWeightCell(currentCell, coords, TargetCoords, unit, weightDictionarys);
        }
    }

    private void SetWeightCell(Coords CurrentCoords, Coords NieghbourCoords, Coords TargetCoords, Unit_Model unit, WeightDictionarys weightDictionarys)
    {
        if (weightDictionarys.ObstacleList.Contains(NieghbourCoords))
        {
            return;
        }
        if (weightDictionarys.CellsWithWeight.ContainsKey(NieghbourCoords))
        {
            if (!weightDictionarys.BorderCellsWithWeight.ContainsKey(NieghbourCoords))
            {
                return;
            }
        }
        if (CheckDiagonalsBlocked(CurrentCoords, NieghbourCoords, weightDictionarys))
        {
            return;
        }

        Coords chunkCoords;
        chunkCoords.x = NieghbourCoords.x / worldModel.ChunkSize;
        chunkCoords.y = NieghbourCoords.y / worldModel.ChunkSize;

        int coordsInChunkx = NieghbourCoords.x % worldModel.ChunkSize;
        int coordsInChunky = NieghbourCoords.y % worldModel.ChunkSize;

        WorldCellModel cell;
        cell = worldModel.ChunkGrid[chunkCoords].worldChunkModel.grid[coordsInChunkx, coordsInChunky];

        if (CellIsObstacle(cell, unit))
        {
            weightDictionarys.ObstacleList.Add(NieghbourCoords);
            return;
        }


        CellWeight cellWeight;


        cellWeight.AbsoluteDistanceToTarget = getDistance2d(NieghbourCoords, TargetCoords) * 10;
        cellWeight.AbsoluteDistanceToUnit = getDistance2d(NieghbourCoords, TargetCoords) * 10; // TODO change targetcoords to unitcoords


        if (cellWeight.AbsoluteDistanceToTarget > Limit || cellWeight.AbsoluteDistanceToUnit > Limit) // check if cell is within limit
        {
            return;
        }

        if (CurrentCoords.x == NieghbourCoords.x || CurrentCoords.y == NieghbourCoords.y)
        {
            cellWeight.DistanceToUnit = weightDictionarys.CellsWithWeight[CurrentCoords].DistanceToUnit + 10;
        }
        else
        {
            cellWeight.DistanceToUnit = weightDictionarys.CellsWithWeight[CurrentCoords].DistanceToUnit + 14;
        }

        if (weightDictionarys.BorderCellsWithWeight.ContainsKey(NieghbourCoords)) //if Nieghbourcell already has a weight overwrite it if the new one is lower
        {
            if (weightDictionarys.CellsWithWeight[NieghbourCoords].DistanceToUnit > cellWeight.DistanceToUnit)
            {
                weightDictionarys.BorderCellsWithWeight[NieghbourCoords] = cellWeight;
                weightDictionarys.CellsWithWeight[NieghbourCoords] = cellWeight;
            }
            return;
        }

        weightDictionarys.BorderCellsWithWeight.Add(NieghbourCoords, cellWeight);
        weightDictionarys.CellsWithWeight.Add(NieghbourCoords, cellWeight);
    }


    private bool CellIsObstacle(WorldCellModel Cell, Unit_Model unit)
    {
        bool r = true;
        if (unit.UnwalkableTerrain.Contains(Cell.terrain))
        {
            r = false;
        }
        // TODO check buildings

        return r;
    }

    private List<FloatCoords> CellToFloatCoords(List<Coords> coords)
    {
        List<FloatCoords> floatCoords = new List<FloatCoords>();

        foreach( Coords entry in coords)
        {
            FloatCoords temp;
            temp.x = entry.x;
            temp.y = entry.y;
            floatCoords.Add(temp);
        }
        return floatCoords;
    }


    private List<FloatCoords> MinimizeWaypoints(List<FloatCoords> RouteCoords)
    {
        List<FloatCoords> WayPoints = new List<FloatCoords>();
        WayPoints.Add(RouteCoords[0]);

        while (true)
        {
            WayPoints.Add(FindNextWayPoint(RouteCoords, WayPoints));

            if (WayPoints[WayPoints.Count].Equals(RouteCoords[RouteCoords.Count]))
            {
                break;
            }
        }
        return WayPoints;
    }

    private FloatCoords FindNextWayPoint(List<FloatCoords> RouteCoords, List<FloatCoords> WayPoints)
    {
        FloatCoords l1 = WayPoints[WayPoints.Count];
        FloatCoords l2;

        for (int i = 1; true; i++)
        {
            if (RouteCoords.IndexOf(l1) + i <= RouteCoords.Count)
            {
                l2 = RouteCoords[RouteCoords.IndexOf(l1) + i];
            }
            else
            {
                return RouteCoords[RouteCoords.IndexOf(l1) + i - 1];
            }


            for (int j = 0; j < i; j++)
            {
                FloatCoords point = RouteCoords[RouteCoords.IndexOf(l1) + j];

                double DistancePointToLine = Math.Abs((l2.x - l1.x) * (l1.y - point.y) - (l1.x - point.x) * (l2.y - l1.y)) / Math.Sqrt(Math.Pow(l2.x - l1.x, 2) + Math.Pow(l2.y - l1.y, 2));

                if (DistancePointToLine > 0.5)
                {
                    return RouteCoords[RouteCoords.IndexOf(l2)];
                }
            }
        }
    }

    // Checks if 
    private bool CheckDiagonalsBlocked(Coords one, Coords two, WeightDictionarys stuf)
    {
        //checks if coord are not next to each other
        if (one.x == two.x || one.y == two.y)
        {
            return true;
        }

        Coords three;
        three.x = one.x;
        three.y = two.y;

        Coords four;
        four.x = two.x;
        four.y = one.y;

        //Checks if both coords are blocked by an obstacle
        if(stuf.ObstacleList.Contains(three) && stuf.ObstacleList.Contains(four))
        {
            return false;
        }

        return true;
    }
}
