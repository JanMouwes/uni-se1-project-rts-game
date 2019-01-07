using kbs2.WorldEntity.Structs;

namespace kbs2.Actions.Interfaces
{
    public delegate void TabActionDelegate();

    public interface IGameAction
    {
        event TabActionDelegate Clicked;

        ViewValues IconValues { get; set; }

        void InvokeClick();
    }
}