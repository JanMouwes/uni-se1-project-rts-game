using kbs2.World.Structs;
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