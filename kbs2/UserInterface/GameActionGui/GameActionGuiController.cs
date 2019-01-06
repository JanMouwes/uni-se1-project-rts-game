using System.Collections.Generic;
using System.Linq;
using kbs2.Actions.GameActionGrid;
using kbs2.Actions.Interfaces;
using kbs2.GamePackage;
using kbs2.GamePackage.Interfaces;

namespace kbs2.UserInterface.GameActionGui
{
    public class GameActionGuiController
    {
        public readonly GameActionGuiView View;
        private readonly GameActionGuiModel model;


        public GameActionGuiController(GameController gameController)
        {
            model = new GameActionGuiModel(gameController.GraphicsDevice)
            {
                GameController = gameController
            };
            View = new GameActionGuiView(model);
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

        public void SetActions(IEnumerable<GameActionTabModel> gameActionTabModels)
        {
            ClearActions();
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