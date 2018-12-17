using kbs2.Actions;
using kbs2.Actions.ActionModels;
using kbs2.Actions.ActionMVC;
using kbs2.GamePackage;
using kbs2.WorldEntity.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.UserInterface
{
    public class BuildActions : IHasActions
    {
        public List<ActionController> Actions { get; set; }

        // generate test actionlist
        public BuildActions(GameController Controller)
        {
            Actions = new List<ActionController>();
            for(int i = 0; i<9; i++)
            {
                ActionController Action = new ActionController { View = new ActionView { Texture = "Button", Colour = Color.White, ZIndex = 2, gameController = Controller } };
                Action.Model = new Spawn_Model { CurrentCoolDown = 100, CoolDown = 20 };
                Action.CooldownView = new CooldownView { actionModel = Action.Model, gameController = Controller };
                Controller.onTick += Action.Update;
                Actions.Add(Action);
            }
        }
    }
}
