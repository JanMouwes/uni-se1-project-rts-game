using System.Collections.Generic;
using kbs2.Actions.ActionMVC;
using kbs2.Actions.Interfaces;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Faction.FactionMVC;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.WorldEntity.Building.BuildingMVC
{
    public class BuildingController : IStructure, IHasGameActions
    {
        private readonly List<IGameAction> actions = new List<IGameAction>();
        public List<IGameAction> GameActions => actions;

        public BuildingModel Model { get; } = new BuildingModel();
        public BuildingView View { get; set; }

        public FloatCoords FloatCoords => (FloatCoords) Model.TopLeft;
        public FloatCoords Centre => new FloatCoords {x = (Model.TopLeft.x + View.Width / 2), y = (Model.TopLeft.y + View.Height / 2)};

        public List<WorldCellModel> OccupiedCells => Model.LocationCells;

        public Coords StartCoords
        {
            get => Model.TopLeft;
            set => Model.TopLeft = value;
        }

        public IStructureDef Def { get; }

        public int ViewRange => 8;

        public float Width => View.Width;

        public float Height => View.Height;

        public Faction_Controller Faction { get; set; }

        public BuildingController(IStructureDef def)
        {
            Def = def;
        }

        public void Update(object sender, OnTickEventArgs eventArgs)
        {
        }

        /// <summary>Checks if user has enough currency to purchase.
        /// <para>If user has enough the cost price will be removed from balance and upkeep cost will be added to the total upkeepcost of there faction</para>
        /// </summary>
        /// TODO rewrite. This is an awful method.
        public void EnoughCurrencyCheck(IStructureDef def)
        {
            if (Faction.currency_Controller.model.currency < def.Cost) return;

            Faction.currency_Controller.RemoveCurrency((float) def.Cost);
            Faction.currency_Controller.AddUpkeepCost((float) def.UpkeepCost);
        }
    }
}