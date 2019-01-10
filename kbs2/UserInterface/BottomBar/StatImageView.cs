using kbs2.GamePackage;
using kbs2.GamePackage.Interfaces;
using kbs2.View.GUI;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
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
    public class StatImageView : IGuiViewImage
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public string Texture { get; set; }
        public FloatCoords Coords { get; set; }
        public int ZIndex { get; set; }
        public Color Colour { get; set; }

        public UnitController Unit { get; set; }
        public Selection_Controller selection_Controller { get; set; }

        public ViewMode ViewMode => ViewMode.Full;

        public double Rotation => throw new NotImplementedException();

        // ENTITY IMAGE
        public StatImageView(FloatCoords coords, UnitController unit , Selection_Controller selection_Controller)
        {
            Coords = coords;
            Width = 30;
            Height = 30;
            Texture = unit.UnitView.Texture;
            ZIndex = 1001;
            Colour = Color.White;
            Unit = unit;
            this.selection_Controller = selection_Controller;
        }
        // MAX HP BAR
        public StatImageView(FloatCoords coords,  UnitController unit, Selection_Controller selection_Controller , string image)
        {
            Coords = coords;
            Coords = new FloatCoords() { x = Coords.x + 1, y = Coords.y + 26};
            Width = 28;
            Height = 4;
            Texture = image;
            ZIndex = 1002;
            Colour = Color.White;
            Unit = unit;
            this.selection_Controller = selection_Controller;
        }

        public void Click(MouseState mouseState)
        {
            bool left = mouseState.LeftButton == ButtonState.Pressed ? true : false;
            if (left)
            {
                selection_Controller.setSelection(Unit);
            }
            else
            {
                selection_Controller.RemoveFromSelection(Unit);
            }
            
        }

        public List<IGuiViewImage> GetContents()
        {
            throw new NotImplementedException();
        }
    }
}
