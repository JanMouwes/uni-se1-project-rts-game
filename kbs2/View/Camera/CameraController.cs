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
        public CameraModel CameraModel { get; set; }

        public CameraController(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            CameraModel = new CameraModel(this);

            base.MinimumZoom = CameraModel.MinimumZoom;
            base.MaximumZoom = CameraModel.MaximumZoom;
        }

        public new float Zoom
        {
            get => base.Zoom;
            set => base.Zoom = value;
        }

        public CameraController(ViewportAdapter viewportAdapter) : this(viewportAdapter.GraphicsDevice)
        {
        }

        /// <summary>
        /// Checks keys for moving the camera and updates the camera.
        /// </summary>
        public void MoveCamera()
        {
            Vector2 moveVelocity = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                moveVelocity += new Vector2((float) CameraModel.MoveSpeed, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                moveVelocity += new Vector2(0, (float) CameraModel.MoveSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                moveVelocity += new Vector2((float) -CameraModel.MoveSpeed, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                moveVelocity += new Vector2(0, (float)-CameraModel.MoveSpeed);

            UpdateZoom();

            Move(moveVelocity);
        }

        /// <summary>
        /// Checks for zoom updates and changes zoom accordingly.
        /// </summary>
        public void UpdateZoom()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.B)) CameraModel.Zoom = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.G)) ZoomOut((float)0.05);
            if (Keyboard.GetState().IsKeyDown(Keys.H)) ZoomIn((float)0.05);

            int currentScrollWheelValue = Mouse.GetState().ScrollWheelValue;
            int scrollChange = CameraModel.PreviousScrollWheelValue - currentScrollWheelValue;
            double zoomChange = scrollChange / 18000.0;

            ZoomOut((float) zoomChange);

            CameraModel.PreviousScrollWheelValue = currentScrollWheelValue;
        }
    }
}