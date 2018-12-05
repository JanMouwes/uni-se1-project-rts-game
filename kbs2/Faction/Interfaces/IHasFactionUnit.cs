using kbs2.Unit.Model;
using kbs2.WorldEntity.Unit.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Faction.Interfaces
{
    public interface IHasFactionUnit
    {
        List<Unit_Controller> Units { get; }
    }
}
