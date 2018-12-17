using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
using Microsoft.Xna.Framework;

namespace kbs2.UserInterface.BottomBar
{
    public class BottomBarStatView
    {
        public BottomBarModel Model { get; set; }

        public StatImageView StatImage { get; set; }
        public StatTextView StatText { get; set; }

        public BottomBarStatView(BottomBarModel model, IViewImage entity, HP_Model healthModel)
        {
            Model = model;
            StatImage = new StatImageView(new FloatCoords() { x = (Model.MainView.coords.x + 5) + (40 * Model.StatViews.Count), y = Model.MainView.coords.y + 5 }, entity);
            StatText = new StatTextView(new FloatCoords() { x = (Model.MainView.coords.x + 5) + (40 * Model.StatViews.Count), y = Model.MainView.coords.y + 35 }, healthModel);
        }
    }
}   