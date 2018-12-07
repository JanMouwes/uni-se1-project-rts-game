using kbs2.WorldEntity.Battle;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.XP;
using System;
using System.Data;

namespace kbs2.Unit.Unit
{
    public class UnitDef
    {
        public float Speed;
        public BattleDef BattleDef { get; set; }
        public HPDef HPDef { get; set; }
        public LevelXPDef LevelXPDef { get; set; }

        public UnitDef()
		{

		}
    }
}
