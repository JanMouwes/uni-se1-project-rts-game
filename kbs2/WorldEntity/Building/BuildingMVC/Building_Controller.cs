using kbs2.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Building
{
	public class Building_Controller
	{
        public Building_Model Model { get; set; }

        public Building_Controller(BuildingDef def,Coords TopLeft)
        {
            Model = new Building_Model( def, TopLeft);
        }
	}
}
