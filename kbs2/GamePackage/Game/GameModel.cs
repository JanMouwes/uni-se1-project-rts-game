using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Desktop.World.World;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.Interfaces;

namespace kbs2.GamePackage
{
	public class GameModel
	{
		public WorldController World { get; set; }
		public List<Faction_Controller> Factions { get; set; }
		public Selection_Controller Selection { get; set; }
		public float Time { get; set; }
		public GameState GameState { get; set; }
		public GameSpeed GameSpeed { get; set; }
		public Pathfinder pathfinder { get; set; }

        // List For everything
        public List<IViewable> ItemList = new List<IViewable>();
        // List For everything in the gui
        public List<IViewable> GuiItemList = new List<IViewable>();
    }
}
