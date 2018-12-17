using kbs2.GamePackage;
using kbs2.World;
using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.utils
{
    public static class ActionButtonPosition
    {
        
        // set locations of all bottons
        public const int ButtonSize = 40;
        public const int ButtonSpace = 5;
        public static Coords[] ButtonCoords = new Coords[9] {
            new Coords{x = (ButtonSize + ButtonSpace)*3, y = (ButtonSize + ButtonSpace)*3},
            new Coords{x = (ButtonSize + ButtonSpace)*2, y = (ButtonSize + ButtonSpace)*3},
            new Coords{x = (ButtonSize + ButtonSpace)*1, y = (ButtonSize + ButtonSpace)*3},
            new Coords{x = (ButtonSize + ButtonSpace)*3, y = (ButtonSize + ButtonSpace)*2},
            new Coords{x = (ButtonSize + ButtonSpace)*2, y = (ButtonSize + ButtonSpace)*2},
            new Coords{x = (ButtonSize + ButtonSpace)*1, y = (ButtonSize + ButtonSpace)*2},
            new Coords{x = (ButtonSize + ButtonSpace)*3, y = (ButtonSize + ButtonSpace)*1},
            new Coords{x = (ButtonSize + ButtonSpace)*2, y = (ButtonSize + ButtonSpace)*1},
            new Coords{x = (ButtonSize + ButtonSpace)*1, y = (ButtonSize + ButtonSpace)*1}
        };

        public static FloatCoords GetPosition(GameController gameController,int index)
        {
            Coords Corner = new Coords { x = gameController.GraphicsDevice.Viewport.Width, y = gameController.GraphicsDevice.Viewport.Height };
            return (FloatCoords)(Corner - ButtonCoords[index]);
        }
    }
}
