using kbs2.World;
using kbs2.WorldEntity.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.WorldEntity.Building
{
    public class BuildingDef : IStructureDef
    {
        // list of cells that contain the building
        public List<Coords> BuildingShape { get; set; }

        // sprite info
        public string Image { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }

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
                switch (c)
                {
                    case 'x':
                        BuildingShape.Add(coords);
                        coords.x++;
                        break;
                    case 'o':
                        coords.x++;
                        break;
                    case ';':
                        coords.x = 0;
                        coords.y++;
                        break;
                    default:
                        throw new ArgumentException($"Invalid char '{c}'"); //NOTE temp solution
                        break;
                }
            }
        }

        public ViewValues ViewValues => new ViewValues(Image, Width, Height);
    }
}