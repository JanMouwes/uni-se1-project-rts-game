using System;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Interfaces;
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
                    Texture = def.Image, Width = def.Width, Height = def.Height, ViewMode = ViewMode.Full
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
            UnitController unit = CreateNewUnit(def, new FloatCoords(), game.GameModel.World, faction);
            unit.LocationController.chunkChanged += game.LoadNewChunks;
            return unit;
        }

        //    TODO rewrite. this is risky
        public ISpawnable CreateNewSpawnable(ISpawnableDef def)
        {
            return CreateNewUnit((UnitDef) def, new FloatCoords(), game.GameModel.World, faction);
        }

        public ITrainable CreateNewTrainable(ITrainableDef def)
        {
            return CreateNewSpawnable(def) as ITrainable;
        }

        public void Dispose()
        {
            faction = null;
            game = null;
        }
    }
}