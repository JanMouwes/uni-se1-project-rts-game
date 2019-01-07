using System.Collections.Generic;
using kbs2.Unit.Interfaces;
using kbs2.World;

namespace kbs2.WorldEntity.Interfaces
{
    public interface IStructureDef : ISpawnableDef, IPurchasable
    {
        /// <summary>
        /// Map of coords relative to start-coords which the structure is to occupy
        /// </summary>
        List<Coords> BuildingShape { get; set; }
    }
}