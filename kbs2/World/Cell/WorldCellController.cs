using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.World.Enums;
using kbs2.World.Interfaces;
using kbs2.World.Structs;

namespace kbs2.World.Cell
{
    public class WorldCellController
    {
        public WorldCellModel worldCellModel { get; set; } = new WorldCellModel();

        public WorldCellView worldCellView { get; set; }

        public WorldCellController(FloatCoords coords, TerrainType terrain)
        {
            worldCellModel.RealCoords = (Coords)coords;
            worldCellModel.Terrain = worldCellModel.BaseTerrain = terrain;
            worldCellView = new WorldCellView(coords, TerrainDef.TerrainDef.TerrainDictionary[terrain]);
        }

        // Changes the TerrainType of the current cell
        public void ChangeTerrain(TerrainType type)
        {
            worldCellModel.Terrain = type;
        }

        // Linkes the building on top of the cell to the model
        public void Construct(IConstructable constructable)
        {
        }

        // Switches the viewmode between enum ViewMode ( full, fog and none )
        public void ChangeViewMode(ViewMode mode)
        {
        }

        // This function is called when the building on top of the cell is destroyed
        public void OnDestruction()
        {
        }
    }
}