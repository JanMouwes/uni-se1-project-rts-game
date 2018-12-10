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
        private List<ActionView[]> currentActions;
        public GameController gameController { get; set; }
        public Coords Corner => new Coords { x= gameController.GraphicsDevice.Viewport.Width, y = gameController.GraphicsDevice.Viewport.Height };
        public int actionIndex;

        public const int ButtonSize = 40;
        public const int ButtonSpace = 5;
        public Coords[] ButtonCoords = new Coords[9] {
            new Coords{x = (ButtonSize + ButtonSpace)*3, y = (ButtonSize + ButtonSpace)*3},
            new Coords{x = (ButtonSize + ButtonSpace)*2, y = (ButtonSize + ButtonSpace)*3},
            new Coords{x = (ButtonSize + ButtonSpace)*1, y = (ButtonSize + ButtonSpace)*3},
            new Coords{x = (ButtonSize + ButtonSpace)*3, y = (ButtonSize + ButtonSpace)*2},
            new Coords{x = (ButtonSize + ButtonSpace)*2, y = (ButtonSize + ButtonSpace)*2},
            new Coords{x = (ButtonSize + ButtonSpace)*1, y = (ButtonSize + ButtonSpace)*2},
            new Coords{x = (ButtonSize + ButtonSpace)*3, y = (ButtonSize + ButtonSpace)*1},
            new Coords{x = (ButtonSize + ButtonSpace)*2, y = (ButtonSize + ButtonSpace)*1},
            new Coords{x = (ButtonSize + ButtonSpace)*1, y = (ButtonSize + ButtonSpace)*1}
        };

        public ActionInterface(GameController gameController)
        {
            this.gameController = gameController;
            currentActions = new List<ActionView[]>();
        }

        public void Next()
        {
            if(actionIndex+1 < currentActions.Count)
            {
                RemoveActions(actionIndex);
                actionIndex++;
                SetActions(actionIndex);
            }
        }

        public void Previous()
        {
            if (actionIndex < 0)
            {
                RemoveActions(actionIndex);
                actionIndex--;
                SetActions(actionIndex);
            }
        }


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
                        hasActions.Actions[(i * 9 + j)].View.Coords = (FloatCoords)(Corner - ButtonCoords[j]);
                        hasActions.Actions[(i * 9 + j)].View.Height = ButtonSize;
                        hasActions.Actions[(i * 9 + j)].View.Width = ButtonSize;
                        actionViews[j] = hasActions.Actions[(i * 9 + j)].View;
                    }
                }
                currentActions.Add(actionViews);
            }
            SetActions(0);
        }

        private void SetActions(int index)
        {
            foreach (ActionView actionView in currentActions[index])
            {
                gameController.gameModel.GuiItemList.Add(actionView);
            }
        }

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
