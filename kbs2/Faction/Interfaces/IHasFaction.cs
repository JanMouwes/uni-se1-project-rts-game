using kbs2.Faction.FactionMVC;
using System;
namespace kbs2.Faction.Interfaces
{
    public interface IHasFaction
    {
        Faction_Model Faction { get; set; }
    }
}
