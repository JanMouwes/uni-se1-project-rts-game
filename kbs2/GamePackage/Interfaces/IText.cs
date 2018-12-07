using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage.Interfaces
{
    public interface IText
    {
        FloatCoords Coords { get; set; }
        string SpriteFont { get; set; }
        string Text { get; set; }
        Color Color { get; set; }
        int ZIndex { get; set; }
    }
}
