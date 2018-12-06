using kbs2.World;
using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using kbs2.Faction.FactionMVC;
using kbs2.Unit.Unit;
using kbs2.Unit.Interfaces;
using kbs2.Faction.Interfaces;
using kbs2.WorldEntity;
using kbs2.GamePackage.Interfaces;
using Microsoft.Xna.Framework;

namespace kbs2.Unit.Model
{
    public class Unit_Model : IHasPersonalSpace, IPurchasable, IElemental, IHasFaction
    {
        public Faction_Model Faction { get; set; }
        public UnitDef UnitDef;

		public float Width { get; set; }
		public float Height { get; set; }

		public bool Selected { get; set; }

        public List<TerrainType> UnwalkableTerrain { get; set; }

        public List<ElementType> Elementtypes { get; set; }

        public CostValue CostValue { get; set; }

		public Unit_Model(float height, float width)
        {
            Height = height;
            Width = width;
            Selected = false;
        }

        public void InsertUnitDef(int id)
        {
            DBController.OpenConnection("DefDex");

            UnitDef = DBController.GetDefinitionFromUnit(1);

            DBController.CloseConnection();
        }

    }
}
