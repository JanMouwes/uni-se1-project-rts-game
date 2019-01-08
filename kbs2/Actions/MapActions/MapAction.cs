using System.Collections.Generic;
using kbs2.Actions.Interfaces;
using kbs2.GamePackage.EventArgs;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.Actions.MapActions
{
    public abstract class MapAction<TSenderDef, TActionDef> : IMapAction where TActionDef : IMapActionDef
    {
        public delegate void MapActionExecutedDelegate(TSenderDef sender, EventArgsWithPayload<FloatCoords> eventArgs);

        public event MapActionExecutedDelegate MapActionExecuted;

        public TActionDef ActionDef { get; }

        public uint CurrentCooldown { get; set; } = 0;

        public abstract bool TryExecute(ITargetable target);
        public abstract bool IsValidTarget(ITargetable targetable);

        protected void InvokeMapActionExecuted(TSenderDef sender, FloatCoords coords) => MapActionExecuted?.Invoke(sender, new EventArgsWithPayload<FloatCoords>(coords));

        private int previousCooldownUpdate;

        protected MapAction(TActionDef actionDef, ViewValues iconValues)
        {
            ActionDef = actionDef;
            IconValues = iconValues;
            CurrentCooldown = 0;
        }

        /// <summary>
        /// updates the cooldown of the actions
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="eventArgs">EventArgs in gametime</param>
        public virtual void Update(object sender, OnTickEventArgs eventArgs)
        {
            double thisSecond = eventArgs.GameTime.TotalGameTime.TotalSeconds;
            int thisSecondInt = (int) thisSecond;

            //    Update cooldown-time
            if (thisSecondInt <= previousCooldownUpdate || CurrentCooldown <= 0) return;
            previousCooldownUpdate = thisSecondInt;
            CurrentCooldown--;
        }

        protected bool CooldownPassed => CurrentCooldown <= 0;

        public ViewValues IconValues { get; }
        public abstract List<MapActionAnimationItem> GetAnimationItems(FloatCoords from, FloatCoords to);
    }
}