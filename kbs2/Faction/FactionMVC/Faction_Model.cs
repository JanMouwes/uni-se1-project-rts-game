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

namespace kbs2.Faction.FactionMVC
{
    public class Faction_Model : IHasFactionUnit, IHasFactionBuilding
    {
        public string Name { get; set; }
        public Dictionary<Faction_Model, Faction_Relations> FactionRelationships { get; set; }
        public List<Unit_Controller> Units { get; set; }
        public List<Building_Controller> Buildings { get; set; }
        public List<BUCController> BUCs { get; set; }

        public Faction_Model(string name)
        {
			Name = name;
			FactionRelationships = new Dictionary<Faction_Model, Faction_Relations>();
            Units = new List<Unit_Controller>();
            Buildings = new List<Building_Controller>();
            BUCs = new List<BUCController>();
        }
    }
}
