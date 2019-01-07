using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Desktop.World.World;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.CPU;
using kbs2.GamePackage.Interfaces;
using kbs2.View.GUI.ActionBox;

namespace kbs2.GamePackage
{
	public class GameModel
	{
		public WorldController World { get; set; }
		public MouseInput MouseInput { get; set; }
		public List<Faction_Controller> Factions { get; set; }
        public List<CPU_Controller> CPUPlayers { get; set; }
		public ActionBoxController ActionBox { get; set; }
		public float Time { get; set; }
		public GameState GameState { get; set; }
		public GameSpeed GameSpeed { get; set; }
		public Pathfinder pathfinder { get; set; }

        // List For everything
        public List<IViewImage> ItemList = new List<IViewImage>();
        // List For everything in the gui
        public List<IViewImage> GuiItemList = new List<IViewImage>();
        // List For everything
        public List<IViewText> TextList = new List<IViewText>();
        // List For everything in the gui
        public List<IViewText> GuiTextList = new List<IViewText>();

        public GameModel()
        {
            Factions = new List<Faction_Controller>();
            CPUPlayers = new List<CPU_Controller>();
        }
    }
}
