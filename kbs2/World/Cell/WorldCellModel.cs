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
		public TerrainType baseTerrain { get; set; }
		public TerrainType terrain { get; set; }
		public Coords realCoords { get; set; }
		public WorldChunkModel parentChunk { get; set; }
		public ViewMode viewMode { get; set; }
	}
}
