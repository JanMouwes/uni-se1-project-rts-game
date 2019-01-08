﻿using System.Collections.Generic;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.Faction.Interfaces;
using kbs2.Unit;
using kbs2.Unit.Interfaces;
using kbs2.World.Enums;
using kbs2.WorldEntity.Health;

namespace kbs2.WorldEntity.Unit.MVC
{
    public class Unit_Model : IHasPersonalSpace, IElemental, IFactionMember
    {
        public Faction_Controller Faction { get; set; }

        public readonly UnitDef Def;

        public bool Selected { get; set; }

        public List<TerrainType> UnwalkableTerrain { get; set; }

        public List<ElementType> ElementTypes { get; set; }

        public float Speed { get; set; }

        public List<IGameAction> Actions => Def.GameActions;

        public HealthValues HealthValues { get; set; }

        public Unit_Model(UnitDef def)
        {
            Def = def;
            HealthValues = new HealthValues()
            {
                CurrentHP = def.MaxHealth,
                MaxHP = def.MaxHealth
            };
            Selected = false;
        }
    }
}