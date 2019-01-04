using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.GamePackage.EventArgs;
using kbs2.World;
using kbs2.World.World;
using kbs2.WorldEntity.Location.LocationMVC;
using kbs2.WorldEntity.Unit;
using kbs2.WorldEntity.Unit.MVC;
using kbs2.WorldEntity.WorldEntitySpawner;
using Microsoft.Xna.Framework;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class EntitySpawnerTests
    {
        [Test]
        public void Test_WhenSpawnUnitInvoked_ShouldSpawnUnit()
        {
            //    Arrange


            Mock<UnitController> unitMock = new Mock<UnitController>(new UnitDef());
            Mock<GameController> gameMock = new Mock<GameController>(GameSpeed.Regular, GameState.Paused);
            Mock<Faction_Controller> factionMock = new Mock<Faction_Controller>("test_faction", gameMock.Object);

            UnitController unit = unitMock.Object;
            GameController game = gameMock.Object;
            GameModel gameModel = new GameModel
            {
                World = new WorldController
                {
                    WorldModel = new WorldModel()
                }
            };

            game.GameModel = gameModel;

            unit.LocationController = new Location_Controller(game.GameModel.World, 0, 0);

            factionMock.Setup(factionParam => factionParam.RegisterUnit(null)).Verifiable();

            Faction_Controller faction = factionMock.Object;

            unit.Faction = faction;

            EntitySpawner spawner = new EntitySpawner(game);

            //    Act
            spawner.SpawnUnit(unit, new Coords());

            //    Assert

            unitMock.VerifyAll();
            gameMock.VerifyAll();
        }
    }
}