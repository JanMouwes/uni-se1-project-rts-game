using kbs2.WorldEntity.Structs;

namespace kbs2.WorldEntity.Interfaces
{
    public interface IWorldEntityDef : ISpawnableDef
    {
        ViewValues ViewValues { get; set; }

        int ViewRange { get; set; }
    }
}