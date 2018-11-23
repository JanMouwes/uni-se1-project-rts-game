using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace kbs2.Desktop.View.Camera
{
    public class CameraController : Camera2D
    {
        public CameraController(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            cameraModel = new CameraModel();
        }

        public CameraController(ViewportAdapter viewportAdapter) : base(viewportAdapter) { }

        public CameraModel cameraModel { get; set; }
    }
}