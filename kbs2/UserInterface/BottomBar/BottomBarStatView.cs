using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using Microsoft.Xna.Framework;

namespace kbs2.UserInterface.BottomBar
{
    public class BottomBarStatView
    {
        public BottomBarModel Model { get; set; }

        public StatImageView StatImage { get; set; }
        public StatTextView StatText { get; set; }

        public BottomBarStatView(BottomBarModel model, IViewImage entity)
        {
            Model = model;
            StatImage = new StatImageView(new FloatCoords() { x = (Model.MainView.coords.x + 5) + (20*Model.StatViews.Count), y = Model.MainView.coords.y + 5 }, entity);
            StatText = new StatTextView();
        }
    }
}   