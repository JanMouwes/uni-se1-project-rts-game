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
    public class Faction_Model : IHasFactionRelationship, IHasFactionUnit, IHasFactionBuilding
    {
        public string Name { get; set; }
        public Dictionary<Faction_Model, Faction_Relations> FactionRelationships { get; set; }
        public List<Unit_Model> Units { get; }
        public List<Building_Model> Buildings { get; }

        public bool IsHostileTo(IHasFaction faction)
        {
            foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in FactionRelationships)
            {
                if (relationship.Key.Name == faction.Faction.Name && relationship.Value == Faction_Relations.hostile)
                {
                    return true;
                } else
                {
                    return false;
                }
            }

            return false;
        }

        public void AddRelationship(Faction_Model faction, Faction_Relations relation)
        {
            foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in FactionRelationships)
            {
                if (relationship.Key.Name == faction.Name)
                {
                    break;
                } else
                {
                    FactionRelationships.Add(faction, relation);
                }
            }
        }

        public void ChangeRelationship(Faction_Model faction, Faction_Relations relation)
        {
            foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in FactionRelationships)
            {
                if (relationship.Key.Name == faction.Name && relationship.Value != relation)
                {
                    FactionRelationships[relationship.Key] = relation;
                }
            }
        }
    }
}
