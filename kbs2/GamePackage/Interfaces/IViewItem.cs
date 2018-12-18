using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.GamePackage.Interfaces
{
    public interface IViewItem
    {
        FloatCoords Coords { get; }
        int ZIndex { get; }
        Color Colour { get; }
    }
}