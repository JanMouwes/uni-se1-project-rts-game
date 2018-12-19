using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Actions;
using kbs2.Desktop.World.World;
using kbs2.Faction.FactionMVC;
using kbs2.Unit.Model;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Battle;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Location;
using kbs2.WorldEntity.XP.XPMVC;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace kbs2.WorldEntity.Unit.MVC
{
	public class Unit_Controller : IMoveable, IHasActions
	{
		public Location_Controller LocationController;
		public Unit_Model UnitModel;
        public Unit_View UnitView;
        public HP_Controller HPController;

        public List<ActionController> Actions { get {return UnitModel.actions; } }

        // Create a new unit and add it to a faction
        public void CreateUnit(Faction_Model faction)
        {
            faction.Units.Add(this);
        }

        public void MoveTo(FloatCoords target,bool CTRL)
        {
            LocationController.MoveTo(target,CTRL);
        }
    }
}
