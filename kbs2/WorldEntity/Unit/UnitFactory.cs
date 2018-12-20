using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Desktop.World.World;
using kbs2.Faction.FactionMVC;
using kbs2.Unit.Model;
using kbs2.Unit.Unit;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Location;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework;

namespace kbs2.WorldEntity.Unit
{
	public class UnitFactory
	{
        public WorldModel World { get; set; }

        public UnitFactory(WorldModel worldModel)
        {
            World = worldModel;
        }

		public static Unit_Controller CreateNewUnit(UnitDef def, WorldModel world)
        {
            Unit_Controller UnitController = new Unit_Controller
            {
                UnitView = new Unit_View
                {
                    Texture = def.Image,
                    Width = def.Width,
                    Height = def.Height,
                    Colour = Color.White,
                    ZIndex = 12
                },
                UnitModel = new Unit_Model
                {
                    Speed = def.Speed,
                    Name = def.Name,
                    //Faction = faction             Uncomment when merge has happened DO NOT REMOVE
                },
                HPController = new HP_Controller
                {
                    HPModel = new HP_Model
                    {
                        CurrentHP = def.HPDef.CurrentHP,
                        MaxHP = def.HPDef.MaxHP
                    }
                }
            };
            // Unit_view needs a Unit_Controller for some reason
            UnitController.UnitView.Unit_Controller = UnitController;

            // Location can still be refactored
            Location_Controller location = new Location_Controller(world, 0, 0);
            location.LocationModel.parent = UnitController;
            UnitController.LocationController = location;

            return UnitController;
        }
    }
}



