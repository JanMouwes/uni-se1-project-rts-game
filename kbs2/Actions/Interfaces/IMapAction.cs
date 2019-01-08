using System.Collections.Generic;
using kbs2.GamePackage.EventArgs;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.Actions.Interfaces
{
    public interface IMapAction
    {
        uint CurrentCooldown { get; set; }

        /// <summary>
        /// Execute action on the selected target
        /// </summary>
        /// <param name="target">Selected target</param>
        /// <returns>Whether the action was successful</returns>
        bool TryExecute(ITargetable target);

        bool IsValidTarget(ITargetable targetable);

        void Update(object sender, OnTickEventArgs eventArgs);

        ViewValues IconValues { get; }

        List<MapActionAnimationItem> GetAnimationItems(FloatCoords from, FloatCoords to);
    }
}