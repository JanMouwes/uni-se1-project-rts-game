using System.Collections.Generic;
using kbs2.Actions.MapActionDefs;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.Actions.GameActionDefs
{
    public class SpawnActionDef : MapActionDef
    {
        //TEMP
        public static SpawnActionDef Raichu
        {
            get
            {
                DBController.OpenConnection("DefDex.db");
                SpawnActionDef def = new SpawnActionDef(10, "raichu_idle", DBController.GetUnitDef(1));
                DBController.CloseConnection();
                return def;
            }
        }

        public static SpawnActionDef Pikachu
        {
            get
            {
                DBController.OpenConnection("DefDex.db");
                SpawnActionDef def = new SpawnActionDef(10, "pikachu_idle", DBController.GetUnitDef(2));
                DBController.CloseConnection();
                return def;
            }
        }
        //TEMP

        public virtual ISpawnableDef SpawnableDef { get; set; }

        public SpawnActionDef(uint cooldown, string imageSource, ISpawnableDef spawnableDef) : base(cooldown, new ViewValues(imageSource, 20, 20))
        {
            this.SpawnableDef = spawnableDef;
        }

        public override List<MapActionAnimationItem> GetAnimationItems(FloatCoords @from, FloatCoords to) => new List<MapActionAnimationItem>();
    }
}