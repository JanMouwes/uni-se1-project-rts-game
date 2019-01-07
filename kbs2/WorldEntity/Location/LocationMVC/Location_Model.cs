using System.Collections.Generic;
using kbs2.GamePackage.EventArgs;
using kbs2.World;
using kbs2.World.Enums;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.WorldEntity.Location
{
    public class LocationModel
    {
        public UnitController Parent;
        public Coords Coords => (Coords) FloatCoords;

        public event OnMoveHandler OnMove;

        private FloatCoords floatCoords;

        public FloatCoords FloatCoords
        {
            get => floatCoords;
            set
            {
                floatCoords = value;
                OnMove?.Invoke(this, new EventArgsWithPayload<FloatCoords>(floatCoords));
            }
        }

        public List<TerrainType> UnwalkableTerrain;

        public LocationModel(float locationX, float locationY)
        {
            FloatCoords = new FloatCoords() {x = locationX, y = locationY};

            UnwalkableTerrain = new List<TerrainType>();
        }
    }
}