using kbs2.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Building
{
	public class BuildingDef
	{
        public List<Coords> BuildingShape { get; set; }
        public string imageSrc { get; set; }
        public float height { get; set; }
        public float width { get; set; }

    }
}
