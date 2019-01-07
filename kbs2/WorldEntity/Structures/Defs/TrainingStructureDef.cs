using System.Collections.Generic;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.WorldEntity.Structures.Defs
{
    public class TrainingStructureDef : BuildingDef
    {
        public List<ITrainableDef> TrainableDefs;
    }
}