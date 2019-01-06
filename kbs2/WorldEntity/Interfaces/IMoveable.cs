using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.GamePackage.EventArgs;

namespace kbs2.WorldEntity.Interfaces
{
    public delegate void OnMoveHandler(object sender, EventArgsWithPayload<FloatCoords> newLocationEventArgs);

    public interface IMoveable
    {
        void MoveTo(FloatCoords target, bool isQueueKeyPressed);

        event OnMoveHandler OnMove;
    }
}