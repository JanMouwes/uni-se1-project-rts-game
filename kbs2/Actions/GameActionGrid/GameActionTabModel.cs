using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;
using kbs2.UserInterface.GameActionGui;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Structs;
using Microsoft.Xna.Framework.Graphics;

namespace kbs2.Actions.GameActionGrid
{
    public class GameActionTabModel
    {
        public const int COLUMNS = 3;
        public const int ROWS = 3;

        public const int GAME_ACTIONS_PER_TAB = ROWS * COLUMNS;

        private int guiWidth => (int) parent.View.Width;

        /// <summary>
        /// Times the size of the moats in between the items
        /// </summary>
        private const float ITEM_WEIGHT = 2;

        /// <summary>
        /// ITEM_WEIGHT times as big as the 'moats' in between
        /// </summary>
        private int ItemWidth => (int) (guiWidth / (COLUMNS * (ITEM_WEIGHT + 1) + 1) * ITEM_WEIGHT);

        private int MoatSize => (int) (guiWidth / (COLUMNS * (ITEM_WEIGHT + 1) + 1));

        private GameActionGuiController parent;

        public GameActionTabItem[] GameActionTabItems { get; }

        public GameActionTabModel(IGameAction[] gameActions, GameActionGuiController parent)
        {
            this.parent = parent;

            GameActionTabItems = new GameActionTabItem[GAME_ACTIONS_PER_TAB];

            int column = 0;
            int row = 0;

            for (int i = 0; i < gameActions.Length; i++)
            {
                IGameAction gameAction = gameActions[i];
                FloatCoords location = new FloatCoords()
                {
                    x = column * (ItemWidth + MoatSize) + MoatSize,
                    y = row * (ItemWidth + MoatSize) + MoatSize
                };

                location += parent.View.Coords;

                ViewValues viewValues = new ViewValues(gameAction.IconValues.Image, ItemWidth, ItemWidth);
                gameAction.IconValues = viewValues;

                GameActionTabItems[i] = new GameActionTabItem(gameAction, location);
                column++;

                if (column < COLUMNS) continue;

                column = 0;
                row++;
            }
        }
    }
}