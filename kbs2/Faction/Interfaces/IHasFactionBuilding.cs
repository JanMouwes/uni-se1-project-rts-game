using System.Collections.Generic;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.Faction.Interfaces
{
    public interface IHasFactionBuilding
    {
        List<IStructure<IStructureDef>> Buildings { get; }
    }
}
