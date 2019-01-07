using System;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.Unit.Model;
using kbs2.Unit.Unit;
using kbs2.World;
using kbs2.World.Enums;
using kbs2.World.Structs;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Location;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework;

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
                    Speed = 0.05f,
                    Name = def.Name,
                    //Faction = faction             Uncomment when merge has happened DO NOT REMOVE
                },
                HPController = new HP_Controller
                {
                    HPModel = new HP_Model
                    {
                        CurrentHP = def.HPDef.CurrentHP,
                        MaxHP = def.HPDef.MaxHP
                    }
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