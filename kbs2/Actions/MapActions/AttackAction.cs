using System;
using System.Collections.Generic;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActionGrid;
using kbs2.Actions.GameActions;
using kbs2.Actions.MapActionDefs;
using kbs2.GamePackage.EventArgs;
using kbs2.utils;
using kbs2.Unit;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework;

namespace kbs2.Actions.MapActions
{
    public class AttackAction : MapAction<UnitController, AttackActionDef>, IUpdateable
    {
        public AttackAction(AttackActionDef actionDef, ViewValues iconValues, UnitController sender) : base(actionDef, iconValues)
        {
            this.sender = sender;
        }

        private readonly UnitController sender;

        public override bool TryExecute(ITargetable target)
        {
            if (!(IsValidTarget(target)) || !CooldownPassed) return false;

            IBattleEntity battleEntity = (IBattleEntity) target;

            HitValues hitValues = new HitValues(ActionDef.BaseDamage, new BattleModifiers(1.0f, ActionDef.ElementType));

            base.InvokeMapActionExecuted(sender, target.FloatCoords);

            battleEntity.TakeHit(hitValues);

            return true;
        }

        public override bool IsValidTarget(ITargetable targetable) => targetable is IBattleEntity;

        public override List<MapActionAnimationItem> GetAnimationItems(FloatCoords from, FloatCoords to)
        {
            return new List<MapActionAnimationItem>()
            {
                new MapActionAnimationItem(from, 1, (float) DistanceCalculator.DiagonalDistance(@from, to), ActionDef.Icon.Image, DistanceCalculator.DegreesFromCoords(from, to))
            };
        }

        public override void Update(object sender, OnTickEventArgs eventArgs)
        {
            base.Update(sender, eventArgs);
        }


        //TODO use these in favour of onTick;
        public void Update(GameTime gameTime)
        {
        }

        public bool Enabled { get; }
        public int UpdateOrder { get; }
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
    }
}