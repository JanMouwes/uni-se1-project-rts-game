using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.WorldEntity.Building;

namespace kbs2.Faction.Interfaces
{
    public interface IHasFactionBuilding
    {
        List<Building_Model> Buildings { get; set; }
    }
}
