using kbs2.GamePackage.AIPackage.Enums;
using kbs2.Unit.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Interfaces
{
    public interface IHasCommand
    {
        Command Order { get; set; }
        IHasPersonalSpace Target { get; set; }
        bool FinishedOrder { get; set; }
    }
}
