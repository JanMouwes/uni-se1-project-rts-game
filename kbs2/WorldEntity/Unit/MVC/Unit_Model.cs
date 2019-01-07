using System.Collections.Generic;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.Faction.Interfaces;
using kbs2.Unit;
using kbs2.Unit.Interfaces;
using kbs2.World.Enums;

namespace kbs2.Unit.Model
{
    public class Unit_Model : IPurchasable, IElemental, IHasFaction
    {
        public Faction_Model Faction { get; set; }

		public bool Selected { get; set; }

        public List<TerrainType> UnwalkableTerrain { get; set; }

        public List<ElementType> Elementtypes { get; set; }

        public CostValue CostValue { get; set; }

        public float Speed { get; set; }
        
        public string Name { get; set; }

        public List<ActionController> actions { get; set; }

        public Unit_Model()
        {
            Selected = false;
            actions = new List<ActionController>();
        }
    }
}
