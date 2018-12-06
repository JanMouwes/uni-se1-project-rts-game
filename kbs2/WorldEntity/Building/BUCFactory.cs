using System;
using kbs2.World;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;

namespace kbs2.WorldEntity.Building
{
    public class BUCFactory
    {
        public static BUCController CreateNewBUC(BuildingDef def, Coords TopLeft)
        {
            BUCController BUCController = new BUCController();

            BUCModel BUCmodel = new BUCModel(TopLeft);
            BUCController.BUCModel = BUCmodel;
            BUCView view = new BUCView("Construction", def.height, def.width)
            {
                model = BUCmodel
            };
            BUCController.BUCView = view;


            return BUCController;
        }
    }
}
