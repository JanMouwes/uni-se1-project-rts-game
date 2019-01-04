using kbs2.Faction.CurrencyMVC;
using kbs2.Faction.Enums;
using kbs2.Faction.Interfaces;
using kbs2.Unit.Model;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Unit.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.GamePackage;
using kbs2.GamePackage.DayCycle;
using kbs2.GamePackage.EventArgs;
using kbs2.Resources;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.ResourceFactory;
using Microsoft.Xna.Framework.Graphics;

namespace kbs2.Faction.FactionMVC
{
    public class Faction_Controller : IHasFactionRelationship
    {
        public FactionModel FactionModel { get; set; }
        public readonly Currency_Controller CurrencyController;

        public Faction_Controller(string name, GameController game)
        {
            FactionModel = new FactionModel(name);
            CurrencyController = new Currency_Controller();

            game.TimeController.DayPassed += OnDayPassed;
        }

        // if new day gives reward
        private void OnDayPassed(object sender, EventArgsWithPayload<IngameTime> eventArgs)
        {
            double balance = 0;
            using (ResourceCalculator resourceCalculator = new ResourceCalculator())
            {
                foreach (ResourceFactoryController resourceFactory in FactionModel.ResourceFactories)
                {
                    resourceCalculator.AddResource(resourceFactory.ResourceValue, resourceFactory.ResourceType);
                }

                balance += resourceCalculator.CalculateResourceWorth();
            }

            balance -= FactionModel.Buildings.Sum(structure => structure.Def.UpkeepCost);
            balance -= FactionModel.Units.Sum(unit => unit.UnitModel.Def.Upkeep);

            CurrencyController.AlterCurrency((float) balance);
        }


        // Adds a unit to the faction units list
        public void AddUnitToFaction(UnitController unit) => FactionModel.Units.Add(unit);

        public void RegisterBuilding(IStructure<IStructureDef> building) => FactionModel.Buildings.Add(building);

        public void UnregisterBuilding(IStructure<IStructureDef> building) => FactionModel.Buildings.Remove(building);

        public void RegisterUnit(UnitController unit) => FactionModel.Units.Add(unit);

        public void UnregisterUnit(UnitController unit) => FactionModel.Units.Remove(unit);

        // Checks if the given faction is hostile to this faction
        public bool IsHostileTo(FactionModel faction) => FactionModel.FactionRelationships[faction] == Faction_Relations.hostile;

        // Checks if there is a relation with the given faction and changes it to the given relation if not the same
        public void ChangeRelationship(FactionModel faction, Faction_Relations relation)
        {
            if (FactionModel.FactionRelationships[faction] == relation) return;

            FactionModel.FactionRelationships.Remove(faction);
            FactionModel.FactionRelationships.Add(faction, relation);

            faction.FactionRelationships.Remove(FactionModel);
            faction.FactionRelationships.Add(FactionModel, relation);
        }

        // Adds a relationship to the faction if it doesnt exist yet
        public void AddRelationship(FactionModel faction, Faction_Relations relation)
        {
            if (FactionModel.FactionRelationships.ContainsKey(faction)) return;

            FactionModel.FactionRelationships.Add(faction, relation);
            faction.FactionRelationships.Add(FactionModel, relation);
        }
    }
}