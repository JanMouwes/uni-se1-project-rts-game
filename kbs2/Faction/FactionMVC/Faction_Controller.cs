using kbs2.Faction.CurrencyMVC;
using kbs2.Faction.Enums;
using kbs2.Faction.Interfaces;
using kbs2.WorldEntity.Unit.MVC;
using System.Linq;
using kbs2.GamePackage;
using kbs2.GamePackage.DayCycle;
using kbs2.GamePackage.EventArgs;
using kbs2.Resources;
using kbs2.Unit.Interfaces;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.ResourceFactory;

namespace kbs2.Faction.FactionMVC
{
    public class Faction_Controller : IHasFactionRelationship
    {
        public FactionModel FactionModel { get; set; }
        public readonly Currency_Controller CurrencyController;

        public Faction_Controller(string name, GameController game) : this(name, game, 500)
        {
        }

        public Faction_Controller(string name, GameController game, float startingBalance)
        {
            FactionModel = new FactionModel(name);
            CurrencyController = new Currency_Controller(startingBalance);

            game.TimeController.DayPassed += OnDayPassed;
        }

        /// <summary>
        /// TODO move to (new) BalanceManager-class
        /// Calculates balance based on income and upkeep.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
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

        public virtual void RegisterBuilding(IStructure<IStructureDef> building) => FactionModel.Buildings.Add(building);

        public virtual void UnregisterBuilding(IStructure<IStructureDef> building) => FactionModel.Buildings.Remove(building);

        public virtual void RegisterUnit(UnitController unit) => FactionModel.Units.Add(unit);

        public virtual void UnregisterUnit(UnitController unit) => FactionModel.Units.Remove(unit);

        public bool CanPurchase(IPurchasable purchasable) => CurrencyController.Model.Currency > purchasable.Cost;

        public void Purchase(IPurchasable purchasable) => CurrencyController.AlterCurrency((float) -purchasable.Cost);

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