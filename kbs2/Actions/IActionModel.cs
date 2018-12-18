using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Actions
{
    public interface IActionModel
    {
        float CoolDown { get; }
        float CurrentCoolDown { get; set; }
    }
}
