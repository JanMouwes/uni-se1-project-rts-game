using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kbs2.GamePackage.Interfaces
{
    public interface IViewImage : IViewItem
    {
        float Width { get; }
        float Height { get; }
        string Texture { get; }
    }
}