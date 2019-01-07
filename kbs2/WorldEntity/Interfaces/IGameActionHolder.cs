using System.Collections.Generic;
using kbs2.Actions.Interfaces;

namespace kbs2.WorldEntity.Interfaces
{
    public interface IGameActionHolder
    {
        List<IGameAction> GameActions { get; }
    }
}