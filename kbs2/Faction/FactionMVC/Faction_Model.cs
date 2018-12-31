using kbs2.Faction.Enums;
using kbs2.Faction.Interfaces;
using kbs2.Unit.Model;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Unit.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.WorldEntity.Building.BuildingMVC;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.Faction.FactionMVC
{
    public class Faction_Model : IHasFactionUnit, IHasFactionBuilding
    {
        public string Name { get; set; }
        public Dictionary<Faction_Model, Faction_Relations> FactionRelationships { get; set; }
        public List<UnitController> Units { get; set; }
        public List<IStructure> Buildings { get; set; }

        public Faction_Model(string name)
        {
			Name = name;
			FactionRelationships = new Dictionary<Faction_Model, Faction_Relations>();
            Units = new List<UnitController>();
            //TODO use central 'List<IStructure>' with LINQ-queries for filtering
            Buildings = new List<IStructure>(); 
        }
    }
}
