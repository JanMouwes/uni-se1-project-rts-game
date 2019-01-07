using System.Collections.Generic;
using kbs2.Faction.Enums;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures;
using Moq;
using NUnit.Framework;


namespace Tests
{
    [TestFixture]
    public class FactionTests
    {
        private Faction_Controller Unit;
        private Faction_Controller Enemy;
        private Faction_Controller Friend;
        private Faction_Controller Neutral;
        private GameController game;

        [SetUp]
        public void init()
        {
            Mock<GameController> gameMock = new Mock<GameController>(GameSpeed.Regular, GameState.Paused);

            game = gameMock.Object;

            Unit = new Faction_Controller("Water", game);
            Enemy = new Faction_Controller("Earth", game);
            Friend = new Faction_Controller("Fire", game);
            Neutral = new Faction_Controller("Äir", game);
        }


        [Test]
        [TestCase(Faction_Relations.hostile, true)]
        [TestCase(Faction_Relations.friendly, false)]
        [TestCase(Faction_Relations.neutral, false)]
        public void IsHostileToFaction(Faction_Relations relation, bool ExpectedResult)
        {
            Unit.AddRelationship(Enemy.FactionModel, relation);


            var result = Unit.IsHostileTo(Enemy.FactionModel);
            var result2 = Enemy.IsHostileTo(Unit.FactionModel);


            Assert.IsTrue(result == ExpectedResult);
            Assert.IsTrue(result2 == ExpectedResult);
        }

        [Test]
        [TestCase(Faction_Relations.friendly, Faction_Relations.friendly, true)]
        [TestCase(Faction_Relations.hostile, Faction_Relations.hostile, true)]
        [TestCase(Faction_Relations.neutral, Faction_Relations.neutral, true)]
        [TestCase(Faction_Relations.friendly, Faction_Relations.hostile, false)]
        [TestCase(Faction_Relations.hostile, Faction_Relations.neutral, false)]
        [TestCase(Faction_Relations.neutral, Faction_Relations.friendly, false)]
        public void AddRelationshipToFaction(Faction_Relations relation, Faction_Relations RelationCheck, bool ExpectedResult)
        {
            //    [Review] wtf? Why loop through relationships when you can access them like FactionRelationships[Key]?

            Unit.AddRelationship(Friend.FactionModel, relation);

            var result = false;
            var result2 = false;

            foreach (KeyValuePair<FactionModel, Faction_Relations> relationship in Unit.FactionModel.FactionRelationships)
            {
                if (relationship.Key.Name == Friend.FactionModel.Name && relationship.Value == RelationCheck)
                {
                    result = true;
                }
            }

            foreach (KeyValuePair<FactionModel, Faction_Relations> relationship in Friend.FactionModel.FactionRelationships)
            {
                if (relationship.Key.Name == Unit.FactionModel.Name && relationship.Value == RelationCheck)
                {
                    result2 = true;
                }
            }

            Assert.IsTrue(result == ExpectedResult);
            Assert.IsTrue(result2 == ExpectedResult);
        }


        [Test]
        [TestCase(Faction_Relations.neutral, Faction_Relations.hostile, true)]
        [TestCase(Faction_Relations.friendly, Faction_Relations.neutral, true)]
        [TestCase(Faction_Relations.hostile, Faction_Relations.hostile, false)]
        public void ChangeRelationshipOfFaction(Faction_Relations relation, Faction_Relations ChangedRelation, bool ExpectedResult)
        {
            //    [Review] wtf? Why loop through relationships when you can access them like FactionRelationships[Key]?

            Unit.AddRelationship(Friend.FactionModel, relation);

            var result = false;
            var result2 = false;

            Unit.ChangeRelationship(Friend.FactionModel, ChangedRelation);

            foreach (KeyValuePair<FactionModel, Faction_Relations> relationship in Unit.FactionModel.FactionRelationships)
            {
                if (relationship.Key.Name == Friend.FactionModel.Name && relationship.Value != relation)
                {
                    result = true;
                }
            }

            foreach (KeyValuePair<FactionModel, Faction_Relations> relationship in Friend.FactionModel.FactionRelationships)
            {
                if (relationship.Key.Name == Unit.FactionModel.Name && relationship.Value != relation)
                {
                    result2 = true;
                }
            }

            Assert.IsTrue(result == ExpectedResult);
            Assert.IsTrue(result2 == ExpectedResult);
        }

        [TestCase()]
        public void Test_ShouldAlterCurrency_WhenDayPassed()
        {
            //TODO
            //    Arrange
            Faction_Controller faction = new Faction_Controller("test", game, 500);

            Mock<IStructure<IStructureDef>> structureMock = new Mock<IStructure<IStructureDef>>();

            structureMock.SetupGet(structureParam => structureParam.Def).Returns(new BuildingDef()
            {
                UpkeepCost = 100
            });
            IStructure<IStructureDef> structure = structureMock.Object;

            faction.RegisterBuilding(structure);

            //    Act
//            game.TimeController = new TimeController();

            //    Assert
        }
    }
}