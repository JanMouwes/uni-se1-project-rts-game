using kbs2.WorldEntity.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.WorldEntity.Building.BuildingMVC;

namespace kbs2.Faction.Interfaces
{
    public interface IHasFactionBuilding
    {
        List<BuildingController> Buildings { get; }
    }
}
