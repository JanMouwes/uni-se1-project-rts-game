using System.Collections.Generic;
using kbs2.World;
using kbs2.World.Enums;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.WorldEntity.Structures.Defs
{
    public class TrainingStructureDef : BuildingDef
    {
        public List<ITrainableDef> TrainableDefs;
    }
}