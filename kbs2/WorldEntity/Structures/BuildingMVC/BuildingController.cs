using System.Collections.Generic;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.WorldEntity.Structures.BuildingMVC
{
    public class BuildingController : IStructure<BuildingDef>, IGameActionHolder
    {
        public List<IGameAction> GameActions { get; } = new List<IGameAction>();

        public BuildingModel Model { get; } = new BuildingModel();
        public BuildingView BuildingView { get; }

        public IViewImage View => BuildingView;

        public FloatCoords FloatCoords => (FloatCoords) Model.TopLeft;
        public FloatCoords Centre => new FloatCoords {x = (Model.TopLeft.x + View.Width / 2), y = (Model.TopLeft.y + View.Height / 2)};

        public List<WorldCellModel> OccupiedCells => Model.LocationCells;

        public Coords StartCoords
        {
            get => Model.TopLeft;
            set => Model.TopLeft = value;
        }

        public BuildingDef Def => Model.Def;

        public int ViewRange => 8;

        public float Width => View.Width;

        public float Height => View.Height;

        public Faction_Controller Faction { get; set; }
        public ViewMode ViewMode { set => Model.ViewMode = value; }

        public BuildingController(BuildingDef def)
        {
            Model.Def = def;
            BuildingView = new BuildingView(Model);
            BuildingView.ViewMode = ViewMode.Full;
        }

        public void Update(object sender, OnTickEventArgs eventArgs)
        {
        }

        /// <summary>Checks if user has enough currency to purchase.
        /// <para>If user has enough the cost price will be removed from balance and upkeep cost will be added to the total upkeepcost of there faction</para>
        /// </summary>
        /// TODO rewrite. This is an awful method.
        /// Does nothing. Only keeping this in as an example of what not to do.
        public void EnoughCurrencyCheck(IStructureDef def)
        {
            if (Faction.CurrencyController.Model.Currency < def.Cost) return;

//            Faction.CurrencyController.RemoveCurrency((float) def.Cost);
//            Faction.CurrencyController.AddUpkeepCost((float) def.UpkeepCost);
        }
    }
}