using System.Collections.Generic;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.BuildingMVC;
using kbs2.WorldEntity.Structures.Defs;

namespace kbs2.WorldEntity.Structures.TrainingStructure
{
    public class TrainingStructureModel : BuildingModel<TrainingStructureDef>
    {
        public Queue<ITrainable> TrainingQueue { get; set; } = new Queue<ITrainable>();
    }
}