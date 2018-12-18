namespace kbs2.Actions.Interfaces
{
    public interface IGameActionDef
    {
        uint Cooldown { get; }

        string ImageSource { get; }
    }
}