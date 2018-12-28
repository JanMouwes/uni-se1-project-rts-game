using kbs2.Actions;
using kbs2.World;
using kbs2.World.Cell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Building
{
    public class Building_Model
    {

        public Coords TopLeft { get; set; }

        public List<ActionController> actions { get; set; }

        // all the cells the building is on
        public List<WorldCellModel> LocationCells { get; set; }

        public Building_Model(Coords topLeft)
        {
            TopLeft = topLeft;
            LocationCells = new List<WorldCellModel>();
            actions = new List<ActionController>();
        }
    }
}
