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

        public delegate void OnSelectionChanged(object sender, EventArgsWithPayload<List<IHasGameActions>> eventArgs);

        public event OnSelectionChanged onSelectionChanged;


        public List<IHasGameActions> SelectedItems { get; set; }
        private EventArgsWithPayload<List<IHasGameActions>> args { get; set; }

        private FloatCoords FirstPoint;
        private FloatCoords TopLeft;
        private FloatCoords BottomRight;
        private bool active;

        // constructor
        public Selection_Controller(GameController game, string lineTexture)
        {
            Model = new Selection_Model();
            SelectedItems = new List<IHasGameActions>();
            gameController = game;

            args = new EventArgsWithPayload<List<IHasGameActions>>(SelectedItems);

            LeftView = new Selection_View();
            // width of the selection border
            LeftView.Width = 4f / gameController.GameView.TileSize;

            RightView = new Selection_View();
            // width of the selection border
            RightView.Width = 4f / gameController.GameView.TileSize;

            TopView = new Selection_View();
            // width of the selection border
            TopView.Height = 4f / gameController.GameView.TileSize;

            BottomView = new Selection_View();
            // width of the selection border
            BottomView.Height = 4f / gameController.GameView.TileSize;
        }

        // name explains this I guess
        public void ButtonPressed(FloatCoords mouseCoords)
        {
            //Save current mouse location/coords
            FirstPoint = WorldPositionCalculator.DrawCoordsToCellFloatCoords((FloatCoords) WorldPositionCalculator.TransformWindowCoords((Coords) mouseCoords, gameController.Camera.GetViewMatrix()), gameController.GameView.TileSize);
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
                FloatCoords SecondPoint = WorldPositionCalculator.DrawCoordsToCellFloatCoords((FloatCoords) WorldPositionCalculator.TransformWindowCoords(tempcoords, gameController.Camera.GetViewMatrix()), gameController.GameView.TileSize);
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
        public void UpdateSelection(bool isQueueButtonPressed)
        {
            if (isQueueButtonPressed && SelectedItems.Where((actions => actions != null)).Any())
            {
                SelectedItems.AddRange(SelectBuildings());
            }
            else
            {
                List<IHasGameActions> selection = SelectUnits();
                SelectedItems = (selection.Count > 0) ? isQueueButtonPressed ? SelectedItems.Union(selection).ToList() : selection : SelectBuildings();
            }

            args = new EventArgsWithPayload<List<IHasGameActions>>(SelectedItems);
            onSelectionChanged?.Invoke(this, args);
        }

        // get all buildings from selectionbox
        public List<IHasGameActions> SelectBuildings()
        {
            List<IHasGameActions> selected;
            if (DistanceCalculator.DiagonalDistance(TopLeft, BottomRight) < 0.5)
            {
                selected = new List<IHasGameActions>();
                WorldCellModel cell = gameController.GameModel.World.GetCellFromCoords((Coords)TopLeft).worldCellModel;
                if (cell.BuildingOnTop != null)
                {
                    selected.Add((IHasGameActions)cell.BuildingOnTop);
                }
            }
            else
            {
                selected = (from Item in gameController.PlayerFaction.FactionModel.Buildings
                            where TopLeft.x <= Item.StartCoords.x + (Item.Width / 2)
                            && TopLeft.y <= Item.StartCoords.y + (Item.Height / 2)
                            && BottomRight.x >= Item.StartCoords.x + (Item.Width / 2)
                            && BottomRight.y >= Item.StartCoords.y + (Item.Height / 2)
                            select Item).Cast<IHasGameActions>().ToList();
            }

            return selected;
        }

        // selects units in selectionbox
        public List<IHasGameActions> SelectUnits()
        {
            List<UnitController> selected;
            if (DistanceCalculator.DiagonalDistance(TopLeft, BottomRight) < 0.5)
            {
                selected = (from Item in gameController.PlayerFaction.FactionModel.Units
                            where DistanceCalculator.DiagonalDistance(TopLeft, Item.LocationController.LocationModel.FloatCoords) < 0.5
                            || DistanceCalculator.DiagonalDistance(TopLeft, Item.LocationController.LocationModel.FloatCoords) < Item.UnitView.Height
                            select Item).ToList();
            }
            else
            {
                selected = (from Item in gameController.PlayerFaction.FactionModel.Units
                            where TopLeft.x <= Item.LocationController.LocationModel.Coords.x + (Item.UnitView.Width / 2)
                            && TopLeft.y <= Item.LocationController.LocationModel.Coords.y + (Item.UnitView.Height / 2)
                            && BottomRight.x >= Item.LocationController.LocationModel.Coords.x + (Item.UnitView.Width / 2)
                            && BottomRight.y >= Item.LocationController.LocationModel.Coords.y + (Item.UnitView.Height / 2)
                            select Item).ToList();
            }
            return selected.Cast<IHasGameActions>().ToList();
            
        }

        // give movecommand to all units in selection
        public void move(bool CTRL)
        {
            MouseState temp = Mouse.GetState();
            Coords tempcoords = new Coords { x = temp.X, y = temp.Y };
            FloatCoords target = WorldPositionCalculator.DrawCoordsToCellFloatCoords((FloatCoords)WorldPositionCalculator.TransformWindowCoords(tempcoords, gameController.Camera.GetViewMatrix()), gameController.GameView.TileSize);
            foreach (IHasGameActions unit in SelectedItems)
            {
                if(unit.GetType() == typeof(UnitController))
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


            gameController.GameModel.ItemList.Add(LeftView);
            gameController.GameModel.ItemList.Add(RightView);
            gameController.GameModel.ItemList.Add(TopView);
            gameController.GameModel.ItemList.Add(BottomView);
        }


        
    }
}