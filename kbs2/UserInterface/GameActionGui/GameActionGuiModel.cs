using System.Collections.Generic;
using kbs2.Actions.GameActionGrid;
using kbs2.GamePackage;
using kbs2.GamePackage.EventArgs;

namespace kbs2.UserInterface.GameActionGui
{
    public class GameActionGuiModel
    {
        public delegate void TabChangeEventDelegate(object sender, EventArgsWithPayload<int> eventArgs);

        public event TabChangeEventDelegate TabChange;

        // List for actions in groups of nine
        public readonly List<GameActionTabModel> TabModels = new List<GameActionTabModel>();
        public GameActionTabModel CurrentTab => TabModels[currentTabIndex];

        public GameController GameController { get; internal set; }

        private int currentTabIndex;

        // index for the groups of nine
        public int CurrentTabIndex
        {
            get => currentTabIndex;
            set
            {
                currentTabIndex = value;
                TabChange?.Invoke(this, new EventArgsWithPayload<int>(currentTabIndex));
            }
        }

        public GameActionGuiModel()
        {
        }
    }
}