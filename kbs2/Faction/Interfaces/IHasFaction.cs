using System;
namespace kbs2.Faction.Interfaces
{
    public interface IHasFaction
    {
        bool IsHostile(IHasFaction faction);
    }
}
