﻿using System;
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
    List<Coords> RouteCells;

    WorldModel worldModel;
    public int Limit { get; set; }
    public List<Coords> CheckedCells;

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

    private List<Coords> DefineRoute(WeightDictionarys CheckedCells, Coords TargetCoords)
    {
        //Delete all bordercoords, they are not needed.
        foreach (KeyValuePair<Coords, CellWeight> cell in CheckedCells.BorderCellsWithWeight){
            if (cell.Key.x == TargetCoords.x && cell.Key.y == TargetCoords.y)
            {
            }else{
                CheckedCells.CellsWithWeight.Remove(cell.Key);
            }
        }


        while (true)
        {
            Coords current;
            // if current is empty use targetpoint to start from
            current.x = TargetCoords.x;
            current.y = TargetCoords.y;

            List<Coords> NeighboursFromCurrent;

            //get neighbours
            Coords[] Neighbours = new Coords[8];
            Neighbours[0] = new Coords { x = 1, y = 0 };
            Neighbours[1] = new Coords { x = -1, y = 0 };
            Neighbours[2] = new Coords { x = 0, y = 1 };
            Neighbours[3] = new Coords { x = 0, y = -1 };
            Neighbours[4] = new Coords { x = 1, y = 1 };
            Neighbours[5] = new Coords { x = -1, y = 1 };
            Neighbours[6] = new Coords { x = -1, y = 1 };
            Neighbours[7] = new Coords { x = -1, y = -1 };

            for (int i = 0; i < 8; i++)
            {
                Coords TempCoords = current + Neighbours[i];
                NeighboursFromCurrent.Add(TempCoords);
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

    private void SetWeightCell(Coords CurrentCoords, Coords NeighbourCoords, Coords TargetCoords, Unit_Model unit, WeightDictionarys weightDictionarys)
    {
        if (weightDictionarys.ObstacleList.Contains(NeighbourCoords))
        {
            return;
        }
        if (weightDictionarys.CellsWithWeight.ContainsKey(NeighbourCoords))
        {
            if (!weightDictionarys.BorderCellsWithWeight.ContainsKey(NeighbourCoords))
            {
                return;
            }
        }
        if (CheckDiagonalsBlocked(CurrentCoords, NeighbourCoords, weightDictionarys))
        {
            return;
        }

        Coords chunkCoords;
        chunkCoords.x = NeighbourCoords.x / worldModel.ChunkSize;
        chunkCoords.y = NeighbourCoords.y / worldModel.ChunkSize;

        int coordsInChunkx = NeighbourCoords.x % worldModel.ChunkSize;
        int coordsInChunky = NeighbourCoords.y % worldModel.ChunkSize;

        WorldCellModel cell;
        cell = worldModel.ChunkGrid[chunkCoords].worldChunkModel.grid[coordsInChunkx, coordsInChunky];

        if (CellIsObstacle(cell, unit))
        {
            weightDictionarys.ObstacleList.Add(NeighbourCoords);
            return;
        }


        CellWeight cellWeight;


        cellWeight.AbsoluteDistanceToTarget = getDistance2d(NeighbourCoords, TargetCoords) * 10;
        cellWeight.AbsoluteDistanceToUnit = getDistance2d(NeighbourCoords, TargetCoords) * 10; // TODO change targetcoords to unitcoords


        if (cellWeight.AbsoluteDistanceToTarget > Limit || cellWeight.AbsoluteDistanceToUnit > Limit) // check if cell is within limit
        {
            return;
        }

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

        weightDictionarys.BorderCellsWithWeight.Add(NeighbourCoords, cellWeight);
        weightDictionarys.CellsWithWeight.Add(NeighbourCoords, cellWeight);
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

    private List<FloatCoords> MinimizeWaypoints(List<Coords> RouteCells)
    {
        return null;
    }

    // Checks if Diagonal move is possible/not blocked 
    private bool CheckDiagonalsBlocked(Coords one, Coords two, WeightDictionarys ObstaclesDictorary)
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
        if(ObstaclesDictorary.ObstacleList.Contains(three) && ObstaclesDictorary.ObstacleList.Contains(four))
        {
            return false;
        }

        return true;
    }
}
