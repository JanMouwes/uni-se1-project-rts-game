using kbs2.Actions.Interfaces;

namespace kbs2.Actions.GameActionSelector
{
    public class MapActionSelector
    {
        public IMapAction SelectedMapAction { get; private set; }

        public bool IsMapActionSelected() => SelectedMapAction != null;

        public bool IsMapActionSelected(IMapAction mapAction) => SelectedMapAction == mapAction;

        public void Clear() => SelectedMapAction = null;

        public void Select(IMapAction mapAction)
        {
            SelectedMapAction = mapAction;
        }
    }
}