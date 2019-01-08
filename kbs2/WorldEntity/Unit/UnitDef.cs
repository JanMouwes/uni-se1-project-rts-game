using System.Collections.Generic;
using kbs2.Actions.Interfaces;
using kbs2.World.Enums;
using kbs2.WorldEntity.Battle;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;
using kbs2.WorldEntity.XP;

namespace kbs2.WorldEntity.Unit
{
    public class UnitDef : ITrainableDef, IGameActionHolder
    {
        public float Speed;
        public string Image { get; set; }
        public float Width;
        public float Height;
        public BattleDef BattleDef { get; set; }
        public int MaxHealth { get; set; }
        public LevelXPDef LevelXPDef { get; set; }

        public ViewValues ViewValues
        {
            get => new ViewValues(Image, Width, Height);
            set
            {
                Image = value.Image;
                Width = value.Width;
                Height = value.Height;
            }
        }

        public int ViewRange { get; set; }

        public float PurchaseCost
        {
            get => (float) Cost;
            set => Cost = value;
        }

        public float Upkeep
        {
            get => (float) UpkeepCost;
            set => UpkeepCost = value;
        }

        public List<TerrainType> LegalTerrain { get; set; }
        public uint TrainingTime { get; set; }

        public ViewValues IconData => new ViewValues(ViewValues.Image, 20, 20);

        public double Cost { get; set; }

        public double UpkeepCost { get; set; }
        
        public List<IGameAction> GameActions { get; } = new List<IGameAction>();
    }
}