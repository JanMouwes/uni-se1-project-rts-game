using System.Collections.Generic;
using kbs2.Actions;
using kbs2.Actions.ActionMVC;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.GamePackage.EventArgs;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.WorldEntity.Building.BuildingMVC;
using kbs2.WorldEntity.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kbs2.WorldEntity.Building.BuildingUnderConstructionMVC
{
    public class ConstructingBuildingController : IStructure<ConstructingBuildingDef>, IHasGameActions
    {
        public delegate void ConstructionCompleteObserver(object sender, EventArgsWithPayload<IStructureDef> eventArgs);

        public float CurrentTimer;
        public ConstructingBuildingModel ConstructingBuildingModel { get; set; } = new ConstructingBuildingModel();
        public ConstructingBuildingView ConstructingBuildingView { get; set; }
        public GameController GameController { get; set; }
        public ConstructionCounter Counter { get; }

        public FloatCoords FloatCoords => (FloatCoords) ConstructingBuildingModel.StartCoords;

        public List<IGameAction> GameActions => ConstructingBuildingModel.GameActions;

        public event ConstructionCompleteObserver ConstructionComplete;

        public ConstructingBuildingController()
        {
            Counter = new ConstructionCounter
            {
                ConstructingBuildingController = this
            };
        }

        // check if timer has run out and update counter
        public void Update(object sender, OnTickEventArgs eventArgs)
        {
            if (eventArgs.GameTime.TotalGameTime.TotalSeconds > ConstructingBuildingModel.FinishTime)
                ConstructionComplete?.Invoke(this, new EventArgsWithPayload<IStructureDef>(ConstructingBuildingModel.BuildingDef));

            Counter.Text = ((int) (ConstructingBuildingModel.FinishTime - eventArgs.GameTime.TotalGameTime.TotalSeconds)).ToString();
            CurrentTimer = (float) eventArgs.GameTime.ElapsedGameTime.TotalSeconds;
        }

        public List<WorldCellModel> OccupiedCells => ConstructingBuildingModel.LocationCells;

        public Coords StartCoords
        {
            get => ConstructingBuildingModel.StartCoords;
            set => ConstructingBuildingModel.StartCoords = value;
        }

        public ConstructingBuildingDef Def { get; }
        public Faction_Controller Faction => ConstructingBuildingModel.FactionController;
    }
}