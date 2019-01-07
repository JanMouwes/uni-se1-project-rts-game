using kbs2.WorldEntity.Interfaces;

namespace kbs2.Actions.GameActionDefs
{
    public class TrainActionDef : MapActionDef
    {
        public virtual ISpawnableDef SpawnableDef { get; }

        public ITrainingEntity Parent;

        public uint TrainingTime;

        public TrainActionDef(uint cooldown, uint trainingTime, string imageSource, ISpawnableDef spawnableDef) : base(cooldown, imageSource)
        {
            SpawnableDef = spawnableDef;
            
            TrainingTime = trainingTime;
        }
    }
}