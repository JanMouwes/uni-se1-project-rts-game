using MonoGame.Extended.ViewportAdapters;
using System;

namespace kbs2.Desktop.View.Camera
{
    public class CameraModel
    {
        // Defines the minimum and maximum zoom
        public const float MaximumZoom = 8.0f;
        public const float MinimumZoom = (float)(1.0 / 6.0);

        // Defines the default amount of tiles on screen
        public const int DefaultTiles = 30;

        // Keeps track of the previous scrollwheel value to keep track of zoom
        public int PreviousScrollWheelValue;

        public float Zoom = 1;

        public float TileCount => (float)Math.Ceiling((DefaultTiles / Zoom) > 1.0 ? (DefaultTiles / Zoom) : 1);
    }
}