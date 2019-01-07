using System;
using kbs2.GamePackage.AIPackage.Enums;
using kbs2.Unit.Unit;

namespace kbs2.Unit.Interfaces
{
    public interface IHasPersonalSpace
    {
        int TriggerRadius { get; set; }
        int ChaseRadius { get; set; }
    }
}
