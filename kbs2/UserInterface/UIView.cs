using kbs2.GamePackage;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.UserInterface
{
    public class UIView : IViewImage
    {
        public GameController gameController { get; set; }
        public Coords coords => new Coords
        {
            x = 0,
            y = gameController.GraphicsDevice.Viewport.Height -140
        };
        
        public FloatCoords Coords { get { return (FloatCoords)coords; } set {; } }
        public float Height { get { return 140; } set {; } }
        public float Width { get { return gameController.GraphicsDevice.Viewport.Width; } set {; } }
        public string Texture { get { return "UITexture"; } set {; } }
        public Color Colour { get { return Color.White; } set {; } }
        public int ZIndex { get { return 1; } set {; } }

        public ViewMode ViewMode => ViewMode.Full;

        public UIView(GameController gameController)
        {
            this.gameController = gameController;
        }
    }
}
