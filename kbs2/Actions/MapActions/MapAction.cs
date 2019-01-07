using kbs2.Actions.Interfaces;
using kbs2.GamePackage.EventArgs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.Actions.GameActions
{
    public abstract class MapAction<TActionDef> : IMapAction where TActionDef : IMapActionDef
    {
        protected TActionDef ActionDef { get; }

        public uint CurrentCooldown { get; set; }

        public abstract void Execute(ITargetable target);

        protected MapAction(TActionDef actionDef, ViewValues iconValues)
        {
            ActionDef = actionDef;
            IconValues = iconValues;
            CurrentCooldown = actionDef.CooldownTime;
        }

        /// <summary>
        /// updates the cooldown of the actions
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="eventArgs">EventArgs in gametime</param>
        public void Update(object sender, OnTickEventArgs eventArgs)
        {
            CurrentCooldown -= (uint) eventArgs.GameTime.ElapsedGameTime.TotalSeconds;
        }

        public ViewValues IconValues { get; }
    }
}