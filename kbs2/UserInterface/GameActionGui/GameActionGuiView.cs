using System.Collections.Generic;
using System.Linq;
using kbs2.Actions.GameActionGrid;
using kbs2.GamePackage;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
using kbs2.View.GUI;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kbs2.UserInterface.GameActionGui
{
    public class GameActionGuiView : IGuiViewImage
    {
        private GameActionGuiModel model;

        private GraphicsDevice GraphicsDevice => model.GraphicsDevice;

        public FloatCoords Coords => (FloatCoords) model.Coords;

        public float Height => (int) (GraphicsDevice.Viewport.Height * (GameActionGuiModel.HEIGHT_PERCENT / 100));

        public float Width => (int) (GraphicsDevice.Viewport.Width * (GameActionGuiModel.WIDTH_PERCENT / 100));

        public string Texture { get; }

        public Color Colour => Color.White;

        public int ZIndex => 1;

        public ViewMode ViewMode => ViewMode.Full;

        public IEnumerable<IViewItem> GetContents => ((model.CurrentTab != null) 
            ? model.CurrentTab.GameActionTabItems.Where(item => item != null).Select(item => (IViewItem) item).ToList() 
            : new List<IViewItem>()).Concat(new List<IViewItem>() { });

        public void Click()
        {
        }

        public GameActionGuiView(GameActionGuiModel model)
        {
            this.model = model;
            Texture = "bottombarright";
        }
    }
}