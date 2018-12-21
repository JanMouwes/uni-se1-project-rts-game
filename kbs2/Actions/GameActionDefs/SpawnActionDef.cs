using kbs2.Actions.Interfaces;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit;

namespace kbs2.Actions.GameActionDefs
{
    public class SpawnActionDef : GameActionDef
    {
        //TEMP
        public static SpawnActionDef Raichu
        {
            get
            {
                DBController.OpenConnection("DefDex.db");
                SpawnActionDef def = new SpawnActionDef(10, "raichu_idle", DBController.GetDefinitionFromUnit(1));
                DBController.CloseConnection();
                return def;
            }
        }

        public static SpawnActionDef Pikachu
        {
            get
            {
                DBController.OpenConnection("DefDex.db");
                SpawnActionDef def = new SpawnActionDef(10, "pikachu_idle", DBController.GetDefinitionFromUnit(2));
                DBController.CloseConnection();
                return def;
            }
        }
        //TEMP

        public virtual ISpawnableDef SpawnableDef { get; }

        public SpawnActionDef(uint cooldown, string imageSource, ISpawnableDef spawnableDef) : base(cooldown,
            imageSource)
        {
            SpawnableDef = spawnableDef;
        }
    }
}