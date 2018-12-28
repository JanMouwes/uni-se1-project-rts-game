using kbs2.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Interfaces
{
    public interface IHasActions
    {
        List<ActionController> Actions { get;}
    }
}
