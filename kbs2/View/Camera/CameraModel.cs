using MonoGame.Extended.ViewportAdapters;

namespace kbs2.Desktop.View.Camera
{
    public class CameraModel
    {
        private float maximumZoom = float.MaxValue;
        private readonly ViewportAdapter viewportAdapter;
        private float minimumZoom;
        private float zoom;

        public CameraModel(ViewportAdapter viewportAdapter)
        {
            this.viewportAdapter = viewportAdapter;
        }
    }
}