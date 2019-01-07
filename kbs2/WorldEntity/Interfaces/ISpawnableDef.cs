using System.Collections.Generic;
using kbs2.World.Enums;

namespace kbs2.WorldEntity.Interfaces
{
    public interface ISpawnableDef : IWorldEntityDef
    {
        List<TerrainType> LegalTerrain { get; set; }
    }
}