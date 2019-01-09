using System.Collections.Generic;
using System.Linq;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.AIPackage.Enums;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
using kbs2.Unit.Interfaces;
using kbs2.Unit.Model;
using kbs2.World.Structs;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Location.LocationMVC;
using kbs2.WorldEntity.Structs;

namespace kbs2.WorldEntity.Unit.MVC
{
    public delegate void DeathDelegate(object sender, EventArgsWithPayload<UnitController> eventArgs);

    public class UnitController : ITrainable, IMoveable, IBattleEntity
    {
        public event OnMoveHandler OnMove
        {
            remove => LocationController.LocationModel.OnMove -= value;
            add => LocationController.LocationModel.OnMove += value;
        }

        public event DeathDelegate Death;
        public event OnTakeHitDelegate OnTakeHit;

        public Location_Controller LocationController;
        public HP_Controller HPController => new HP_Controller();
        public Unit_Model UnitModel;
        public Unit_View UnitView;

        public IViewImage View => UnitView;

        public List<IGameAction> GameActions { get; }

        public FloatCoords FloatCoords => Center;

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
            GameActions = new List<IGameAction>(def.GameActions);
        }

        public void MoveTo(FloatCoords target, bool isQueueKeyPressed)
        {
            LocationController.MoveTo(target, isQueueKeyPressed);
        }


        public virtual void Update(object sender, OnTickEventArgs eventArgs) => LocationController.Update(sender, eventArgs);

        public FloatCoords Centre => Center;

        public ITrainableDef Def => UnitModel.Def;

        public HealthValues HealthValues => UnitModel.HealthValues;


        public void TakeHit(HitValues hitValues)
        {
            UnitModel.HealthValues.CurrentHP -= hitValues.Damage * hitValues.BattleModifiers.AttackModifier;

            OnTakeHit?.Invoke(this, new EventArgsWithPayload<HitValues>(hitValues));

            if (HealthValues.CurrentHP <= 0) Death?.Invoke(this, new EventArgsWithPayload<UnitController>(this));
        }
    }
}