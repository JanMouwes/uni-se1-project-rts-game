using kbs2.Actions;
using kbs2.Actions.ActionMVC;
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

        public BuildActions()
        {
            Actions = new List<ActionController>();
            for(int i = 0; i<9; i++)
            {
                Actions.Add(new ActionController { View = new ActionView { Texture = "TrainingCenter", Color = Color.White, ZIndex = 2 } });
            }
        }
    }
}
