using kbs2.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Building
{
    public class Building_Model
    {
        public BuildingDef Defenition {get; set;}
        public Coords TopLeft { get; set; }

        public Building_Model(BuildingDef buildingDef,  Coords topLeft)
        {
            Defenition = buildingDef;
            TopLeft = topLeft;
        }
    }
}
