using kbs2.World.Enums;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.World.Cell
{
    public class WorldCellController : ITargetable
    {
        public WorldCellModel worldCellModel { get; set; }

        public WorldCellView worldCellView { get; set; }

        public WorldCellController(FloatCoords coords, TerrainType terrain)
        {
            worldCellModel = new WorldCellModel(terrain, (Coords) coords);
            worldCellView = new WorldCellView(worldCellModel, TerrainDef.TerrainDef.TerrainDictionary[terrain]);
        }

        // Changes the TerrainType of the current cell
        public void ChangeTerrain(TerrainType type)
        {
            worldCellModel.Terrain = type;
        }

        // Switches the viewmode between enum ViewMode ( full, fog and none )
        public void ChangeViewMode(ViewMode mode)
        {
            worldCellModel.ViewMode = mode;
        }

        // This function is called when the building on top of the cell is destroyed
        public void OnDestruction()
        {
        }

        public FloatCoords FloatCoords => (FloatCoords) worldCellModel.RealCoords;
    }
}