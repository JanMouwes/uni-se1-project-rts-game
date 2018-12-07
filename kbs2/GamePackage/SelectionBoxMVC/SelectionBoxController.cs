using kbs2.World.Structs;
using Microsoft.Xna.Framework;
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

        public void DrawSelectionBox(MouseState CurMouseState, Matrix matrix, int tileSize)
        {
            if(CurMouseState.LeftButton == ButtonState.Pressed && BoxModel.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                BoxView.Coords = new FloatCoords()
                {
                    x =
                    CalcMapPosOf(
                        new Vector2(CurMouseState.X, CurMouseState.Y),
                        matrix,
                        tileSize
                    ).X,
                    y =
                   CalcMapPosOf(
                        new Vector2(CurMouseState.X, CurMouseState.Y),
                        matrix,
                        tileSize
                    ).Y
                };
                    
            }

            if (CurMouseState.LeftButton == ButtonState.Pressed && BoxModel.PreviousMouseState.LeftButton == ButtonState.Pressed)
            {
                SetSelectionBoxWH(
                    CalcMapPosOf(
                        new Vector2(CurMouseState.X, CurMouseState.Y), 
                        matrix, 
                        tileSize
                    ).X, 
                    CalcMapPosOf(
                        new Vector2(CurMouseState.X, CurMouseState.Y),
                        matrix, 
                        tileSize
                    ).Y
                );
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

        public Vector2 CalcMapPosOf(Vector2 position, Matrix matrix, int tileSize)
        {
            Vector2 vector = Vector2.Transform(position, Matrix.Invert(matrix)) / tileSize;
            return vector;
        }

        public RectangleF ReturnSelectionBox() => new RectangleF(BoxModel.SelectionBox.X, BoxModel.SelectionBox.Y, BoxView.Width, BoxView.Height);
    }
}
