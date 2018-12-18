using kbs2.Actions.Interfaces;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit;

namespace kbs2.Actions.GameActionDefs
{
    public class SpawnActionDef : GameActionDef
    {
        //TEMP
        public static SpawnActionDef Raichu => new SpawnActionDef(10, "raichu_idle", new UnitDef());
        public static SpawnActionDef Pikachu => new SpawnActionDef(10, "pikachu_idle", new UnitDef());
        //TEMP

        public ISpawnableDef SpawnableDef { get; }

        public SpawnActionDef(uint cooldown, string imageSource, ISpawnableDef spawnableDef) : base(cooldown,
            imageSource)
        {
            SpawnableDef = spawnableDef;
        }
    }
}