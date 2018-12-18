using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;

namespace kbs2.Actions.GameActionGrid
{
    public class GameActionTabModel
    {
        public const int GameActionsPerTab = 9;

        public GameActionHolder[] GameActionHolders { get; private set; }

        public GameActionTabModel(IGameAction[] gameActions)
        {
            GameActionHolders = new GameActionHolder[GameActionsPerTab];
            for (int i = 0; i < gameActions.Length; i++)
            {
                GameActionHolders[i] = new GameActionHolder(gameActions[i]);
            }
        }
    }
}