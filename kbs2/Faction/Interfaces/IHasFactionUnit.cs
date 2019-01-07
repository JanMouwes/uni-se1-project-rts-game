using kbs2.WorldEntity.Unit.MVC;
using System.Collections.Generic;

namespace kbs2.Faction.Interfaces
{
    public interface IHasFactionUnit
    {
        List<UnitController> Units { get; }
    }
}
