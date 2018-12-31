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

        public BuildingController(IStructureDef def)
        {
            Def = def;
        }

        public BuildingModel Model { get; } = new BuildingModel();
        public BuildingView View { get; set; }

        public FloatCoords FloatCoords => (FloatCoords) Model.TopLeft;

        public FloatCoords center => new FloatCoords { x = (Model.TopLeft.x + View.Width/2), y = (Model.TopLeft.y + View.Height/2) };

        public List<IGameAction> GameActions => actions;
        public List<WorldCellModel> OccupiedCells => Model.LocationCells;

        public Coords StartCoords
        {
            get => Model.TopLeft;
            set => Model.TopLeft = value;
        }

        public int viewrange => 8;

        public IStructureDef Def { get; }

        public Faction_Controller Faction { get; set; }

        public float With => View.Width;

        public float Heigth => View.Height;

        public void Update(object sender, OnTickEventArgs eventArgs)
        {
        }

        

        /// <summary>Checks if user has enough currency to purchase.
        /// <para>If user has enough the cost price will be removed from balance and upkeep cost will be added to the total upkeepcost of there faction</para>
        /// </summary>
        public void EnoughCurrencyCheck(IStructureDef def)
        {
            if(Faction.currency_Controller.model.currency >= def.Cost){
                Faction.currency_Controller.RemoveCurrency((float)def.Cost);
                Faction.currency_Controller.AddUpkeepCost((float)def.UpkeepCost);
            }
        }

    }
}