using System;
using System.Collections.Generic;
using kbs2.World;
using kbs2.World.Enums;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.WorldEntity.Structures
{
    public class BuildingDef : IStructureDef
    {
        public List<Coords> BuildingShape { get; set; }

        #region Sprite info

        [Obsolete] public float Height => ViewValues.Height;
        [Obsolete] public float Width => ViewValues.Width;

        #endregion

        //Cost info
        public double Cost { get; set; }
        public double UpkeepCost { get; set; }

        public HPDef HPDef { get; set; } = new HPDef();


        public ViewValues ViewValues { get; set; }

        public int ViewRange { get; set; }

        public uint ConstructionTime { get; set; }

        public List<TerrainType> LegalTerrain { get; set; }
    }
}