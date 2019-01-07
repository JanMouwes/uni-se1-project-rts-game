using kbs2.Actions.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.Actions.GameActions
{
    public class GameAction : IGameAction
    {
        public event TabActionDelegate Clicked;

        public ViewValues IconValues { get; set; }
        
        public void InvokeClick() => Clicked?.Invoke();

        public GameAction(ViewValues iconValues)
        {
            IconValues = iconValues;
        }
    }
}