using System.Collections.Generic;
using System.Linq;
using kbs2.Actions.GameActionGrid;
using kbs2.Actions.Interfaces;
using kbs2.GamePackage;
using kbs2.GamePackage.EventArgs;
using kbs2.World;
using Microsoft.Xna.Framework.Graphics;

namespace kbs2.UserInterface.GameActionGui
{
    public class GameActionGuiModel
    {
        public delegate void TabChangeEventDelegate(object sender, EventArgsWithPayload<int> eventArgs);

        public event TabChangeEventDelegate TabChange;

        #region Size constants

        public const float WIDTH_PERCENT = 15;
        public const float HEIGHT_PERCENT = 30;

        public const float LOCATION_X_PERCENT = 85;
        public const float LOCATION_Y_PERCENT = 70;

        #endregion

        public GraphicsDevice GraphicsDevice { get; }

        public Coords Coords => new Coords
        {
            x = (int) (GraphicsDevice.Viewport.Width * (LOCATION_X_PERCENT / 100)),
            y = (int) (GraphicsDevice.Viewport.Height * (LOCATION_Y_PERCENT / 100))
        };

        // List for actions in groups of nine
        public readonly List<GameActionTabModel> TabModels = new List<GameActionTabModel>();
        public GameActionTabModel CurrentTab => TabModels.Count > 0 ? TabModels[currentTabIndex] : null;

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

        public GameActionGuiModel(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }
    }
}