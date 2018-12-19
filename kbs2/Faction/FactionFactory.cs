using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Faction
{
    public class FactionFactory
    {
        public static Faction_Controller CreatePlayerFaction(string name)
        {
            Faction_Controller playerFaction = new Faction_Controller()
            {
                FactionModel = new Faction_Model
                {
                    Name = name,
                    FactionRelationships = new Dictionary<Faction_Controller, Enums.Faction_Relations>(),
                    Units = new List<WorldEntity.Unit.MVC.Unit_Controller>(),
                    Buildings = new List<WorldEntity.Building.Building_Controller>(),
                    BUCs = new List<WorldEntity.Building.BuildingUnderConstructionMVC.BUCController>()
                },

                currency_Controller = new CurrencyMVC.Currency_Controller()
            };
            

            return playerFaction;
        }
        // Reden voor verschillende methodes tussen Player en Faction is start scenarios etc.
        public static Faction_Controller CreateCPUFaction(string name)
        {
            Faction_Controller cpuFaction = new Faction_Controller()
            {
                FactionModel = new Faction_Model
                {
                    Name = name,
                    FactionRelationships = new Dictionary<Faction_Controller, Enums.Faction_Relations>(),
                    Units = new List<WorldEntity.Unit.MVC.Unit_Controller>(),
                    Buildings = new List<WorldEntity.Building.Building_Controller>(),
                    BUCs = new List<WorldEntity.Building.BuildingUnderConstructionMVC.BUCController>()
                },

                currency_Controller = new CurrencyMVC.Currency_Controller()
            };

            return cpuFaction;
        }
    }
}
