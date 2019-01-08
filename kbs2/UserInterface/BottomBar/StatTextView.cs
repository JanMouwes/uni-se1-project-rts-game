using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.UserInterface.BottomBar
{
    public class StatTextView : IViewText
    {
        public string SpriteFont { get; set; }
        public string Text { get; set; }
        public FloatCoords Coords { get; set; }
        public int ZIndex { get; set; }
        public Color Colour { get; set; }

        public ViewMode ViewMode => ViewMode.Full;

        // Health
        public StatTextView(FloatCoords coords, HP_Model healthModel) : this(coords, $"{healthModel.CurrentHP} / {healthModel.MaxHP}") { Colour = Color.Red; }

        // Name
        public StatTextView(FloatCoords coords, string displayText)
        {
            Coords = coords;
            Coords = new FloatCoords() { x = Coords.x + 20, y = Coords.y + 20};
            Text = displayText;
            SpriteFont = "unitstatinfo";
            ZIndex = 1005;
            Colour = Color.Black;
        }
    }
}
