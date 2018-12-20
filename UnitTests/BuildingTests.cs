using kbs2.Faction.FactionMVC;
using kbs2.World;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingMVC;
using kbs2.WorldEntity.Interfaces;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class BuildingTests
    {

        IStructureDef def;
        BuildingController controller;

        [SetUp]
        public void TestSetUp()
        {
            def = new BuildingDef
            {
                Cost = 50,
                UpkeepCost = 1.4,
            };
            controller = new BuildingController(def)
            {
                Faction = new Faction_Controller("test-faction"),
                StartCoords = new Coords()
            };
        }

        [Test]
        public void EnoughCurrencyCheck_RemoveCurrency_Test()
        {
            float expected = controller.Faction.currency_Controller.model.currency - (float)def.Cost;
            controller.EnoughCurrencyCheck(def);

            Assert.AreEqual(expected, controller.Faction.currency_Controller.model.currency);
        }

        [Test]
        public void EnoughCurrencyCheck_AddUpkeepCosts_Test()
        {
            float expected = controller.Faction.currency_Controller.model.UpkeepCost - (float)def.UpkeepCost;
            controller.EnoughCurrencyCheck(def);

            Assert.AreEqual(expected, controller.Faction.currency_Controller.model.UpkeepCost);
        }

    }
}