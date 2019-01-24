using System;
using System.Collections.Generic;
using kbs2.Actions;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;
using kbs2.Actions.MapActionDefs;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.GamePackage.Interfaces;
using kbs2.Unit;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Location.LocationMVC;
using kbs2.WorldEntity.Structs;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework;

namespace kbs2.WorldEntity.Unit
{
    public class UnitFactory : IDisposable
    {
        private Faction_Controller faction;
        private GameController game;

        private static UnitController CreateNewUnit(UnitDef def, FloatCoords topLeft, WorldController world, Faction_Controller factionController)
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
                    Faction = factionController,
                    Def = def,
                    Name = def.Name
                },
                HPController =
                {
                    HPModel =
                    {
                        CurrentHP = (int)def.HPDef.CurrentHP,
                        MaxHP = (int)def.HPDef.MaxHP
                    }
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

            //FIXME temp. This needs to be heavily refactored, but works for now

            #region TEMP_REGION

            GameActionFactory factory = new GameActionFactory(game);

            MapActionFactory mapActionFactory = new MapActionFactory(faction, game);

            AttackActionDef attackActionDef = new AttackActionDef(3, new ViewValues("mapaction-lightningbolt", 20, 20), 20, ElementType.Electric);

            IMapAction mapAction = mapActionFactory.CreateAttackAction(attackActionDef, unit);
            IGameAction gameAction = factory.CreateSelectAction(mapAction);

            unit.GameActions.Add(gameAction);

            unit.OnTakeHit += (sender, args) =>
            {
                UnitController unitSender = (UnitController) sender;

                Random random = new Random();

                const int offsetMargin = 75;
                const int timeOffsetMargin = 5;

                FloatCoords offset = new FloatCoords()
                {
                    x = random.Next(-offsetMargin, offsetMargin) / 100f,
                    y = -1 + random.Next(-offsetMargin, offsetMargin) / 100f
                };

                MapActionAnimationText animationText = new MapActionAnimationText(unitSender.Center + offset, $"-{Math.Round(args.Value.Damage * args.Value.BattleModifiers.AttackModifier, 1)}", "Currency", 0)
                {
                    Colour = Color.Red
                };
                game.AnimationController.AddAnimation_ByFrames(new List<IViewItem>()
                    {
                        animationText
                    }, 30 + random.Next(-timeOffsetMargin, +timeOffsetMargin));
            };

            #endregion

            unit.LocationController.chunkChanged += game.LoadNewChunks;
            return unit;
        }

        //    TODO rewrite. this is risky
        public ISpawnable CreateNewSpawnable(ISpawnableDef def)
        {
            return CreateNewUnit((UnitDef) def);
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