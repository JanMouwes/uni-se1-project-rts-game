using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures;
using kbs2.WorldEntity.Unit;
using kbs2.WorldEntity.Unit.MVC;
using kbs2.WorldEntity.WorldEntitySpawner;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class GameActionTests
    {
        private GameController gameController;
        private Faction_Controller factionController;

        [SetUp]
        public void Setup()
        {
            //    World
            Mock<WorldController> worldControllerMock = new Mock<WorldController>();
            //worldControllerMock.Setup(worldController => worldController.)


            //    GameController
            Mock<GameController> gameControllerMock = new Mock<GameController>(new object[] {GameSpeed.Regular, GameState.Running});
            gameController = gameControllerMock.Object;

            //    Faction
            factionController = new Faction_Controller("test-faction", gameController);
        }


        [TestCase(1, 1)]
        [TestCase(2, 2)]
        public void Test_ShouldSpawnUnit_WhenExecuteInvoked(int x, int y)
        {
            //    Arrange
            Mock<EntitySpawner> spawner = new Mock<EntitySpawner>(MockBehavior.Loose, new object[] {gameController});


            spawner.Setup(entitySpawner => entitySpawner.SpawnUnit(It.IsAny<UnitController>(), It.IsAny<Coords>())).Verifiable();

            gameController.Spawner = spawner.Object;

            Coords coords = new Coords()
            {
                x = x,
                y = y
            };

            UnitDef unitDef = Mock.Of<UnitDef>();

            Mock<SpawnActionDef> actionDef = new Mock<SpawnActionDef>(new object[] {(uint) 10, "image", unitDef});

            actionDef.SetupGet(def => def.SpawnableDef).Returns(() => unitDef);

            Mock<ITargetable> target = new Mock<ITargetable>();

            target.Setup(targetable => targetable.FloatCoords).Returns(() => (FloatCoords) coords);

            SpawnAction spawnAction = new SpawnAction(actionDef.Object, gameController, factionController);

            //    Act
            spawnAction.Execute(target.Object);

            //    Assert

            spawner.VerifyAll();
        }

        [TestCase(1, 1)]
        [TestCase(2, 2)]
        public void Test_ShouldSpawnStructure_WhenExecuteInvoked(int x, int y)
        {
            //    Arrange
            Mock<EntitySpawner> spawner = new Mock<EntitySpawner>(MockBehavior.Loose, new object[] {gameController});

            spawner.Setup(entitySpawner => entitySpawner.SpawnStructure(It.IsAny<Coords>(), It.IsAny<IStructure<IStructureDef>>())).Verifiable();

            gameController.Spawner = spawner.Object;

            Coords coords = new Coords()
            {
                x = x,
                y = y
            };

            BuildingDef buildingDef = Mock.Of<BuildingDef>();

            Mock<SpawnActionDef> actionDef = new Mock<SpawnActionDef>(new object[] {(uint) 10, "image", buildingDef});

            actionDef.SetupGet(def => def.SpawnableDef).Returns(() => buildingDef);

            Mock<ITargetable> target = new Mock<ITargetable>();

            target.Setup(targetable => targetable.FloatCoords).Returns(() => (FloatCoords) coords);

            SpawnAction spawnAction = new SpawnAction(actionDef.Object, gameController, factionController);

            //    Act
            spawnAction.Execute(target.Object);

            //    Assert

            spawner.VerifyAll();
        }


        /*
        [TestCase(1, 1, 3, 4, 23, 24)]
        [TestCase(1, 2, 3, 4, 23, 44)]
        [TestCase(3, 2, 3, 4, 63, 44)]
        public void Test_Chunk_Generates_CorrectCellCoords(int chunkX, int chunkY, int relativeCellX, int relativeCellY, int expectedCellX, int expectedCellY)
        {
            //    Arrange
            WorldChunkController chunk;
            Coords chunkCoords = new Coords() { x = chunkX, y = chunkY };
            Coords relativeCellCoords = new Coords() { x = relativeCellX, y = relativeCellY };
            Coords expectedCellCoords = new Coords() { x = expectedCellX, y = expectedCellY };

            //    Act
            chunk = WorldChunkFactory.ChunkOfDefaultTerrain(chunkCoords);


            //    Assert
            WorldCellController cell = chunk.WorldChunkModel.grid[relativeCellCoords.x, relativeCellCoords.y];

            Assert.AreEqual(cell.worldCellModel.RealCoords, expectedCellCoords);
        }
        */
    }
}