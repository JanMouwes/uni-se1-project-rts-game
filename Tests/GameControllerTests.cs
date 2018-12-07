using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.GamePackage;
using kbs2.GamePackage.EventArgs;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    class GameControllerTests
    {
        [TestCase(GameSpeed.Regular , GameState.Running, GameSpeed.Regular, GameState.Running)]
        public void Test_GameController_Registers_Values(GameSpeed gameSpeed, GameState gameState, GameSpeed expectedGameSpeed, GameState expectedGameState)
        {
            // Arrange
            GameController controller;

            // Act
            controller = new GameController(gameSpeed, gameState);

            // Assert
            Assert.AreEqual(controller.GameSpeed, expectedGameSpeed);
            Assert.AreEqual(controller.GameState, expectedGameState);
        }
 
        [TestCase(GameSpeed.Regular, GameState.Running, GameSpeed.Regular, GameState.Running)]
        public void Test_Events_Are_Raised(GameSpeed gameSpeed, GameState gameState, GameSpeed expectedGameSpeed, GameState expectedGameState)
        {
            List<GameSpeed> receivedSpeedEvents = new List<GameSpeed>();

            List<GameState> receivedStateEvents = new List<GameState>();

            GameController controller = new GameController(GameSpeed.Regular, GameState.Running);

            controller.GameSpeedChange += delegate (object sender, GameSpeedEventArgs e)
            {
                receivedSpeedEvents.Add(e.GameSpeed);
            };

            controller.GameStateChange += delegate (object sender, GameStateEventArgs e)
            {
                receivedStateEvents.Add(e.GameState);
            };

            controller.GameSpeed = gameSpeed;
            controller.GameState = gameState;
            Assert.AreEqual(receivedSpeedEvents[0], expectedGameSpeed);
            Assert.AreEqual(receivedStateEvents[0], expectedGameState);
        }

    }
}
