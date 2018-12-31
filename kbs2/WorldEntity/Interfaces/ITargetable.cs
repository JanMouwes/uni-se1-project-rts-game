using kbs2.World.Structs;

namespace kbs2.WorldEntity.Interfaces
{
    public interface ITargetable
    {
        /// <summary>
        /// Target's location
        /// </summary>
        FloatCoords FloatCoords { get; }
    }
}