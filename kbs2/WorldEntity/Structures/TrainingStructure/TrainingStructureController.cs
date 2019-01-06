using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.BuildingMVC;
using kbs2.WorldEntity.Structures.Defs;
using kbs2.WorldEntity.WorldEntitySpawner;

namespace kbs2.WorldEntity.Structures.TrainingStructure
{
    public class TrainingStructureController : IStructure<TrainingStructureDef>, ITrainingEntity
    {
        public Faction_Controller Faction { get; set; }

        public TrainingStructureDef Def => Model.Def;
        private TrainingStructureModel Model { get; } = new TrainingStructureModel();

        /// <inheritdoc cref="View"/>
        public TrainingStructureView TrainingStructureView { get; }

        public IViewImage View => TrainingStructureView;

        public List<WorldCellModel> OccupiedCells { get; } = new List<WorldCellModel>();

        public FloatCoords FloatCoords => (FloatCoords) StartCoords;

        public Coords StartCoords
        {
            get => Model.TopLeft;
            set => Model.TopLeft = value;
        }

        public FloatCoords Centre => new FloatCoords
        {
            x = StartCoords.x + Width / 2,
            y = StartCoords.y + Height / 2
        };

        public int ViewRange => Def.ViewRange;
        public float Width => Def.ViewValues.Width;
        public float Height => Def.ViewValues.Height;

        public int TimeFinished { get; private set; }
        public int TimeRemaining { get; private set; }
        public int TotalTimeRemaining { get; private set; }

        public ITrainable CurrentlyTraining { get; private set; }

        public Queue<ITrainable> TrainingQueue
        {
            get => Model.TrainingQueue;
            set
            {
                if (Model.TrainingQueue.Count >= QueueLimit) return;

                Model.TrainingQueue = value;
            }
        }

        public uint QueueLimit { get; set; }

        private EntitySpawner spawner;

        public TrainingStructureController(TrainingStructureDef def, EntitySpawner spawner)
        {
            this.spawner = spawner;
            Model.Def = def;
            TrainingStructureView = new TrainingStructureView(Model);
        }

        public void Update(object sender, OnTickEventArgs eventArgs)
        {
            int totalTimeElapsed = (int) Math.Floor(eventArgs.GameTime.TotalGameTime.TotalSeconds);

            if (CurrentlyTraining == null)
            {
                if (!TrainingQueue.Any()) return;
                
                CurrentlyTraining = TrainingQueue.Dequeue();
                TimeFinished = totalTimeElapsed + (int) CurrentlyTraining.Def.TrainingTime;
                return;
            }

            if (totalTimeElapsed >= TimeFinished)
            {
                spawner.SpawnWorldEntity(CurrentlyTraining);
                CurrentlyTraining = null;
                return;
            }

            TimeRemaining = TimeFinished - totalTimeElapsed;
        }

        public List<IGameAction> GameActions { get; } = new List<IGameAction>();
    }
}