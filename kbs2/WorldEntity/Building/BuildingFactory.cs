using kbs2.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Building
{
    public static class BuildingFactory
    {
        public static Building_Controller CreateNewBuilding(BuildingDef def, Coords TopLeft)
        {
            Building_Controller building_Controller = new Building_Controller(def,TopLeft);
            

            return building_Controller;
        }
    }
}
