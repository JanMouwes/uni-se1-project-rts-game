using kbs2.Actions.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.Actions.ActionTabActions
{
    public class SelectMapAction_GameAction : IGameAction
    {
        public event TabActionDelegate Clicked;

        public ViewValues IconValues { get; set; }
        public void InvokeClick() => Clicked?.Invoke();

        public SelectMapAction_GameAction(GameActionSelector.MapActionSelector selector, IMapAction mapAction, ViewValues iconValues)
        {
            IconValues = iconValues;
            Clicked += () =>
            {
                if (mapAction == null) return;

                selector.Select(mapAction);
            };
        }
    }
}