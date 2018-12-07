using System;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Desktop.World.World;
using kbs2.GamePackage;
using kbs2.World.Cell;
using kbs2.WorldEntity.Interfaces;
using Microsoft.Xna.Framework;

namespace kbs2.WorldEntity.Building.BuildingUnderConstructionMVC
{
    public class BUCController : IBlockCells
    {

        public BUCModel BUCModel { get; set; }
        public BUCView BUCView { get; set; }
        public WorldController World { get; set; }
        public GameController gameController { get; set; }
        public int TimerView  {get; set; }


        public BUCController()
        {
        }

        public void Update(object sender, OnTickEventArgs eventArgs)
        {
            if(eventArgs.GameTime.TotalGameTime.Seconds > BUCModel.Time)
            {
                SetBuilding();
            }
            TimerView = BUCModel.Time - eventArgs.GameTime.TotalGameTime.Seconds;
            Console.WriteLine(TimerView);
        }


        private void SetBuilding()
        {

            World.RemoveBUC(this);
            foreach(WorldCellModel cell in BUCModel.LocationCells)
            {
                cell.BuildingOnTop = null;
            }

            Building_Controller building = BuildingFactory.CreateNewBuilding(BUCModel.BuildingDef, BUCModel.TopLeft);
            World.AddBuilding(BUCModel.BuildingDef, building);
            gameController.onTick -= Update;
        }
    }
}
