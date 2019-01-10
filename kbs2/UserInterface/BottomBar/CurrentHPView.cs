using kbs2.View.GUI;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.UserInterface.BottomBar
{
    public class CurrentHPView : IGuiViewImage
    {
        public float Width => (float)(((double)hpModel.CurrentHP / hpModel.MaxHP) * 28);
        public float Height { get; set; }
        public string Texture { get; set; }
        public FloatCoords Coords { get; set; }
        public int ZIndex { get; set; }
        public Color Colour { get; set; }
        public HealthValues hpModel { get; set; }

        public ViewMode ViewMode => ViewMode.Full;

        public double Rotation => throw new NotImplementedException();
        
        // CUR HP BAR
        public CurrentHPView(FloatCoords coords, UnitController unit)
        {
            Coords = coords;
            Coords = new FloatCoords() { x = Coords.x + 1, y = Coords.y + 26 };
            this.hpModel = unit.HealthValues;
            Height = 4;
            Texture = "curhpbar";
            ZIndex = 1003;
            Colour = Color.White;
        }

        public void Click(MouseState mouseState)
        {

        }

        public List<IGuiViewImage> GetContents()
        {
            throw new NotImplementedException();
        }
    }
}
