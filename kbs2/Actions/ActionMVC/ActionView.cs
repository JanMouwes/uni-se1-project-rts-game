using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Actions.ActionMVC
{
    public class ActionView : IViewable
    {
        public FloatCoords Coords { get ; set ; }
        public float Width { get ; set; }
        public float Height { get; set; }
        public string Texture { get; set; }
        public Color Color { get; set; }
        public int ZIndex { get; set; }
    }
}
