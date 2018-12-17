using kbs2.World;
using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using kbs2.Faction.FactionMVC;
using kbs2.Unit.Unit;
using kbs2.Unit.Interfaces;
using kbs2.Faction.Interfaces;
using kbs2.WorldEntity;
using kbs2.GamePackage.Interfaces;
using kbs2.World.Enums;
using Microsoft.Xna.Framework;
using kbs2.Actions;

namespace kbs2.Unit.Model
{
    public class Unit_Model : IHasPersonalSpace, IPurchasable, IElemental, IHasFaction
    {
        public Faction_Model Faction { get; set; }

		public bool Selected { get; set; }

        public List<TerrainType> UnwalkableTerrain { get; set; }

        public List<ElementType> Elementtypes { get; set; }

        public CostValue CostValue { get; set; }

        public float Speed { get; set; }

        public List<ActionController> actions { get; set; }

		public Unit_Model()
        {
            Selected = false;
            actions = new List<ActionController>();
        }
    }
}
