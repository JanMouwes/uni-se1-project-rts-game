using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.Resources.Enums;
using kbs2.WorldEntity.Structures.Defs;
using kbs2.WorldEntity.Structures.ResourceFactory;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ResourceFactoryTests
    {
        private ResourceFactoryDef def;
        private Faction_Controller faction;

        [SetUp]
        public void Setup()
        {
            Mock<ResourceFactoryDef> defMock = new Mock<ResourceFactoryDef>();
            def = defMock.Object;
            
            Mock<GameController> gameMock = new Mock<GameController>(GameSpeed.Regular, GameState.Paused);

            Mock<Faction_Controller> factionMock = new Mock<Faction_Controller>("test_faction", gameMock.Object);
            faction = factionMock.Object;
        }

        [TestCase(ResourceType.Stone)]
        [TestCase(ResourceType.Wood)]
        [TestCase(ResourceType.Food)]
        public void Test_ShouldSetResourceType_WhenGivenDef(ResourceType resourceType)
        {
            //    Arrange
            def.ResourceType = resourceType;

            //    Act
            ResourceFactoryController resourceFactory = new ResourceFactoryController(def, faction);

            //    Assert
            Assert.AreEqual(resourceType, resourceFactory.ResourceType);
        }
    }
}