using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.World.Interfaces;

namespace kbs2.World.Cell
{
	class WorldCellController
	{
		public void ChangeTerrain(TerrainType type) { }

		public void Construct(IConstructable cunstructable) { }

		public void ChangeView(ViewMode mode) { }

		public void OnDestruction() { }
	}
}
