using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;

namespace kbs2.Actions.GameActionGrid
{
    public class GameActionTabItem
    {
        public IGameAction GameAction { get; }

        public GameActionTabItem(IGameAction gameAction)
        {
            this.GameAction = gameAction;
        }
    }
}