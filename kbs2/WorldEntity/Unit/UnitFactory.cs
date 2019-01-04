using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.Unit.Model;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Location;
using kbs2.WorldEntity.Location.LocationMVC;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.WorldEntity.Unit
{
    public class UnitFactory : IDisposable
    {
        private Faction_Controller faction;
        private GameController game;

        public static UnitController CreateNewUnit(UnitDef def, FloatCoords topLeft, WorldController world, Faction_Controller factionController)
        {
            UnitController unitController = new UnitController(def)
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

            Location_Controller location = new Location_Controller(world, topLeft.x, topLeft.y)
            {
                LocationModel =
                {
                    Parent = unitController
                }
            };
            unitController.LocationController = location;
            return unitController;
        }

        public UnitFactory(Faction_Controller faction, GameController game)
        {
            this.faction = faction;
            this.game = game;
        }

        public UnitController CreateNewUnit(UnitDef def)
        {
            return CreateNewUnit(def, new FloatCoords(), game.GameModel.World, faction);
        }

        public void Dispose()
        {
            faction = null;
            game = null;
        }
    }
}