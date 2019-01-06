using kbs2.GamePackage.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.WorldEntity.Interfaces
{
    public interface ITrainableDef : ISpawnableDef
    {
        uint TrainingTime { get; }

        ViewValues IconData { get; }
    }
}