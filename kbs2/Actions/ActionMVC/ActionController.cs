using kbs2.Actions.ActionMVC;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static kbs2.Actions.ActionDelegates;

namespace kbs2.Actions
{
    public class ActionController
    {
        public IActionModel Model;
        public ActionView View;
        public CooldownView CooldownView;
        public GameAction gameAction;

        public void Activate(FloatCoords target)
        {
            if (Model.CurrentCoolDown <= 0)
            {
                gameAction(Model, target);
                Model.CurrentCoolDown = Model.CoolDown;
            }
        }

        public void Update(object sender, OnTickEventArgs eventArgs)
        {
            Model.CurrentCoolDown -= (float)eventArgs.GameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
