using System.Collections.Generic;
using System.Linq;
using kbs2.Actions.ActionTabActions;
using kbs2.GamePackage.EventArgs;
using kbs2.utils;
using kbs2.View.GUI;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
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
            List<IGuiViewImage> clickedGuiItems = (from item in game.GameModel.GuiItemList
                where mouseCoords.x >= item.Coords.x
                      && mouseCoords.y >= item.Coords.y
                      && mouseCoords.x <= item.Coords.x + item.Width
                      && mouseCoords.y <= item.Coords.y + item.Height
                orderby item.ZIndex descending
                select item).ToList();


            switch (activeButton)
            {
                case MouseButton.Left:
                    switch (mouseState.LeftButton)
                    {
                        //TODO LeftButtonPressed?.Invoke()
                        //TODO LeftButtonReleased?.Invoke()
                        case ButtonState.Pressed:
                            if (clickedGuiItems.Any())
                            {
                                if (PreviousMouseState.LeftButton != ButtonState.Released) return;
                                clickedGuiItems.First().Click(mouseState);
                                return;
                            }

                            Selection.ButtonPressed(mouseCoords);

                            game.MapActionSelector.Clear();
                            break;
                        case ButtonState.Released:
                            if (!clickedGuiItems.Any())
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
                            if (PreviousMouseState.RightButton != ButtonState.Released) break;
                            if (clickedGuiItems.Any())
                            {
                                if (PreviousMouseState.LeftButton != ButtonState.Released) return;
                                clickedGuiItems.First().Click(mouseState);
                                return;
                            }

                            FloatCoords cellCoords = WorldPositionCalculator.DrawCoordsToCellFloatCoords(WorldPositionCalculator.TransformWindowCoords((Coords) mouseCoords, game.Camera.GetViewMatrix()), game.GameView.TileSize);

                            IWorldEntity worldEntity;
                            ITargetable target = game.CellContainsIWorldEntity(cellCoords, out worldEntity) ? (ITargetable) worldEntity : game.GameModel.World.GetCellFromCoords((Coords) cellCoords);
                            if (game.MapActionSelector.IsMapActionSelected() && game.MapActionSelector.SelectedMapAction.IsValidTarget(target))
                            {
                                foreach (IGameActionHolder selectedItem in Selection.SelectedItems)
                                {
                                    if (selectedItem.GameActions.Any(item => item is SelectMapAction_GameAction mapActionGameAction
                                                                             && mapActionGameAction.MapAction.IsValidTarget(target))) ;
                                    selectedItem.GameActions.OfType<SelectMapAction_GameAction>().First().MapAction.TryExecute(target);
                                }

                                game.MapActionSelector.SelectedMapAction.TryExecute(target);
                            }
                            else
                            {
                                Selection.move(Keyboard.GetState().IsKeyDown(Keys.LeftShift));
                            }


                            break;
                        case ButtonState.Released:
                            break;
                    }

                    break;
            }
        }
    }
}