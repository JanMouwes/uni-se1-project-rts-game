using System.Collections.Generic;
using kbs2.Actions.Interfaces;
using kbs2.GamePackage.Interfaces;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Structs;

namespace kbs2.Actions.MapActionDefs
{
    public abstract class MapActionDef : IMapActionDef
    {
        public uint CooldownTime { get; }

        public ViewValues Icon { get; }

        public abstract List<MapActionAnimationItem> GetAnimationItems(FloatCoords from, FloatCoords to);

        protected MapActionDef(uint cooldown, ViewValues imageSource)
        {
            CooldownTime = cooldown;
            Icon = imageSource;
        }
    }
}