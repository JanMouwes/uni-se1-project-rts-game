using System;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures;
using kbs2.WorldEntity.Structures.BuildingMVC;
using kbs2.WorldEntity.Structures.Defs;
using kbs2.WorldEntity.Structures.ResourceFactory;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class BuildingFactoryTests
    {
        private Faction_Controller faction;
        private GameController game;

        [SetUp]
        public void Setup()
        {
            Mock<GameController> gameMock = new Mock<GameController>(GameSpeed.Regular, GameState.Paused);
            game = gameMock.Object;

            Mock<Faction_Controller> factionMock = new Mock<Faction_Controller>("test_faction", game);
            faction = factionMock.Object;
        }

        [Test]
        public void Test_ShouldThrowException_WhenGivenNull()
        {
            //    Arrange

            //    Act

            //    Assert
            Assert.Throws<ArgumentNullException>(() =>
                {
                    BuildingFactory factory = new BuildingFactory(null, game);
                }
            );
        }

        [TestCase(typeof(BuildingDef), typeof(BuildingController))]
        [TestCase(typeof(ResourceFactoryDef), typeof(ResourceFactoryController))]
        public void Test_ShouldReturnBuilding_WhenGivenDef(Type defType, Type expectedType)
        {
            //    Arrange
            BuildingFactory buildingFactory = new BuildingFactory(faction, game);
            IStructureDef def = (IStructureDef) Activator.CreateInstance(defType);


            //    Act
            IStructure<IStructureDef> building = buildingFactory.CreateNewBuilding(def);

            //    Assert
            Assert.AreEqual(expectedType, building.GetType());
        }
    }
}