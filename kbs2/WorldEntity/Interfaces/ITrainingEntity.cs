using System.Collections.Generic;
using kbs2.Faction.Interfaces;
using kbs2.World;

namespace kbs2.WorldEntity.Interfaces
{
    public interface ITrainingEntity : IFactionMember
    {
        int TimeRemaining { get; }
        int TotalTimeRemaining { get; }

        Coords SpawnPosition { get; }

        ITrainable CurrentlyTraining { get; }

        Queue<ITrainable> TrainingQueue { get; set; }
        uint QueueLimit { get; set; }
    }
}