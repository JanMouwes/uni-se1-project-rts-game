using System;
using System.Collections.Generic;
using kbs2.Actions;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Desktop.World.World;
using kbs2.GamePackage;
using kbs2.World.Cell;
using kbs2.WorldEntity.Interfaces;
using Microsoft.Xna.Framework;

namespace kbs2.WorldEntity.Building.BuildingUnderConstructionMVC
{
    public class BUCController : IBlockCells, IHasActions
    {

        public BUCModel BUCModel { get; set; }
        public BUCView BUCView { get; set; }
        public WorldController World { get; set; }
        public GameController gameController { get; set; }
        public ConstructionCounter counter { get; set; }

        public List<ActionController> Actions => throw new NotImplementedException();

        public BUCController()
        {
            counter = new ConstructionCounter();
            counter.BUCController = this;
        }

        // check if timer has run out and update counter
        public void Update(object sender, OnTickEventArgs eventArgs)
        {
            if(eventArgs.GameTime.TotalGameTime.TotalSeconds > BUCModel.Time)
            {
                SetBuilding();
            }
            counter.Text = ((int)(BUCModel.Time - eventArgs.GameTime.TotalGameTime.TotalSeconds)).ToString();
        }

        // replace buc with building
        private void SetBuilding()
        {
            World.RemoveBUC(this);
            // romove refrences to this from cells
            foreach(WorldCellModel cell in BUCModel.LocationCells)
            {
                cell.BuildingOnTop = null;
            }
            // make building
            Building_Controller building = BuildingFactory.CreateNewBuilding(BUCModel.BuildingDef, BUCModel.TopLeft);
            World.AddBuilding(BUCModel.BuildingDef, building);
            BUCModel.faction_Controller.AddBuildingToFaction(building);

            // unsub from ontick event
            gameController.onTick -= Update;
        }
    }
}
