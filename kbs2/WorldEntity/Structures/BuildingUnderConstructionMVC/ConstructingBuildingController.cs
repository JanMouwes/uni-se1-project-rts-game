using System.Collections.Generic;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.Defs;

namespace kbs2.WorldEntity.Structures.BuildingUnderConstructionMVC
{
    public class ConstructingBuildingController : IStructure<ConstructingBuildingDef>, IGameActionHolder
    {
        public delegate void ConstructionCompleteObserver(object sender, EventArgsWithPayload<IStructureDef> eventArgs);

        public ConstructingBuildingModel ConstructingBuildingModel { get; set; } = new ConstructingBuildingModel();
        public ConstructingBuildingView ConstructingBuildingView { get; set; }
        public ConstructionCounter Counter { get; }

        public FloatCoords FloatCoords => (FloatCoords) ConstructingBuildingModel.StartCoords;

        public List<IGameAction> GameActions => ConstructingBuildingModel.GameActions;

        public event ConstructionCompleteObserver ConstructionComplete;

        public FloatCoords Centre => new FloatCoords
        {
            x = StartCoords.x + ConstructingBuildingView.Width / 2,
            y = StartCoords.y + ConstructingBuildingView.Height / 2
        };

        public int ViewRange => 8;

        public Unit_Controller View => ConstructingBuildingView;

        public List<WorldCellModel> OccupiedCells => ConstructingBuildingModel.LocationCells;

        public Coords StartCoords
        {
            get => ConstructingBuildingModel.StartCoords;
            set => ConstructingBuildingModel.StartCoords = value;
        }

        public ConstructingBuildingDef Def => ConstructingBuildingModel.BuildingDef;
        public Faction_Controller Faction
        {
            get => ConstructingBuildingModel.FactionController;
            set => ConstructingBuildingModel.FactionController = value;
        }

        public float Width => ConstructingBuildingView.Width;

        public float Height => ConstructingBuildingView.Height;

        public ViewMode ViewMode { set => ConstructingBuildingView.ViewMode = value; }

        public ConstructingBuildingController(ConstructingBuildingDef def)
        {
            ConstructingBuildingModel.BuildingDef = def;
            Counter = new ConstructionCounter
            {
                ConstructingBuildingController = this
            };
        }

        // check if timer has run out and update counter
        public void Update(object sender, OnTickEventArgs eventArgs)
        {
            if (eventArgs.GameTime.TotalGameTime.TotalSeconds >= ConstructingBuildingModel.FinishTime)
            {
                ConstructionComplete?.Invoke(this, new EventArgsWithPayload<IStructureDef>(ConstructingBuildingModel.BuildingDef));
                ConstructionComplete = null;
            }

            Counter.Text = ((int) (ConstructingBuildingModel.FinishTime - eventArgs.GameTime.TotalGameTime.TotalSeconds)).ToString();
        }
    }
}