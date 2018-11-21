using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.World.Chunk;

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
        public ViewMode ViewMode { get; set; }
	}
}
