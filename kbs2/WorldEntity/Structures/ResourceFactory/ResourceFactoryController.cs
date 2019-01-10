using System.Collections.Generic;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
using kbs2.Resources.Enums;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.Defs;

namespace kbs2.WorldEntity.Structures.ResourceFactory
{
    public class ResourceFactoryController : IStructure<ResourceFactoryDef>
    {
        public Faction_Controller Faction { get; set; }

        public ResourceFactoryDef Def => Model.Def;
        private ResourceFactoryModel Model { get; } = new ResourceFactoryModel();

        /// <inheritdoc cref="View"/>
        public ResourceFactoryView ResourceFactoryView { get; }

        public Unit_Controller View => ResourceFactoryView;

        public List<WorldCellModel> OccupiedCells { get; } = new List<WorldCellModel>();

        public FloatCoords FloatCoords => (FloatCoords) StartCoords;

        public Coords StartCoords
        {
            get => Model.TopLeft;
            set => Model.TopLeft = value;
        }

        public FloatCoords Centre => new FloatCoords
        {
            x = StartCoords.x + Width / 2,
            y = StartCoords.y + Height / 2
        };

        public int ViewRange => Def.ViewRange;

        public float Width => Def.ViewValues.Width;
        public float Height => Def.ViewValues.Height;

        /// <summary>
        /// Amount of resources that this structure produces
        /// </summary>
        public float ResourceValue => 50; //FIXME calculate according to environment. Set to 50 for now
        
        /// <summary>
        /// Type of resources that this structure produces
        /// </summary>
        public ResourceType ResourceType => Def.ResourceType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="def">Structure's definition</param>
        /// <param name="faction">Structure's faction</param>
        public ResourceFactoryController(ResourceFactoryDef def, Faction_Controller faction)
        {
            Model.Def = def;
            Faction = faction;

            ResourceFactoryView = new ResourceFactoryView(Model);
        }

        public void Update(object sender, OnTickEventArgs eventArgs)
        {
        }

        public List<IGameAction> GameActions { get; } = new List<IGameAction>();
        public ViewMode ViewMode { set => Model.ViewMode = value; }
    }
}