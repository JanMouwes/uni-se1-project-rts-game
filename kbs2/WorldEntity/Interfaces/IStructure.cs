using System.Collections.Generic;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;

namespace kbs2.WorldEntity.Interfaces
{
    public interface IStructure : IStructure<IStructureDef>
    {
    }

    public interface IStructure<out TStructureDef> : IWorldEntity, IImpassable where TStructureDef : IStructureDef
    {
        /// <summary>
        /// Cells which the structure occupies
        /// </summary>
        List<WorldCellModel> OccupiedCells { get; }

        /// <summary>
        /// Coords to which the Def's BuildingShape's Coords are relative 
        /// </summary>
        Coords StartCoords { get; set; }

        /// <summary>
        /// Definition of the Structure
        /// </summary>
        TStructureDef Def { get; }

        /// <summary>
        /// midle of the building
        /// </summary>
        FloatCoords center { get; }
        
        int viewrange { get; }
        
        float With { get; }
        float Heigth { get; }
    }
}