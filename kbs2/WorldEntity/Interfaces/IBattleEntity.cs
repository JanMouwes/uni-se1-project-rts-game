using kbs2.GamePackage.EventArgs;
using kbs2.Unit;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Structs;

namespace kbs2.WorldEntity.Interfaces
{
    public delegate void OnTakeHitDelegate(object sender, EventArgsWithPayload<HitValues> eventArgs);
    
    public interface IBattleEntity
    {
        HealthValues HealthValues { get; }

        event OnTakeHitDelegate OnTakeHit;

        void TakeHit(HitValues hitValues);
    }
}