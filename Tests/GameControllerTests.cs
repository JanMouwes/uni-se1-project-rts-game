using kbs2.GamePackage;
using kbs2.GamePackage.Game;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
