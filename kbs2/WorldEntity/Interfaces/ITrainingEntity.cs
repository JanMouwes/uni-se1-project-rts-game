using System.Collections.Generic;
using kbs2.Faction.Interfaces;
using kbs2.World;

namespace kbs2.WorldEntity.Interfaces
{
    /// <summary>
    /// Class for Entities that can train ITrainables 
    /// </summary>
    public interface ITrainingEntity : IFactionMember
    {
        /// <summary>
        /// Time remaining for the current ITrainable
        /// </summary>
        int TimeRemaining { get; }
        
        /// <summary>
        /// Total time remaining for the current ITrainable plus sum of training-time for all ITrainables in the TrainingQueue
        /// </summary>
        int TotalTimeRemaining { get; }

        /// <summary>
        /// Position at which the ITrainable will spawn
        /// </summary>
        Coords SpawnPosition { get; }

        /// <summary>
        /// Current unit training
        /// </summary>
        ITrainable CurrentlyTraining { get; }

        /// <summary>
        /// Queue of ITrainables training
        /// </summary>
        Queue<ITrainable> TrainingQueue { get; set; }
        
        /// <summary>
        /// Max amount of units TrainingEntity can train
        /// </summary>
        uint QueueLimit { get; set; }
    }
}