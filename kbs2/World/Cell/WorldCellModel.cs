using kbs2.World.Chunk;
using kbs2.World.Enums;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.World.Cell
{
    public class WorldCellModel
    {

        // The base terrain defines the original terrain of this tile while the terrain keeps track of the current terrain state.
        public TerrainType BaseTerrain { get; set; }
        public TerrainType Terrain { get; set; }

        // RealCoords are coords relative to the center tile
        public Coords RealCoords { get; set; }

        // This defines what chunk the cell is in
        public WorldChunkModel ParentChunk { get; set; }

        // ViewMode defines the current ViewMode Enum state ( full, fog or none )
        public ViewMode ViewMode { get; set; } = ViewMode.None;

        public IStructure<IStructureDef> BuildingOnTop { get; set; }

        public WorldCellModel(TerrainType terrain, Coords realCoords)
        {
            this.Terrain = this.BaseTerrain = terrain;
            RealCoords = realCoords;
        }
    }
}
