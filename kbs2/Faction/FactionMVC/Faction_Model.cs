using kbs2.Faction.Enums;
using kbs2.Faction.Interfaces;
using kbs2.Unit.Model;
using kbs2.WorldEntity.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Faction.FactionMVC
{
    public class Faction_Model
    {
        public string Name { get; set; }
        public Dictionary<Faction_Model, Faction_Relations> FactionRelationships { get; set; }
        public List<Unit_Model> Units { get; }
        public List<Building_Model> Buildings { get; }

        public Faction_Model(string name)
        {
			Name = name;
			FactionRelationships = new Dictionary<Faction_Model, Faction_Relations>();
        }
    }
}
