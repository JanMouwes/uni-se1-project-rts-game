using System;
using kbs2.Faction.FactionMVC;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.BuildingMVC;
using kbs2.WorldEntity.Structures.Defs;
using kbs2.WorldEntity.Structures.ResourceFactory;

namespace kbs2.WorldEntity.Structures
{
    public class BuildingFactory : IDisposable
    {
        private Faction_Controller faction;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="faction">Factory's faction</param>
        /// <exception cref="ArgumentNullException">Thrown when faction is null</exception>
        public BuildingFactory(Faction_Controller faction)
        {
            this.faction = faction ?? throw new ArgumentNullException(nameof(faction));
        }

        /// <summary>
        /// Creates new structure according to def belonging to factory's faction
        /// </summary>
        /// <param name="def">Definition of structure to be created</param>
        /// <returns>Structure defined by Def belonging to factory's faction</returns>
        public IStructure<IStructureDef> CreateNewBuilding(IStructureDef def)
        {
            IStructure<IStructureDef> buildingController;

            switch (def)
            {
                case ResourceFactoryDef resourceFactoryDef:
                    buildingController = new ResourceFactoryController(resourceFactoryDef, faction);
                    break;
                default:
                    buildingController = new BuildingController(def);
                    buildingController.Faction = faction;
                    break;
            }

            return buildingController;
        }

        public void Dispose()
        {
            faction = null;
        }
    }
}