using System.Collections.Generic;
using kbs2.Actions.GameActionGrid;
using kbs2.GamePackage;

namespace kbs2.UserInterface.GameActionGui
{
    public class GameActionGuiController
    {
        private GameActionGuiView view;
        private readonly GameActionGuiModel model;


        public GameActionGuiController(GameController gameController)
        {
            model = new GameActionGuiModel
            {
                GameController = gameController
            };
            view = new GameActionGuiView();
        }

        // switch to next group of nine
        public void Next()
        {
            if (model.CurrentTabIndex + 1 == model.TabModels.Count) return;

            model.CurrentTabIndex++;
        }

        // switch to previous group of nine
        public void Previous()
        {
            if (model.CurrentTabIndex == 0) return;

            model.CurrentTabIndex--;
        }

        public void SetActions(List<GameActionTabModel> gameActionTabModels)
        {
            model.TabModels.Clear();
            foreach (GameActionTabModel gameActionTabModel in gameActionTabModels)
            {
                model.TabModels.Add(gameActionTabModel);
            }
        }

        // remove all actions
        private void ClearActions() => model.TabModels.Clear();

        private void RemoveActions(GameActionTabModel tabModel) => model.TabModels.Remove(tabModel);
    }
}