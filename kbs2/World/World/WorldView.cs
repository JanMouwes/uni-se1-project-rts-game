using kbs2.Desktop.World.World;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.World.World
{
    public class WorldView
    {
        public WorldModel worldModel { get; set; }

        public WorldView(WorldModel world)
        {
            this.worldModel = world;
        }

    }
}
