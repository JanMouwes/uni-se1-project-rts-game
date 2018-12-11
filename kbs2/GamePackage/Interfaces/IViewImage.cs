using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage.Interfaces
{
    public interface IViewImage : IViewItem
    {
        float Width { get; set; }
        float Height { get; set; }
        string Texture { get; set; }
    }
}
