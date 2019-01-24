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
        private GameController game;
        
        public FactionFactory(GameController game)
        {
            this.game = game;
        }


        /// <summary>
        /// Returns a new Faction that the player can interact with
        /// </summary>
        /// <param name="name">De meegegeven naam onderscheid de factions</param>
        /// <param name="balance">Faction's starting balance</param>
        /// <returns>Geeft een faction controller</returns>
        public Faction_Controller CreatePlayerFaction(string name, float balance)
        {
            return CreateFaction(name, balance);
        }

        /// <summary>
        /// Returns a new hostile Faction that you cant interact with
        /// </summary>
        /// <param name="name">De meegegeven naam onderscheid de factions</param>
        /// <param name="startingBalance">Faction's starting balance</param>
        /// <returns>Geeft een faction controller</returns>
        public Faction_Controller CreateCPUFaction(string name, float startingBalance = 500f)
        {
            return CreateFaction(name, startingBalance);
        }

        public Faction_Controller CreateFaction(string name, float startingBalance)
        {
            Faction_Controller faction = new Faction_Controller(name, game, startingBalance)
            {
                FactionModel = new FactionModel(name)
                {
                    Name = name,
                    FactionRelationships = new Dictionary<Faction_Controller, Enums.Faction_Relations>(),
                    Units = new List<WorldEntity.Unit.MVC.UnitController>(),
                    Buildings = new List<IStructure<IStructureDef>>()
                },
            };

            faction.FactionModel.Faction = faction;

            return faction;
        }
    }
}
