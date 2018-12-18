using kbs2.World;
using kbs2.WorldEntity.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Building
{
	public class BuildingDef
	{
        // list of cells that contain the building
        public List<Coords> BuildingShape { get; set; }

        // sprite info
        public string imageSrc { get; set; }
        public float height { get; set; }
        public float width { get; set; }
        public string Name { get; set; }

        public HPDef HPDef { get; set; }

        public void AddShapeFromString(string shape)
        {
            BuildingShape = new List<Coords>();
            char[] array = shape.ToCharArray();
            Coords coords = new Coords
            {
                x = 0,
                y = 0
            };
            foreach (char c in array)
            {
                if(c == 'x')
                {
                    BuildingShape.Add(coords);
                    coords.x++;
                }
                if(c == 'o')
                {
                    coords.x++;
                }
                if(c == ';')
                {
                    coords.x = 0;
                    coords.y++;
                }
            }
        }


    }
}
