using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;

namespace kbs2.Actions.GameActionGrid
{
    public class GameActionTabModel
    {
        public const int GameActionsPerTab = 9;

        public GameActionTabItem[] GameActionTabItems { get; private set; }

        public GameActionTabModel(IGameAction[] gameActions)
        {
            GameActionTabItems = new GameActionTabItem[GameActionsPerTab];
            for (int i = 0; i < gameActions.Length; i++)
            {
                GameActionTabItems[i] = new GameActionTabItem(gameActions[i]);
            }
        }
    }
}