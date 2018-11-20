using kbs2.Faction.Enums;
using kbs2.Faction.FactionMVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Faction.Interfaces
{
    public interface IHasFactionRelationship
    {
        bool IsHostileTo(Faction_Model faction);
        void ChangeRelationship(Faction_Model faction, Faction_Relations relation);
        void AddRelationship(Faction_Model faction, Faction_Relations relation);
    }
}
