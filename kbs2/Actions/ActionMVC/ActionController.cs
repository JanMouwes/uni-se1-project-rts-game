using kbs2.Actions.ActionMVC;
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
        public GameAction gameAction;

        public void Activate()
        {
            gameAction(Model);
        }
    }
}
