using kbs2.WorldEntity.Interfaces;

namespace kbs2.Actions.Interfaces
{
    public interface IGameAction
    {
        uint CurrentCooldown { get; set; }


        void Execute(ITargetable target);
    }
}