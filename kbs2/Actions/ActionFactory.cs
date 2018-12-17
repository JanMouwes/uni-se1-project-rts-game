using kbs2.Actions.ActionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Actions
{
    public enum ActionType
    {
        SpawnBuilding,
        SpawnUnit
    }

    public class ActionFactory
    {
        public ActionController NewAction(ActionType actionType)
        {
            ActionController actionController = new ActionController();
            switch (actionType)
            {
                case ActionType.SpawnBuilding:
                    actionController.Model = NewSpawnModel();
                    break;
                case ActionType.SpawnUnit:
                    actionController.Model = NewSpawnModel();
                    break;

            }


            return actionController;
        }

        public IActionModel NewSpawnModel()
        {
            Spawn_Model model = new Spawn_Model();
            model.ConstructionTime = 20;
            model.CoolDown = 20;
            model.CurrentCoolDown = 20;
            //todo add faction
            return model;
        }
    }
}
