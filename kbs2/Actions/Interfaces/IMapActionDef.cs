using System.Collections.Generic;
using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using kbs2.WorldEntity.Structs;

namespace kbs2.Actions.Interfaces
{
    public interface IMapActionDef
    {
        uint CooldownTime { get; }

        ViewValues Icon { get; }


    }
}