using kbs2.Faction.Enums;
using kbs2.Faction.Interfaces;
using kbs2.Unit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Faction.FactionMVC
{
    public class Faction_Model : IHasFactionRelationship, IHasFactionUnit, IHasFactionBuilding
    {
        public string Name { get; set; }
        public Dictionary<Faction_Model, Faction_Relations> FactionRelationships { get; set; }
        public List<Unit_Model> Units { get; set; }
        public List<Building_Model> Buildings { get; set; }

        public void AddRelationship(Faction_Model faction, Faction_Relations relation)
        {
            throw new NotImplementedException();
        }

        public void ChangeRelationship(Faction_Model faction, Faction_Relations relation)
        {
            throw new NotImplementedException();
        }

        public bool IsHostileTo(IHasFaction faction)
        {
            throw new NotImplementedException();
        }
    }
}
