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

            base.MinimumZoom = CameraModel.MinimumZoom;
            base.MaximumZoom = CameraModel.MaximumZoom;
        }

        public CameraController(ViewportAdapter viewportAdapter) : this(viewportAdapter.GraphicsDevice)
        {
        }

        public CameraModel cameraModel { get; set; }
    }
}