using kbs2.GamePackage;
using kbs2.GamePackage.Interfaces;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.Actions.ActionMVC
{
    public class ActionView : IViewImage
    {
        public GameController gameController { get; set; }
        
        // the index of the location
        public int index { get; set; }

        //stuff from IViewable
        public double Rotation { get; }
        public FloatCoords Coords { get => ActionButtonPosition.GetPosition(gameController, index); set {; } }
        public float Width { get {return ActionButtonPosition.ButtonSize; } set {; } }
        public float Height { get { return ActionButtonPosition.ButtonSize; } set {; } }
        public string Texture { get; set; }
        public Color Colour { get; set; }
        public int ZIndex { get; set; }

        public ViewMode ViewMode => ViewMode.Full;
    }
}
