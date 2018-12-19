using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Interfaces
{
    public interface IMoveable
    {
        void MoveTo(FloatCoords target,bool CTRL);
    }
}
