using System.Collections.Generic;
using System.Linq;
using kbs2.GamePackage.EventArgs;
using kbs2.UserInterface.BottomBar;
using kbs2.View.GUI;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.BuildingMVC;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kbs2.UserInterface
{
    public class BottomBarView : IGuiViewImage
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Coords coords => new Coords
        {
            x = (int)(GraphicsDevice.Viewport.Width * .15),
            y = (int)(GraphicsDevice.Viewport.Height * .81)
        };

        public double Rotation { get; }
        public FloatCoords Coords { get { return (FloatCoords)coords; } set {; } }
        public float Height { get { return (int)(GraphicsDevice.Viewport.Height * .19); } set {; } }
        public float Width { get { return (int)(GraphicsDevice.Viewport.Width * .70); } set {; } }
        public string Texture { get { return "bottombarmid"; } set {; } }

        public BottomBarModel Model { get; set; }

        public Color Colour { get { return Color.White; } set {; } }
        public int ZIndex { get { return 1; } set {; } }

        public ViewMode ViewMode => ViewMode.Full;

        public void Click()
        {
        }

        public List<IGuiViewImage> GetContents()
        {
            List<IGuiViewImage> templist = new List<IGuiViewImage>();
            foreach(BottomBarStatView statView in Model.StatViews)
            {
                templist.Add(statView.StatImage);
                templist.Add(statView.MaxHP);
                templist.Add(statView.CurHP);
            }

            return templist;
        } 

        public BottomBarView(GraphicsDevice GraphicsDevice)
        {
            this.GraphicsDevice = GraphicsDevice;
            Model = new BottomBarModel(this);
        }


        /// <summary>
        /// Changes the HUD according to the selected entities
        /// </summary>
        /// <param name="eventArgs">List of selected entities</param>
        public void UpdateHUDOnSelect(object sender, EventArgsWithPayload<List<IGameActionHolder>> eventArgs)
        {
            // Clean the GUI of selected entities
            /*foreach (BottomBarStatView view in bottomBarView.Model.StatViews)
            {
                GameModel.GuiItemList.Remove(view.StatImage);
                GameModel.GuiItemList.Remove(view.CurHP);
                GameModel.GuiItemList.Remove(view.MaxHP);
                GameModel.GuiTextList.Remove(view.StatName);
            }*/

            Model.StatViews.Clear();

            // Convert all IHasActions to Unit_Controllers
            List<IGameActionHolder> selection = eventArgs.Value;
            // Add new views to the model
            foreach (UnitController unit in selection.OfType<UnitController>()) Model.StatViews.Add(new BottomBarStatView(Model, unit.UnitView, unit.HealthValues));

            // Add new views to the model
            //foreach (BuildingController building in selection.OfType<BuildingController>()) Model.StatViews.Add(new BottomBarStatView(Model, building.View, building.HealthValues));
            /*
            if (Model.StatViews.Count == 1)
            {
                if (selection.Count > 0)
                {
                    Model.StatViews[0].AddNameText(((UnitController)selection[0]).UnitModel.Name);
                }
                else if (structures.Count > 0)
                {
                    //bottomBarView.Model.StatViews[0].AddNameText(((BuildingController)buildings[0]));
                }
            }
            */
        }
    }
}
