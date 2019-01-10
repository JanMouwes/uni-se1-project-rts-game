using kbs2.GamePackage.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.UserInterface.Interfaces
{
    public interface IButton : Unit_Controller
    {
        string Name { get; set; }
    }
}
