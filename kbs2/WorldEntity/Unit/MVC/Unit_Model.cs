using System.Collections.Generic;
using kbs2.Faction.FactionMVC;
using kbs2.Unit.Interfaces;
using kbs2.Faction.Interfaces;
using kbs2.World.Enums;
using kbs2.Actions.Interfaces;

namespace kbs2.Unit.Model
{
    public class Unit_Model : IHasPersonalSpace, IPurchasable, IElemental, IFactionMember
    {
        public Faction_Controller Faction { get; set; }

        public bool Selected { get; set; }

        public List<TerrainType> UnwalkableTerrain { get; set; }

        public List<ElementType> ElementTypes { get; set; }

        public CostValue CostValue { get; set; }

        public float Speed { get; set; }

        public List<IGameAction> Actions { get; } = new List<IGameAction>();

        public Unit_Model()
        {
            Selected = false;
        }
    }
}