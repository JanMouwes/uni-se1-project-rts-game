using kbs2.Actions.Interfaces;
using kbs2.View.GUI;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.Actions.GameActionGrid
{
    public class GameActionTabItem : GuiViewImage
    {
        private IGameAction GameAction { get; }

        public GameActionTabItem(IGameAction gameAction, FloatCoords location) : base(location, gameAction.IconValues)
        {
            this.GameAction = gameAction;
        }

        public override void Click() => GameAction.InvokeClick();
    }
}