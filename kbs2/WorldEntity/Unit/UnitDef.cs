using kbs2.WorldEntity.Battle;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;
using kbs2.WorldEntity.XP;

namespace kbs2.WorldEntity.Unit
{
    public class UnitDef : IWorldEntityDef
    {
        public float Speed;
        public string Image { get; set; }
        public float Width;
        public float Height;
        public BattleDef BattleDef { get; set; }
        public HPDef HPDef { get; set; }
        public LevelXPDef LevelXPDef { get; set; }
        public ViewValues ViewValues => new ViewValues(Image, Width, Height);
    }
}