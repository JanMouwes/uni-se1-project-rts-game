using System;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kbs2.UserInterface
{
    public class UIView : IViewable
    {

        public Coords coords = new Coords
        {
            x = 0,
            y = 0
        };
        
        public FloatCoords Coords { get { return (FloatCoords)coords; } set {; } }
        public float Height { get { return 140; } set {; } }
        public float Width { get; set; }
        public string Texture { get { return "UITexture"; } set {; } }
        public Color Color { get { return Color.White; } set {; } }
        public int ZIndex { get { return 1; } set {; } }
    }
}
