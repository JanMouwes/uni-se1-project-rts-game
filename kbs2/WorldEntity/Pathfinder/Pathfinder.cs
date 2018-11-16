using System;
using System.Collections.Generic;
using kbs2.Unit.Model;
using kbs2.World;
using kbs2.World.Structs;

public class Pathfinder
{
    Dictionary<Coords, float> CellsWithWeight;

	public Pathfinder()
	{
	}

    public List<Coords> FindPath(FloatCoords TargetCoords, Unit_Model unit)
    {
        return null;
    }

    private void CalculateWeight(Coords targetCell)
    {

    }

    private bool CellIsObstacle(Coords Cell, Unit_Model unit)
    {
        return true;
    }

    private List<FloatCoords> MinimizeWaypoints(List<Coords> RouteCells)
    {
        return null;
    }


}
