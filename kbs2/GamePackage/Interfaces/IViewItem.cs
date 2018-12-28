using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.GamePackage.Interfaces
{
    public interface IViewItem
    {
        FloatCoords Coords { get; set; }
        int ZIndex { get; set; }
        Color Colour { get; set; }
    }
}