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
        bool IsHostileTo(FactionModel faction);
        void ChangeRelationship(FactionModel faction, Faction_Relations relation);
        void AddRelationship(FactionModel faction, Faction_Relations relation);
    }
}
