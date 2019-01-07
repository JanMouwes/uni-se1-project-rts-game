using kbs2.WorldEntity.Structures.BuildingMVC;
using kbs2.WorldEntity.Structures.Defs;

namespace kbs2.WorldEntity.Structures.TrainingStructure
{
    public class TrainingStructureView : BuildingView<TrainingStructureModel, TrainingStructureDef>
    {
        public TrainingStructureView(TrainingStructureModel model) : base(model)
        {
        }
    }
}