﻿using kbs2.Faction.Enums;
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
    public class Faction_Controller : IHasFactionRelationship 
    {
        static public Faction_Model FactionModel { get; set; }
        // Adds a unit to the faction units list
        Action<Unit_Model> AddUnitToFaction = unit => FactionModel.Units.Add(unit);
        // Checks if the given faction is hostile to this faction
        public bool IsHostileTo(Faction_Model faction) => FactionModel.FactionRelationships[faction] == Faction_Relations.hostile;
        // Checks if there is a relation with the given faction and changes it to the given relation if not the same
        public void ChangeRelationship(Faction_Model faction, Faction_Relations relation)
        {
            if(FactionModel.FactionRelationships[faction] != relation)
            {
                FactionModel.FactionRelationships.Remove(faction);
                FactionModel.FactionRelationships.Add(faction, relation);

                faction.FactionRelationships.Remove(FactionModel);
                faction.FactionRelationships.Add(FactionModel, relation);
            }
        }
        // Adds a relationship to the faction if it doesnt exist yet
        public void AddRelationship(Faction_Model faction, Faction_Relations relation)
        {
            if (!FactionModel.FactionRelationships.ContainsKey(faction))
            {
                FactionModel.FactionRelationships.Add(faction, relation);
                faction.FactionRelationships.Add(FactionModel, relation);
            }
        }
    }
}
