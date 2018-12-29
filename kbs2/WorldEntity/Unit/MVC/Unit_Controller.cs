using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Actions;
using kbs2.Desktop.World.World;
using kbs2.Unit.Model;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Battle;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Location;
using kbs2.WorldEntity.Location.LocationMVC;
using kbs2.WorldEntity.XP.XPMVC;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace kbs2.WorldEntity.Unit.MVC
{
    public class Unit_Controller : ISelectable, IMoveable, IHasActions
    {
        public Location_Controller LocationController;
        public Unit_Model UnitModel;
        public Unit_View UnitView;

        public List<ActionController> Actions => UnitModel.actions;

        public Unit_Controller()
        {
            UnitView = new Unit_View(this);
            UnitModel = new Unit_Model();
        }

        public RectangleF CalcClickBox()
        {
            return new RectangleF(LocationController.LocationModel.floatCoords.x - UnitView.Width / 2, LocationController.LocationModel.floatCoords.y - UnitView.Height / 2, UnitView.Width, UnitView.Height);
        }

        public void MoveTo(FloatCoords target, bool isQueueKeyPressed) => LocationController.MoveTo(target, isQueueKeyPressed);
    }
}