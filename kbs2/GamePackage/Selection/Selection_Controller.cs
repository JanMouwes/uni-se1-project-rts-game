using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.GamePackage.EventArgs;
using kbs2.GamePackage.Selection;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit.MVC;
using Microsoft.Xna.Framework.Input;

namespace kbs2.GamePackage
{
    public delegate void OnSelectionChangedDelegate(object sender, EventArgsWithPayload<List<IGameActionHolder>> eventArgs);

    public class Selection_Controller
    {
        private Selection_Model Model { get; set; }

        private Selection_View LeftView { get; set; }
        private Selection_View RightView { get; set; }
        private Selection_View TopView { get; set; }
        private Selection_View BottomView { get; set; }

        private GameController Game { get; set; }


        public event OnSelectionChangedDelegate OnSelectionChanged;


        public List<IGameActionHolder> SelectedItems { get; set; }

        private FloatCoords firstPoint;
        private FloatCoords topLeft;
        private FloatCoords bottomRight;
        private bool active;

        // constructor
        public Selection_Controller(GameController game, string lineTexture)
        {
            Model = new Selection_Model();
            SelectedItems = new List<IGameActionHolder>();
            this.Game = game;

            LeftView = new Selection_View();
            // width of the selection border
            LeftView.Width = 4f / this.Game.GameView.TileSize;

            RightView = new Selection_View();
            // width of the selection border
            RightView.Width = 4f / this.Game.GameView.TileSize;

            TopView = new Selection_View();
            // width of the selection border
            TopView.Height = 4f / this.Game.GameView.TileSize;

            BottomView = new Selection_View();
            // width of the selection border
            BottomView.Height = 4f / this.Game.GameView.TileSize;
        }

        // name explains this I guess
        public void ButtonPressed(FloatCoords mouseCoords)
        {
            //Save current mouse location/coords
            firstPoint = WorldPositionCalculator.DrawCoordsToCellFloatCoords(WorldPositionCalculator.TransformWindowCoords((Coords) mouseCoords, Game.Camera.GetViewMatrix()), Game.GameView.TileSize);
            //enable drawfunction of selectionbox
            active = true;
        }

        // name explains this I guess
        public void ButtonRelease(bool QueueKeyPressed)
        {
            UpdateSelection(QueueKeyPressed);
            //disable drawfunction
            active = false;
        }

        public void Update(object sender, OnTickEventArgs eventArgs)
        {
            if (!active) return;

            MouseState temp = Mouse.GetState();
            Coords tempcoords = new Coords {x = temp.X, y = temp.Y};
            // saves current mouse location 
            FloatCoords SecondPoint = WorldPositionCalculator.DrawCoordsToCellFloatCoords(WorldPositionCalculator.TransformWindowCoords(tempcoords, Game.Camera.GetViewMatrix()), Game.GameView.TileSize);
            //standardices coords to topleft and bottomright 
            SetCoords(firstPoint, SecondPoint);
            RegisterDrawBox();
        }

        //standardices coords to topleft and bottomright 
        public void SetCoords(FloatCoords firstCoords, FloatCoords secondCoords)
        {
            topLeft.x = firstCoords.x < secondCoords.x ? firstCoords.x : secondCoords.x;
            topLeft.y = firstCoords.y < secondCoords.y ? firstCoords.y : secondCoords.y;

            bottomRight.x = firstCoords.x > secondCoords.x ? firstCoords.x : secondCoords.x;
            bottomRight.y = firstCoords.y > secondCoords.y ? firstCoords.y : secondCoords.y;
        }

        // get all items in selectionbox
        public void UpdateSelection(bool isQueueButtonPressed)
        {
            if (isQueueButtonPressed && SelectedItems.Where((actions => actions != null)).Any())
            {
                SelectedItems.AddRange(SelectBuildings());
            }
            else
            {
                List<IGameActionHolder> selection = SelectUnits();
                SelectedItems = (selection.Count > 0) ? isQueueButtonPressed ? SelectedItems.Union(selection).ToList() : selection : SelectBuildings();
            }

            OnSelectionChanged?.Invoke(this, new EventArgsWithPayload<List<IGameActionHolder>>(SelectedItems));
        }

