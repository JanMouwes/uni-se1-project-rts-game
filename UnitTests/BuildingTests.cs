using System;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.GamePackage.DayCycle;
using kbs2.World;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.BuildingMVC;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class BuildingTests
    {
        IStructureDef def;
        BuildingController controller;

        [SetUp]
        public void TestSetUp()
        {
            Mock<GameController> gameMock = new Mock<GameController>(GameSpeed.Regular, GameState.Paused);

            def = new BuildingDef
            {
                Cost = 50,
                UpkeepCost = 1.4,
            };
            controller = new BuildingController(def)
            {
                Faction = new Faction_Controller("test-faction", gameMock.Object),
                StartCoords = new Coords()
            };
        }

        [Obsolete]
        [Test]
        public void EnoughCurrencyCheck_RemoveCurrency_Test()
        {
            float expected = controller.Faction.CurrencyController.Model.currency - (float) def.Cost;
            controller.EnoughCurrencyCheck(def);

            Assert.AreEqual(expected, controller.Faction.CurrencyController.Model.currency);
        }

        [Obsolete]
        [Test]
        public void EnoughCurrencyCheck_AddUpkeepCosts_Test()
        {
            float expected = controller.Faction.CurrencyController.Model.UpkeepCost - (float) def.UpkeepCost;
            controller.EnoughCurrencyCheck(def);

            Assert.AreEqual(expected, controller.Faction.CurrencyController.Model.UpkeepCost);
        }
    }
}