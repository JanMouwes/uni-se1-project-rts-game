using System.Collections.Generic;
using kbs2.Actions.ActionMVC;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.Interfaces;

namespace kbs2.WorldEntity.Interfaces
{
    public interface IHasGameActions
    {
        List<IGameAction> GameActions { get; }
    }
}