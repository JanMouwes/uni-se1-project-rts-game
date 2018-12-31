using System.Collections.Generic;
using kbs2.Actions.ActionMVC;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Faction.FactionMVC;
using kbs2.Unit.Model;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Location;

namespace kbs2.WorldEntity.Unit.MVC
{
    public class UnitController : IWorldEntity, IMoveable, IHasGameActions
    {
        public Location_Controller LocationController;
        public Unit_Model UnitModel;
        public Unit_View UnitView;

        public List<IGameAction> GameActions => UnitModel.Actions;

        public FloatCoords FloatCoords => LocationController.LocationModel.FloatCoords;
        public Faction_Controller Faction => UnitModel.Faction;

        public FloatCoords center { get {
                return new FloatCoords
                {
                    x = LocationController.LocationModel.FloatCoords.x + UnitView.Width / 2,
                    y = LocationController.LocationModel.FloatCoords.y + UnitView.Height / 2
                };
            }
        }

        public int viewrange = 8;

        public UnitController()
        {
            UnitView = new Unit_View(this);
            UnitModel = new Unit_Model();
        }

        public void MoveTo(FloatCoords target, bool isQueueKeyPressed)
        {
            LocationController.MoveTo(target, isQueueKeyPressed);
        }


        public void Update(object sender, OnTickEventArgs eventArgs)=>LocationController.Ontick(sender, eventArgs);
        

        

    }
}