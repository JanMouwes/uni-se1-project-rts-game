using System.Collections.Generic;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Faction.Interfaces;
using kbs2.Unit.Interfaces;
using kbs2.World.Cell;

namespace kbs2.WorldEntity.Interfaces
{
    public interface IWorldEntity : IHasPersonalSpace, ISpawnable, ITargetable, IFactionMember
    {
        /// <summary>
        /// Method subscribed to central 'OnTick'-event
        /// </summary>
        /// <param name="sender">OnTick's sender</param>
        /// <param name="eventArgs">Event-payload</param>
        void Update(object sender, OnTickEventArgs eventArgs);
    }
}