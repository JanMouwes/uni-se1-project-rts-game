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
        public StatImageView MaxHP { get; set; }
        public StatImageView CurHP { get; set; }
        public StatTextView NameText { get; set; }

        public BottomBarStatView(BottomBarModel model, IViewImage entity, HP_Model healthModel)
        {
            Model = model;
            StatImage = new StatImageView(Offset(), entity);
            MaxHP = new StatImageView(Offset(), "hpbar");
            CurHP = new StatImageView(Offset(), healthModel, "curhpbar");
        }

        public void AddNameText(string name)
        {
            NameText = new StatTextView(Offset(), name);
        }



        /// <summary>
        /// ListView of Selected entities
        /// </summary>
        /// <returns></returns>
        private FloatCoords Offset() {
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