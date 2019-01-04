using kbs2.World;
using kbs2.WorldEntity.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;
using kbs2.Unit;

namespace kbs2.WorldEntity.Building
{
    public class BuildingDef : IStructureDef
    {
        public List<Coords> BuildingShape { get; set; }

        #region Sprite info

        [Obsolete] public string Image { get; set; }
        [Obsolete] public float Height { get; set; }
        [Obsolete] public float Width { get; set; }

        #endregion

        //Cost info
        public double Cost { get; set; }
        public double UpkeepCost { get; set; }

        public HPDef HPDef { get; set; } = new HPDef();


        public ViewValues ViewValues
        {
            get => new ViewValues(Image, Width, Height);
            set
            {
                Image = value.Image;
                Width = value.Width;
                Height = value.Height;
            }
        }

        public int ViewRange { get; set; }

        public uint ConstructionTime { get; set; }
    }
}