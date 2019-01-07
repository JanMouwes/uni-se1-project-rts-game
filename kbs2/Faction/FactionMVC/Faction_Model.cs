using kbs2.Faction.Enums;
using kbs2.Faction.Interfaces;
using kbs2.WorldEntity.Unit.MVC;
using System.Collections.Generic;
using System.Linq;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.ResourceFactory;

namespace kbs2.Faction.FactionMVC
{
    public class FactionModel : IHasFactionUnit, IHasFactionBuilding
    {
        public string Name { get; set; }
        
        public Dictionary<FactionModel, Faction_Relations> FactionRelationships { get; set; }

        public List<IWorldEntity> Entities { get; } = new List<IWorldEntity>();

        public List<UnitController> Units { get; set; }

        public List<IStructure<IStructureDef>> Buildings { get; set; }

        public IEnumerable<ResourceFactoryController> ResourceFactories => from building in Buildings
            where building is ResourceFactoryController
            select (ResourceFactoryController) building;


        public FactionModel(string name)
        {
            Name = name;
            FactionRelationships = new Dictionary<FactionModel, Faction_Relations>();
            Units = new List<UnitController>();
            Buildings = new List<IStructure<IStructureDef>>();
        }
    }
}