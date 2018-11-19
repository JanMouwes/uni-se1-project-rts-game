using kbs2.Unit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Faction.Interfaces
{
    public interface IHasFactionUnit
    {
        List<Unit_Model> Units { get; set; }
    }
}
