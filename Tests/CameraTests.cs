using kbs2.Desktop.View.Camera;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Tests
{
    [TestFixture]
    public class CameraTests
    {
        private GraphicsDevice graphicsDevice;

        [SetUp]
        public void TestSetUp()
        {
        }

        //    Not testable, too integrated - Jan
        /*[TestCase(1)]
        [TestCase(2)]
        [TestCase(3.5f)]
        public void Test_SetZoom_Updates_Model(float zoom)
        {
            //    Arrange
            CameraController cameraController = new CameraController(graphicsDevice);

            //    Act
            cameraController.Zoom = zoom;

            //    Assert
            Assert.AreEqual(cameraController.CameraModel.Zoom, zoom);
        }*/
    }
}