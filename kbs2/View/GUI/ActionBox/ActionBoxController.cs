using kbs2.GamePackage.EventArgs;
using kbs2.World.Structs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.View.GUI.ActionBox
{
    public class ActionBoxController
    {
        public ActionBoxModel BoxModel { get; set; }
        public ActionBoxView BoxView { get; set; }

        public ActionBoxController(FloatCoords loc)
        {
            BoxModel = new ActionBoxModel(loc);
            BoxView = new ActionBoxView(loc);
        }

        public void OnRightClick(object sender, EventArgsWithPayload<MouseState> mouseEvent)
        {
            if(mouseEvent.Value.RightButton == ButtonState.Pressed)
            {
                BoxView.Coords = new FloatCoords() { x = mouseEvent.Value.X, y = mouseEvent.Value.Y };
                BoxModel.Text.Coords = new FloatCoords() { x = mouseEvent.Value.X, y = mouseEvent.Value.Y };
                BoxModel.Show = true;
            }
                
            if (mouseEvent.Value.LeftButton == ButtonState.Pressed)
                BoxModel.Show = false;
        }
    }
}