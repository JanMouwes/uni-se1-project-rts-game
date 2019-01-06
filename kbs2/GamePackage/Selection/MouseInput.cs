using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Interfaces;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Structs;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework.Input;

namespace kbs2.GamePackage
{
    public enum MouseButton
    {
        Left,
        Right
    }

    public class MouseInput
    {
        public MouseState PreviousMouseState { get; set; }
        public ButtonState MouseInputStatus { get; set; }
        public GameController game { get; set; }
        public Selection_Controller Selection { get; set; }

        public MouseInput(GameController game)
        {
            this.game = game;
            PreviousMouseState = new MouseState();
            Selection = new Selection_Controller(game, "PurpleLine");
        }


        public void OnMouseStateChange(object sender, EventArgsWithPayload<MouseState> mouseEvent)
        {
            if (mouseEvent.Value.LeftButton != PreviousMouseState.LeftButton)
            {
                MouseInputStatus = mouseEvent.Value.LeftButton;
                MouseState temp = Mouse.GetState();
                GuiOrMap(new FloatCoords {x = temp.X, y = temp.Y}, mouseEvent.Value, MouseButton.Left);
            }

            if (mouseEvent.Value.RightButton != PreviousMouseState.RightButton)
            {
                MouseInputStatus = mouseEvent.Value.RightButton;
                MouseState temp = Mouse.GetState();
                GuiOrMap(new FloatCoords {x = temp.X, y = temp.Y}, mouseEvent.Value, MouseButton.Right);
            }


            PreviousMouseState = mouseEvent.Value;
        }

        public void GuiOrMap(FloatCoords mouseCoords, MouseState mouseState, MouseButton activeButton)
        {
            List<IViewImage> clickedGuiItems = (from item in game.GameModel.GuiItemList
                where mouseCoords.x >= item.Coords.x
                      && mouseCoords.y >= item.Coords.y
                      && mouseCoords.x <= item.Coords.x + item.Width
                      && mouseCoords.y <= item.Coords.y + item.Height
                select item).ToList();

            if (clickedGuiItems.Count > 0)
            {
                //TODO geef door aan gui
                Console.WriteLine("gui Click");
            }
            else
            {
                switch (activeButton)
                {
                    case MouseButton.Left:
                        switch (mouseState.LeftButton)
                        {
                            //TODO LeftButtonPressed?.Invoke()
                            //TODO LeftButtonReleased?.Invoke()
                            case ButtonState.Pressed:


                                Selection.ButtonPressed(mouseCoords);
                                break;
                            case ButtonState.Released:
                                game.MapActionSelector.Clear();

                                Selection.ButtonRelease(Keyboard.GetState().IsKeyDown(Keys.LeftShift));
                                break;
                        }

                        break;
                    case MouseButton.Right:
                        switch (mouseState.RightButton)
                        {
                            //TODO RightButtonPressed?.Invoke()
                            //TODO RightButtonReleased?.Invoke()
                            case ButtonState.Pressed:
                                Selection.move(Keyboard.GetState().IsKeyDown(Keys.LeftShift));
                                break;
                            case ButtonState.Released:
                            {
                                FloatCoords cellCoords = WorldPositionCalculator.DrawCoordsToCellFloatCoords((FloatCoords) WorldPositionCalculator.TransformWindowCoords((Coords) mouseCoords, game.Camera.GetViewMatrix()), game.GameView.TileSize);
                                if (PreviousMouseState.RightButton == ButtonState.Pressed) game.SelectedMapAction?.Execute(game.GameModel.World.GetCellFromCoords((Coords) cellCoords));
                                break;
                            }
                        }

                        break;
                }
            }
        }
    }
}