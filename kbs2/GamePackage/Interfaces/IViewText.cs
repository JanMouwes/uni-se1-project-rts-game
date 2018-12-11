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
    public interface IViewText : IViewItem
    {
        string SpriteFont { get; set; }
        string Text { get; set; }
    }
}