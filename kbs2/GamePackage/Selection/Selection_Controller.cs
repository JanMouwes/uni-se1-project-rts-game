using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Selection;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage
{
    public class Selection_Controller
    {
        public Selection_Model Model { get; set; }
        public Selection_View LeftView { get; set; }
        public Selection_View RightView { get; set; }
        public Selection_View TopView { get; set; }
        public Selection_View BottomView { get; set; }
        public GameController gameController { get; set; }

        public delegate void OnSelectionChanged(object sender, EventArgsWithPayload<List<IHasActions>> eventArgs);

        public event OnSelectionChanged onSelectionChanged;


        public List<IHasActions> SelectedItems { get; set; }
        private EventArgsWithPayload<List<IHasActions>> args { get; set; }

        private FloatCoords FirstPoint;
        private FloatCoords TopLeft;
        private FloatCoords BottomRight;
        private bool active;

        // constructor
        public Selection_Controller(GameController game, string lineTexture)
        {
            Model = new Selection_Model();
            SelectedItems = new List<IHasActions>();
            gameController = game;

            args = new EventArgsWithPayload<List<IHasActions>>(SelectedItems);

            LeftView = new Selection_View();
            // width of the selection border
            LeftView.Width = 4f / gameController.gameView.TileSize;

            RightView = new Selection_View();
            // width of the selection border
            RightView.Width = 4f / gameController.gameView.TileSize;

            TopView = new Selection_View();
            // width of the selection border
            TopView.Height = 4f / gameController.gameView.TileSize;

            BottomView = new Selection_View();
            // width of the selection border
            BottomView.Height = 4f / gameController.gameView.TileSize;
        }

        // name explains this I guess
        public void ButtonPressed(FloatCoords mouseCoords)
        {
            //Save current mouse location/coords
            FirstPoint = WorldPositionCalculator.DrawCoordsToCellFloatCoords(WorldPositionCalculator.TransformWindowCoords((Coords) mouseCoords, gameController.camera.GetViewMatrix()), gameController.gameView.TileSize);
            //enable drawfunction of selectionbox
            active = true;
        }

        // name explains this I guess
        public void ButtonRelease(bool CTRL)
        {
            UpdateSelection(CTRL);
            //disable drawfunction
            active = false;
        }

        public void Update(object sender, OnTickEventArgs eventArgs)
        {
            if (active)
            {
                MouseState temp = Mouse.GetState();
                Coords tempcoords = new Coords {x = temp.X, y = temp.Y};
                // saves current mouse location 
                FloatCoords SecondPoint = WorldPositionCalculator.DrawCoordsToCellFloatCoords(WorldPositionCalculator.TransformWindowCoords(tempcoords, gameController.camera.GetViewMatrix()), gameController.gameView.TileSize);
                //standardices coords to topleft and bottomright 
                SetCoords(FirstPoint, SecondPoint);
                DrawBox();
            }
        }

        //standardices coords to topleft and bottomright 
        public void SetCoords(FloatCoords firstCoords, FloatCoords secondCoords)
        {
            TopLeft.x = firstCoords.x < secondCoords.x ? firstCoords.x : secondCoords.x;
            TopLeft.y = firstCoords.y < secondCoords.y ? firstCoords.y : secondCoords.y;

            BottomRight.x = firstCoords.x > secondCoords.x ? firstCoords.x : secondCoords.x;
            BottomRight.y = firstCoords.y > secondCoords.y ? firstCoords.y : secondCoords.y;
        }

        // get all items in selectionbox
        public void UpdateSelection(bool isQueueKeyPressed)
        {
            if (isQueueKeyPressed && SelectedItems.OfType<IHasActions>().Any())
            {
                SelectedItems.AddRange(SelectBuildings());
            }
            else
            {
                List<IHasActions> selection = SelectUnits();

                SelectedItems = (selection.Count > 0)
                    ? isQueueKeyPressed
                        ? SelectedItems.Union(selection).ToList()
                        : selection
                    : SelectBuildings();
            }

            args = new EventArgsWithPayload<List<IHasActions>>(SelectedItems);
            onSelectionChanged?.Invoke(this, args);
        }

        // get all buildings from selectionbox
        public List<IHasActions> SelectBuildings()
        {
            List<IHasActions> selected;
            if (!(DistanceCalculator.DiagonalDistance(TopLeft, BottomRight) < 0.5))
                return (from Item in gameController.PlayerFaction.FactionModel.Buildings
                    where TopLeft.x <= Item.Model.TopLeft.x + (Item.View.Width / 2)
                          && TopLeft.y <= Item.Model.TopLeft.y + (Item.View.Height / 2)
                          && BottomRight.x >= Item.Model.TopLeft.x + (Item.View.Width / 2)
                          && BottomRight.y >= Item.Model.TopLeft.y + (Item.View.Height / 2)
                    select Item).Cast<IHasActions>().ToList();

            selected = new List<IHasActions>();
            WorldCellModel cell = gameController.gameModel.World.GetCellFromCoords((Coords) TopLeft).worldCellModel;
            if (cell.BuildingOnTop != null)
            {
                selected.Add((IHasActions) cell.BuildingOnTop);
            }

            return selected;
        }

        // selects units in selectionbox
        public List<IHasActions> SelectUnits()
        {
            List<Unit_Controller> Selected;
            if (DistanceCalculator.DiagonalDistance(TopLeft, BottomRight) < 0.5)
            {
                Selected = (from Item in gameController.PlayerFaction.FactionModel.Units
                    where DistanceCalculator.DiagonalDistance(TopLeft, Item.LocationController.LocationModel.floatCoords) < 0.5
                          || DistanceCalculator.DiagonalDistance(TopLeft, Item.LocationController.LocationModel.floatCoords) < Item.UnitView.Height
                    select Item).ToList();
            }
            else
            {
                Selected = (from Item in gameController.PlayerFaction.FactionModel.Units
                    where TopLeft.x <= Item.LocationController.LocationModel.coords.x + (Item.UnitView.Width / 2)
                          && TopLeft.y <= Item.LocationController.LocationModel.coords.y + (Item.UnitView.Height / 2)
                          && BottomRight.x >= Item.LocationController.LocationModel.coords.x + (Item.UnitView.Width / 2)
                          && BottomRight.y >= Item.LocationController.LocationModel.coords.y + (Item.UnitView.Height / 2)
                    select Item).ToList();
            }

            return Selected.Cast<IHasActions>().ToList();
        }

        // give movecommand to all units in selection
        public void move(bool CTRL)
        {
            MouseState temp = Mouse.GetState();
            Coords tempcoords = new Coords {x = temp.X, y = temp.Y};
            FloatCoords target = WorldPositionCalculator.DrawCoordsToCellFloatCoords(WorldPositionCalculator.TransformWindowCoords(tempcoords, gameController.camera.GetViewMatrix()), gameController.gameView.TileSize);
            foreach (IHasActions unit in SelectedItems)
            {
                if (unit.GetType() == typeof(Unit_Controller))
                {
                    ((IMoveable) unit).MoveTo(target, CTRL);
                }
            }
        }

        //draws the selectionbox
        public void DrawBox()
        {
            LeftView.Coords = TopLeft;
            LeftView.Height = Math.Abs(BottomRight.y - TopLeft.y);

            RightView.Coords = new FloatCoords {x = BottomRight.x - RightView.Width, y = TopLeft.y};
            RightView.Height = Math.Abs(BottomRight.y - TopLeft.y);

            TopView.Coords = TopLeft;
            TopView.Width = Math.Abs(BottomRight.x - TopLeft.x);

            BottomView.Coords = new FloatCoords {x = TopLeft.x, y = BottomRight.y - BottomView.Height};
            BottomView.Width = Math.Abs(BottomRight.x - TopLeft.x);


            gameController.gameModel.ItemList.Add(LeftView);
            gameController.gameModel.ItemList.Add(RightView);
            gameController.gameModel.ItemList.Add(TopView);
            gameController.gameModel.ItemList.Add(BottomView);
        }
    }
}