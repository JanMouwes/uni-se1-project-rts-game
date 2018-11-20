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
		private TerrainType baseTerrain;
		public TerrainType terrain;
		private Coords realCoords;
		private WorldChunkModel parentChunk;
		private ViewMode viewMode;
	}
}
