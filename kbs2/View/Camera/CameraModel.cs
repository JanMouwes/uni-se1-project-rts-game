using MonoGame.Extended.ViewportAdapters;
using System;
using MonoGame.Extended;

namespace kbs2.Desktop.View.Camera
{
    public class CameraModel
    {
        // Defines the minimum and maximum zoom
        public const float MaximumZoom = 4.0f;
        public const float MinimumZoom = (float) (1.0 / 5.0);

        // Defines the default amount of tiles on screen
        public const int DefaultTiles = 30;

        // Defines the speed the camera moves at
        private const float MoveSpeedBase = 6f;

        // Reference to parent camera
        private Camera2D parentCamera;

        // Movement speed, adjusted to zoom levels
        public double MoveSpeed => MoveSpeedBase / Math.Sqrt(parentCamera.Zoom);

        // Keeps track of the previous scrollwheel value to keep track of zoom
        public int PreviousScrollWheelValue;

        // Defines the zoom level
        public float Zoom
        {
            get => parentCamera.Zoom;
            set => parentCamera.Zoom = value;
        }

        // The current (horizontal) tileCount
        public float TileCount =>
            (float) Math.Ceiling((DefaultTiles / parentCamera.Zoom) > 1.0f ? (DefaultTiles) : 1.0);

        // Constructor
        public CameraModel(Camera2D parentCamera) => this.parentCamera = parentCamera;
       
    }
}