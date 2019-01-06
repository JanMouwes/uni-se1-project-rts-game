using kbs2.Actions.GameActionDefs;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.Actions.GameActions
{
    public class TrainAction : MapAction<TrainActionDef>
    {
        public TrainAction(TrainActionDef actionDef) : base(actionDef, actionDef.SpawnableDef.ViewValues)
        {
        }

        public override void Execute(ITargetable target)
        {
            throw new System.NotImplementedException();
        }
    }
}