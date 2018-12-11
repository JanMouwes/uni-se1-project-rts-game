using kbs2.GamePackage;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Actions.ActionMVC
{
    public class ActionView : IViewImage
    {
        public GameController gameController { get; set; }

        // bottom right corner of the window
        public Coords Corner => new Coords { x = gameController.GraphicsDevice.Viewport.Width, y = gameController.GraphicsDevice.Viewport.Height };
        
        // set locations of all bottons
        public const int ButtonSize = 40;
        public const int ButtonSpace = 5;
        public Coords[] ButtonCoords = new Coords[9] {
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
        // the index of the location
        public int index { get; set; }

        //stuff from IViewable
        public FloatCoords Coords { get { return (FloatCoords)(Corner - ButtonCoords[index]); } set {; } }
        public float Width { get {return ButtonSize; } set {; } }
        public float Height { get { return ButtonSize; } set {; } }
        public string Texture { get; set; }
        public Color Colour { get; set; }
        public int ZIndex { get; set; }
    }
}
