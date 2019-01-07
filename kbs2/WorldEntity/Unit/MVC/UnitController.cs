using System.Collections.Generic;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Location.LocationMVC;

namespace kbs2.WorldEntity.Unit.MVC
{
    public class UnitController : ITrainable, IMoveable
    {
        public event OnMoveHandler OnMove
        {
            remove => LocationController.LocationModel.OnMove -= value;
            add => LocationController.LocationModel.OnMove += value;
        }

        public Location_Controller LocationController;
        public Unit_Model UnitModel;
        public Unit_View UnitView;

        public IViewImage View => UnitView;

        public List<IGameAction> GameActions => UnitModel.Actions;

        public FloatCoords FloatCoords => LocationController.LocationModel.FloatCoords;

        public Faction_Controller Faction
        {
            get => UnitModel.Faction;
            set => UnitModel.Faction = value;
        }

        public FloatCoords Center => new FloatCoords
        {
            x = LocationController.LocationModel.FloatCoords.x + UnitView.Width / 2,
            y = LocationController.LocationModel.FloatCoords.y + UnitView.Height / 2
        };

        public int ViewRange => UnitModel.Def.ViewRange;

        public UnitController(UnitDef def)
        {
            UnitView = new Unit_View(this);
            UnitModel = new Unit_Model(def);
        }

        public void MoveTo(FloatCoords target, bool isQueueKeyPressed)
        {
            LocationController.MoveTo(target, isQueueKeyPressed);
        }


        public virtual void Update(object sender, OnTickEventArgs eventArgs) => LocationController.Update(sender, eventArgs);

        public ITrainableDef Def => UnitModel.Def;
    }
}