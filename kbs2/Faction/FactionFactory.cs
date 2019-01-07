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
        /// <summary>
        /// Returns a new Faction that the player can interact with
        /// </summary>
        /// <param name="name">De meegegeven naam onderscheid de factions</param>
        /// <returns>Geeft een faction controller</returns>
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

        /// <summary>
        /// Returns a new hostile Faction that you cant interact with
        /// </summary>
        /// <param name="name">De meegegeven naam onderscheid de factions</param>
        /// <returns>Geeft een faction controller</returns>
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
