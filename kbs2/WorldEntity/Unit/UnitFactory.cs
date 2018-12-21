using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Faction.FactionMVC;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Location;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.WorldEntity.Unit
{
    public class UnitFactory : IDisposable
    {
        private Faction_Controller faction;

        public static UnitController CreateNewUnit(UnitDef def, FloatCoords TopLeft, WorldModel worldModel, Faction_Controller factionController)
        {
            UnitController unitController = new UnitController
            {
                UnitView =
                {
                    Texture = def.Image, Width = def.Width, Height = def.Height
                },
                UnitModel =
                {
                    Speed = def.Speed,
                    Faction = factionController
                }
            };

            Location_Controller location = new Location_Controller(worldModel, TopLeft.x, TopLeft.y)
            {
                LocationModel =
                {
                    Parent = unitController
                }
            };
            unitController.LocationController = location;
            return unitController;
        }

        public UnitFactory(Faction_Controller faction)
        {
            this.faction = faction;
        }

        public UnitController CreateNewUnit(UnitDef def, WorldModel worldModel)
        {
            return CreateNewUnit(def, new FloatCoords(), worldModel, faction);
        }

        public void Dispose()
        {
        }
    }
}