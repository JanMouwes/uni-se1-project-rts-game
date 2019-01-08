using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.WorldEntity.Interfaces;
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
        public static Faction_Controller CreatePlayerFaction(string name, GameController game)
        {
            Faction_Controller playerFaction = new Faction_Controller(name, game)
            {
                FactionModel = new FactionModel(name)
                {
                    Name = name,
                    FactionRelationships = new Dictionary<Faction_Controller, Enums.Faction_Relations>(),
                    Units = new List<WorldEntity.Unit.MVC.UnitController>(),
                    Buildings = new List<IStructure<IStructureDef>>()
                },

                CurrencyController = new CurrencyMVC.Currency_Controller(500f)
            };

            playerFaction.FactionModel.Faction = playerFaction;

            return playerFaction;
        }

        /// <summary>
        /// Returns a new hostile Faction that you cant interact with
        /// </summary>
        /// <param name="name">De meegegeven naam onderscheid de factions</param>
        /// <returns>Geeft een faction controller</returns>
        public static Faction_Controller CreateCPUFaction(string name, GameController game)
        {
            Faction_Controller cpuFaction = new Faction_Controller(name, game)
            {
                FactionModel = new FactionModel(name)
                {
                    Name = name,
                    FactionRelationships = new Dictionary<Faction_Controller, Enums.Faction_Relations>(),
                    Units = new List<WorldEntity.Unit.MVC.UnitController>(),
                    Buildings = new List<IStructure<IStructureDef>>()
                },

                CurrencyController = new CurrencyMVC.Currency_Controller(500f)
            };

            return cpuFaction;
        }
    }
}
