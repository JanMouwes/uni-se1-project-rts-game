using MonoGame.Extended.ViewportAdapters;
using System;
using MonoGame.Extended;

namespace kbs2.Desktop.View.Camera
{
    public class CameraModel
    {
        // Defines the minimum and maximum zoom
        public const float MaximumZoom = 6.0f;
        public const float MinimumZoom = (float) (1.0 / 4.0);

        private Camera2D parentCamera;

        // Defines the default amount of tiles on screen
        public const int DefaultTiles = 30;

        // Defines the speed the camera moves at
        private const float MoveSpeedBase = 4f;

        //    Movement speed, adjusted to zoom levels
        public double MoveSpeed => MoveSpeedBase / Math.Sqrt(parentCamera.Zoom);

        // Keeps track of the previous scrollwheel value to keep track of zoom
        public int PreviousScrollWheelValue;

        // Defines the zoom level
        public float Zoom
        {
            get => parentCamera.Zoom;
            set => parentCamera.Zoom = value;
        }

        public float TileCount =>
            (float) Math.Ceiling((DefaultTiles / parentCamera.Zoom) > 1.0f ? (DefaultTiles) : 1.0);

        public CameraModel(Camera2D parentCamera)
        {
            this.parentCamera = parentCamera;
            parentCamera.MaximumZoom = MaximumZoom;
            parentCamera.MinimumZoom = MinimumZoom;
        }
    }
}