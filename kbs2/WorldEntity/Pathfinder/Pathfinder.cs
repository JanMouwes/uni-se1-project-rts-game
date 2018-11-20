using System;
using System.Collections.Generic;
using kbs2.Desktop.World.World;
using kbs2.Unit.Model;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;


public struct CellWeight
{
    public double DistanceToTarget;
    public double DistanceToUnit;
    public double Weight => DistanceToTarget + DistanceToUnit;
}

public struct WeightDictionarys
{
    public WeightDictionarys(bool x)
    {
        CellsWithWeight = new Dictionary<Coords, CellWeight>();
        BorderCellsWithWeight = new Dictionary<Coords, CellWeight>();
    }

    public Dictionary<Coords, CellWeight> CellsWithWeight;
    public Dictionary<Coords, CellWeight> BorderCellsWithWeight;
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

        //todo add unit cell that contains unit to CellsWithWeight
        Coords targetIntCoords;
        targetIntCoords.x = (int)TargetCoords.x;
        targetIntCoords.y = (int)TargetCoords.y;

        for (int i = 0; i<Limit*Limit*2;i++)
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
        Coords[] loop = new Coords[4];
        loop[0] = new Coords { x = 1, y = 0 };
        loop[1] = new Coords { x = -1, y = 0 };
        loop[2] = new Coords { x = 0, y = 1 };
        loop[3] = new Coords { x = 0, y = -1 };




        for (int i = 0; i<4; i++)
        {

            Coords coords = currentCell + loop[i];

            if (!weightDictionarys.CellsWithWeight.ContainsKey(coords))
            {
                Coords chunkCoords;
                chunkCoords.x = coords.x / worldModel.ChunkSize;
                chunkCoords.y = coords.y / worldModel.ChunkSize;
                    
                int coordsInChunkx = coords.x % worldModel.ChunkSize;
                int coordsInChunky = coords.y % worldModel.ChunkSize;

                WorldCellModel cell;
                cell = worldModel.ChunkGrid[chunkCoords].worldChunkModel.grid[coordsInChunkx, coordsInChunky];

                if (!CellIsObstacle(cell, unit))
                {
                    CellWeight cellWeight;

                        
                    cellWeight.DistanceToTarget = getDistance2d(coords, TargetCoords);

                    cellWeight.DistanceToUnit = 0;
                    // TODO calc distance via unit.location

                    if(cellWeight.DistanceToTarget<Limit || cellWeight.DistanceToUnit < Limit)
                    {
                        weightDictionarys.BorderCellsWithWeight.Add(coords, cellWeight);
                        weightDictionarys.CellsWithWeight.Add(coords, cellWeight);
                    }
                }
            }
        }
    }

    private bool CellIsObstacle(WorldCellModel Cell, Unit_Model unit)
    {
        bool r = true;
        if (unit.UnwalkableTerrain.Contains(Cell.terrain))
        {
            r = false;
        }
        // todo check buildings

        return r;
    }

    private List<FloatCoords> MinimizeWaypoints(List<Coords> RouteCells)
    {
        return null;
    }
}
