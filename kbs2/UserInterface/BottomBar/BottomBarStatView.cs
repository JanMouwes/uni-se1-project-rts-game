using kbs2.GamePackage;
using kbs2.GamePackage.Interfaces;
using kbs2.World.Structs;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework;

namespace kbs2.UserInterface.BottomBar
{
    public class BottomBarStatView
    {
        public BottomBarModel Model { get; set; }

        public StatImageView StatImage { get; set; }
        public StatImageView MaxHP { get; set; }
        public CurrentHPView CurHP { get; set; }
        public StatTextView StatName { get; set; }

        public BottomBarStatView(BottomBarModel model, UnitController unit, Selection_Controller selection_Controller)
        {
            Model = model;
            StatImage = new StatImageView(ListView(), unit,selection_Controller);
            MaxHP = new StatImageView(ListView(), unit, selection_Controller, "hpbar");
            CurHP = new CurrentHPView(ListView(), unit);
        }

        public void AddNameText(string name)
        {
            StatName = new StatTextView(ListView(), name);
        }

        /// <summary>
        /// Puts all the views in a list form
        /// </summary>
        /// <returns></returns>
        private FloatCoords ListView() {
            const int padding = 5;
            const int margin = 30;

            int row = 0;
            int unitsPerRow = (int) (Model.MainView.Width / margin) - 1;
            int rowSizeY = 40;
            int curUnitPos = 0;
            

            for (int i = 0; i < Model.StatViews.Count; i++)
            {
                if (i % unitsPerRow == 0 && i != 0)
                {
                    row++;
                    curUnitPos = -1;
                }

                curUnitPos++;
            }

            return new FloatCoords()
            {
                x = (Model.MainView.coords.x + padding) + (margin * curUnitPos),
                y = Model.MainView.coords.y + (row * rowSizeY)
            }; 
        }
    }
}   