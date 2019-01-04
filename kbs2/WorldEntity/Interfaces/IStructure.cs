using System.Collections.Generic;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;

namespace kbs2.WorldEntity.Interfaces
{

    public interface IStructure<out TStructureDef> : IWorldEntity, IImpassable where TStructureDef : IStructureDef
    {
        IViewImage View { get; }

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
        /// Entity's view-range in cells
        /// </summary>
        int ViewRange { get; }

        /// <summary>
        /// Centre coords of the building
        /// </summary>
        FloatCoords Centre { get; }

        /// <summary>
        /// Entity's width in cells
        /// </summary>
        float Width { get; }

        /// <summary>
        /// Entity's height in cells
        /// </summary>
        float Height { get; }
    }
}