using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

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

        /// <summary>
        /// Checks keys for moving the camera and updates the camera.
        /// </summary>
        public void MoveCamera()
        {
            Vector2 moveVelocity = Vector2.Zero;

            const float moveSpeed = 1.5f;

            if (Keyboard.GetState().IsKeyDown(Keys.Right)) moveVelocity += new Vector2(moveSpeed, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) moveVelocity += new Vector2(0, moveSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) moveVelocity += new Vector2(-moveSpeed, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) moveVelocity += new Vector2(0, -moveSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.G)) ZoomOut((float)0.1);
            if (Keyboard.GetState().IsKeyDown(Keys.H)) ZoomIn((float)0.1);

            UpdateZoom();

            Move(moveVelocity);
        }

        /// <summary>
        /// Checks for zoom updates and changes zoom accordingly.
        /// </summary>
        public void UpdateZoom()
        {
            int currentScrollWheelValue = Mouse.GetState().ScrollWheelValue;
            int scrollChange = cameraModel.PreviousScrollWheelValue - currentScrollWheelValue;
            double zoomChange = scrollChange / 36000.0;

            ZoomOut((float)zoomChange);

            cameraModel.PreviousScrollWheelValue = currentScrollWheelValue;

            if (Math.Abs(zoomChange) < 0.000001) return;
        }
    }
}