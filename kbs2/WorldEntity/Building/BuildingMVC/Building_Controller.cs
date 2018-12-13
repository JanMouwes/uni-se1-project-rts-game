using kbs2.Actions;
using kbs2.World;
using kbs2.WorldEntity.Building.BuildingMVC;
using kbs2.WorldEntity.Interfaces;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Building
{
	public class Building_Controller : IImpassable, IHasActions
    {
        public Building_Model Model { get; set; }
        public BuildingView View { get; set; }

        public List<ActionController> Actions { get { return Model.actions; } } 

        public RectangleF CalcClickBox()
        {
            return new RectangleF((float)Model.TopLeft.x, (float)Model.TopLeft.y, View.Height, View.Width);
        }
    }
}
