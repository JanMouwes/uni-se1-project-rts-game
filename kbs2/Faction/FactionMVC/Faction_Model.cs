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

        public Faction_Model(string name)
        {
			Name = name;
			FactionRelationships = new Dictionary<Faction_Model, Faction_Relations>();
        }

        // Checks if the given faction is hostile to this faction
        public bool IsHostileTo(Faction_Model faction)
        {
            foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in FactionRelationships)
            {
                if (relationship.Key.Name == faction.Name && relationship.Value == Faction_Relations.hostile)
                {
                    return true;
                }
            }

            return false;
        }
        // Adds a new relationship from the given faction to the FactionRelationships dictionary
        public void AddRelationship(Faction_Model faction, Faction_Relations relation)
        {
            // Checks if there is not already an existing relation with this faction
            int i = 0;

            foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in FactionRelationships)
            {
                if (relationship.Key.Name == faction.Name)
                {
                    i++;
                }
            }

            if(i == 0)
            {
                FactionRelationships.Add(faction, relation);
                faction.FactionRelationships.Add(this, relation);
            }
        }
        // Edits the relationship from the given faction to the FactionRelationships dictionary
        public void ChangeRelationship(Faction_Model faction, Faction_Relations relation)
        {
            foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in FactionRelationships)
            {
                // Checks if the given relation is not the same as the current dictionary's relation
                if (relationship.Key.Name == faction.Name && relationship.Value != relation)
                {
                    FactionRelationships[relationship.Key] = relation;
                }
            }
        }
    }
}
