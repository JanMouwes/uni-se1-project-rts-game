using kbs2.GamePackage;
using kbs2.GamePackage.Interfaces;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.Actions.ActionMVC
{
    public class CooldownView : IViewText
    {
        public GameController gameController { get; set; }

        // the index of the location
        public int index { get; set; }


        public IActionModel actionModel { get; set; }

        public string SpriteFont { get {return "BuildingTimer"; } set {; } }
        public string Text { get {return actionModel.CurrentCoolDown>0? ((int)actionModel.CurrentCoolDown).ToString(): " " ; } set {; } }
        public FloatCoords Coords { get => ActionButtonPosition.GetPosition(gameController, index); set {; } }
        public int ZIndex { get {return 2; }  set {; } }
        public Color Colour { get {return Color.White; } set {; } }

        public ViewMode ViewMode => ViewMode.Full;
    }
}
