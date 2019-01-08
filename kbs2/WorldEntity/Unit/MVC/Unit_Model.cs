using System.Collections.Generic;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.Faction.Interfaces;
using kbs2.GamePackage.AIPackage.Enums;
using kbs2.Unit;
using kbs2.Unit.Interfaces;
using kbs2.World.Enums;
using kbs2.WorldEntity.Health;

namespace kbs2.Unit.Model
{
    public class Unit_Model : IHasPersonalSpace, IPurchasable, IElemental, IFactionMember, IHasCommand
    {
        public Faction_Controller Faction { get; set; }

        public UnitDef Def { get; set; }

        public bool Selected { get; set; }

        public int TriggerRadius { get; set; }
        public int ChaseRadius { get; set; }

        public Command Order { get; set; }
        public IHasPersonalSpace Target { get; set; }
        public bool FinishedOrder { get; set; }

        public double Cost { get; set; }
        public double UpkeepCost { get; set; }

        public List<TerrainType> UnwalkableTerrain { get; set; }

        public List<ElementType> ElementTypes { get; set; }

        public CostValue CostValue { get; set; }

        public float Speed { get; set; }
        
        public string Name { get; set; }

        public List<IGameAction> Actions { get; set; }

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
            Order = Command.Idle;
        }
    }
}