using kbs2.Actions.GameActionDefs;
using kbs2.Actions.Interfaces;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.Actions.GameActions
{
    public abstract class GameAction<TGameActionDef> : IGameAction where TGameActionDef : IGameActionDef
    {
        protected TGameActionDef ActionDef { get; }
        
        public uint CurrentCooldown { get; set; }

        public abstract void Execute(ITargetable target);

        protected GameAction(TGameActionDef actionDef)
        {
            ActionDef = actionDef;
            CurrentCooldown = actionDef.Cooldown;
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
    }
}