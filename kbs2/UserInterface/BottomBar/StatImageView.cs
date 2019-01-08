using kbs2.GamePackage.Interfaces;
using kbs2.View.GUI;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.UserInterface.BottomBar
{
    public class StatImageView : IGuiViewImage
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public string Texture { get; set; }
        public FloatCoords Coords { get; set; }
        public int ZIndex { get; set; }
        public Color Colour { get; set; }

        public ViewMode ViewMode => ViewMode.Full;

        // ENTITY IMAGE
        public StatImageView(FloatCoords coords, IViewImage entity)
        {
            Coords = coords;
            Width = 30;
            Height = 30;
            Texture = entity.Texture;
            ZIndex = 1001;
            Colour = Color.White;
        }
        // MAX HP BAR
        public StatImageView(FloatCoords coords, string image)
        {
            Coords = coords;
            Coords = new FloatCoords() { x = Coords.x + 1, y = Coords.y + 26};
            Width = 28;
            Height = 4;
            Texture = image;
            ZIndex = 1002;
            Colour = Color.White;
        }
        // CUR HP BAR
        public StatImageView(FloatCoords coords, HP_Model hpModel, string image)
        {
            Coords = coords;
            Coords = new FloatCoords() { x = Coords.x + 1, y = Coords.y + 26 };
            Width = (float) (((double) hpModel.CurrentHP / hpModel.MaxHP) * 28);
            Height = 4;
            Texture = image;
            ZIndex = 1003;
            Colour = Color.White;
        }

        public void Click()
        {
            
        }
    }
}
