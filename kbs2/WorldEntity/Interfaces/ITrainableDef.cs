using kbs2.Unit.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.WorldEntity.Interfaces
{
    public interface ITrainableDef : ISpawnableDef, IPurchasable
    {
        uint TrainingTime { get; }

        ViewValues IconData { get; }
    }
}