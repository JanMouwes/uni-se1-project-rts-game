using kbs2.Actions;
using kbs2.Actions.ActionMVC;
using kbs2.GamePackage;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.UserInterface
{
    public class ActionInterface
    {
        // List for actions in groups of nine
        private List<ActionView[]> currentActions;
        public GameController gameController { get; set; }
        
        // index for the groups of nine
        public int actionIndex;

        public ActionInterface(GameController gameController)
        {
            this.gameController = gameController;
            currentActions = new List<ActionView[]>();
        }

        // switch to next group of nine
        public void Next()
        {
            if(actionIndex+1 < currentActions.Count)
            {
                RemoveActions(actionIndex);
                actionIndex++;
                SetActions(actionIndex);
            }
        }

        // switch to previous group of nine
        public void Previous()
        {
            if (actionIndex < 0)
            {
                RemoveActions(actionIndex);
                actionIndex--;
                SetActions(actionIndex);
            }
        }

        // set all actions from IHasActions in CurrentActions
        public void SetActions(IHasActions hasActions)
        {
            RemoveActions();
            int amount = (int)Math.Ceiling((hasActions.Actions.Count) / 9f);
            actionIndex = 0;

            for (int i = 0; i < amount; i++)
            {
                ActionView[] actionViews = new ActionView[9];
                for(int j =0; j < 9; j++)
                {
                    if(hasActions.Actions.Count > i * 9 + j)
                    {
                        hasActions.Actions[(i * 9 + j)].View.index = j;
                        
                        actionViews[j] = hasActions.Actions[(i * 9 + j)].View;
                    }
                }
                currentActions.Add(actionViews);
            }
            if (currentActions.Count > 0)
            {
                SetActions(0);
            }
            
        }

        // Set actiongroup on screen
        private void SetActions(int index)
        {
            foreach (ActionView actionView in currentActions[index])
            {
                gameController.gameModel.GuiItemList.Add(actionView);
            }
        }

        // remove all actions
        private void RemoveActions()
        {
            foreach (ActionView[] actionViews in currentActions)
            {
                foreach(ActionView actionView in actionViews)
                {

                    if (gameController.gameModel.GuiItemList.Contains(actionView))
                    {
                        gameController.gameModel.GuiItemList.Remove(actionView);
                    }
                }
            }
            currentActions.Clear();
        }

        // remove group of nine from screen
        private void RemoveActions(int index)
        {
            foreach (ActionView actionView in currentActions[index])
            {
                if (gameController.gameModel.GuiItemList.Contains(actionView))
                {
                    gameController.gameModel.GuiItemList.Remove(actionView);
                }
            }
        }
    }
}
