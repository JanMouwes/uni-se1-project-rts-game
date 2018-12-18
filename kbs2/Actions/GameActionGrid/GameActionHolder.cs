using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;

namespace kbs2.Actions.GameActionGrid
{
    public class GameActionHolder
    {
        public IGameAction GameAction { get; }

        public GameActionHolder(IGameAction gameAction)
        {
            this.GameAction = gameAction;
        }
    }
}