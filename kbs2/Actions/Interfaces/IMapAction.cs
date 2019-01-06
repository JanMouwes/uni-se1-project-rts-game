using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.GamePackage.EventArgs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.Actions.Interfaces
{
    public interface IMapAction
    {
        uint CurrentCooldown { get; set; }

        void Execute(ITargetable target);

        void Update(object sender, OnTickEventArgs eventArgs);
        
        ViewValues IconValues { get; }
    }
}