        // get all buildings from selectionbox
        public List<IGameActionHolder> SelectBuildings()
        {
            List<IGameActionHolder> selected;
            if (DistanceCalculator.DiagonalDistance(topLeft, bottomRight) < 0.5)
            {
                selected = new List<IGameActionHolder>();
                WorldCellModel cell = Game.GameModel.World.GetCellFromCoords((Coords) topLeft).worldCellModel;
                if (cell.BuildingOnTop != null)
                {
                    selected.Add(cell.BuildingOnTop);
                }
            }
            else
            {
                //    TODO optimise
                selected = (from item in Game.PlayerFaction.FactionModel.Buildings
                    where topLeft.x <= item.StartCoords.x + (item.Width / 2)
                          && topLeft.y <= item.StartCoords.y + (item.Height / 2)
                          && bottomRight.x >= item.StartCoords.x + (item.Width / 2)
                          && bottomRight.y >= item.StartCoords.y + (item.Height / 2)
                    select item).Cast<IGameActionHolder>().ToList();
            }

            return selected;
        }

        // selects units in selectionbox
        public List<IGameActionHolder> SelectUnits()
        {
            List<UnitController> selected;
            if (DistanceCalculator.DiagonalDistance(topLeft, bottomRight) < 0.5)
            {
                selected = (from Item in Game.PlayerFaction.FactionModel.Units
                    where DistanceCalculator.DiagonalDistance(topLeft, Item.LocationController.LocationModel.FloatCoords) < 0.5
                          || DistanceCalculator.DiagonalDistance(topLeft, Item.LocationController.LocationModel.FloatCoords) < Item.UnitView.Height
                    select Item).ToList();
            }
            else
            {
                selected = (from Item in Game.PlayerFaction.FactionModel.Units
                    where topLeft.x <= Item.LocationController.LocationModel.Coords.x + (Item.UnitView.Width / 2)
                          && topLeft.y <= Item.LocationController.LocationModel.Coords.y + (Item.UnitView.Height / 2)
                          && bottomRight.x >= Item.LocationController.LocationModel.Coords.x + (Item.UnitView.Width / 2)
                          && bottomRight.y >= Item.LocationController.LocationModel.Coords.y + (Item.UnitView.Height / 2)
                    select Item).ToList();
            }

            return selected.Cast<IGameActionHolder>().ToList();
        }

        // give movecommand to all units in selection
        public void move(bool isQueueKeyPressed)
        {
            MouseState temp = Mouse.GetState();
            Coords tempcoords = new Coords {x = temp.X, y = temp.Y};
            FloatCoords target = WorldPositionCalculator.DrawCoordsToCellFloatCoords(WorldPositionCalculator.TransformWindowCoords(tempcoords, Game.Camera.GetViewMatrix()), Game.GameView.TileSize);
            foreach (IGameActionHolder unit in SelectedItems)
            {
                if (unit is IMoveable moveable)
                {
                    moveable.MoveTo(target, isQueueKeyPressed);
                }
            }
        }

        //draws the selectionbox
        public void RegisterDrawBox()
        {
            LeftView.Coords = topLeft;
            LeftView.Height = Math.Abs(bottomRight.y - topLeft.y);

            RightView.Coords = new FloatCoords {x = bottomRight.x - RightView.Width, y = topLeft.y};
            RightView.Height = Math.Abs(bottomRight.y - topLeft.y);

            TopView.Coords = topLeft;
            TopView.Width = Math.Abs(bottomRight.x - topLeft.x);

            BottomView.Coords = new FloatCoords {x = topLeft.x, y = bottomRight.y - BottomView.Height};
            BottomView.Width = Math.Abs(bottomRight.x - topLeft.x);


            Game.GameModel.ItemList.Add(LeftView);
            Game.GameModel.ItemList.Add(RightView);
            Game.GameModel.ItemList.Add(TopView);
            Game.GameModel.ItemList.Add(BottomView);
        }
    }
}