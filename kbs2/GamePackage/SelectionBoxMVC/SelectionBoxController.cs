using kbs2.World.Structs;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage.SelectionBoxMVC
{
    public class SelectionBoxController
    {
        public SelectionBoxModel BoxModel { get; set; }
        public SelectionBoxView BoxView { get; set; }
        
        public SelectionBoxController()
        {
            BoxModel = new SelectionBoxModel();
            BoxView = new SelectionBoxView();
        }

        public void DrawSelectionBox(MouseState CurMouseState)
        {
            if(CurMouseState.LeftButton == ButtonState.Pressed && BoxModel.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                BoxModel.InitXYCoord = new FloatCoords() { x = CurMouseState.X, y = CurMouseState.Y };
            }

            if (CurMouseState.LeftButton == ButtonState.Pressed && BoxModel.PreviousMouseState.LeftButton == ButtonState.Pressed)
            {
                SetSelectionBoxWH(CurMouseState.X, CurMouseState.Y);
            }

            if(CurMouseState.LeftButton == ButtonState.Released && BoxModel.PreviousMouseState.LeftButton == ButtonState.Pressed)
            {
                BoxModel.SelectionBox = ReturnSelectionBox();
            }

            if(CurMouseState.LeftButton == ButtonState.Released && BoxModel.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                ResetSelectionBox();
            }

            BoxModel.PreviousMouseState = CurMouseState;
        }

        public void ResetSelectionBox()
        {
            BoxView.Coords = new FloatCoords() { x = -1, y = -1 };
            BoxView.Width = 0;
            BoxView.Height = 0;
        }

        public void SetSelectionBoxWH(float width, float height)
        {
            BoxView.Width = width;
            BoxView.Height = height;
        }

        public RectangleF ReturnSelectionBox() => new RectangleF(BoxModel.InitXYCoord.x, BoxModel.InitXYCoord.y, BoxView.Width, BoxView.Height);

        /*public void DrawSelectionBox()
        {
            if (CurMouseState.LeftButton == ButtonState.Pressed && Model.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                Model.SelectionBox = new RectangleF(CurMouseState.X, CurMouseState.Y, 0, 0);
                Vector2 boxPosition = new Vector2(CurMouseState.X, CurMouseState.Y);
                Vector2 worldPosition = Vector2.Transform(boxPosition, Matrix.Invert(viewMatrix));
                View.Coords = new FloatCoords() { x = worldPosition.X, y = worldPosition.Y };
                View.Width = 0;
                View.Height = 0;
                AdjustViewBox(CurMouseState, viewMatrix, tileSize, zoom);
            }

            if (CurMouseState.LeftButton == ButtonState.Pressed)
            {
                Model.SelectionBox = new RectangleF(Model.SelectionBox.X, Model.SelectionBox.Y, CurMouseState.X - Model.SelectionBox.X, CurMouseState.Y - Model.SelectionBox.Y);
                View.Coords = new FloatCoords() { x = Model.SelectionBox.X, y = Model.SelectionBox.Y };
                View.Width = CurMouseState.X - Model.SelectionBox.X;
                View.Height = CurMouseState.Y - Model.SelectionBox.Y;
                AdjustViewBox(CurMouseState, viewMatrix, tileSize, zoom);
            }

            if (CurMouseState.LeftButton == ButtonState.Released && Model.PreviousMouseState.LeftButton == ButtonState.Pressed)
                CheckClickedBox(List, viewMatrix, tileSize, zoom);

            if (CurMouseState.LeftButton == ButtonState.Released && Model.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                Model.SelectionBox = new RectangleF(-1f, -1f, 0f, 0f);
                View.Coords = new FloatCoords() { x = -1, y = -1 };
                View.Width = 0;
                View.Height = 0;
                AdjustViewBox(CurMouseState, viewMatrix, tileSize, zoom);
            }

            Model.PreviousMouseState = CurMouseState;
        }*/
    }
}
