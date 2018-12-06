using System;
using kbs2.Desktop.World.World;
using kbs2.World.Cell;
using Microsoft.Xna.Framework;

namespace kbs2.WorldEntity.Building.BuildingUnderConstructionMVC
{
    public class BUCController
    {

        public BUCModel BUCModel { get; set; }
        public BUCView BUCView { get; set; }
        public WorldController World { get; set; }


        public BUCController()
        {
        }

        public void Update(GameTime gameTime)//todo sub ontick
        {
            if(gameTime.TotalGameTime.Seconds < BUCModel.Time)
            {
                SetBuilding();
            }

        }


        public void SetBuilding()
        {

            World.RemoveBUC(this);
            foreach(WorldCellModel cell in BUCModel.LocationCells)
            {
                cell.BuildingOnTop = null;
            }

            Building_Controller building = BuildingFactory.CreateNewBuilding(BUCModel.BuildingDef, BUCModel.TopLeft);
            World.AddBuilding(BUCModel.BuildingDef, building);
        }
    }
}
