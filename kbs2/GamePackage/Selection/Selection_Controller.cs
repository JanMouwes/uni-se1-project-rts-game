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


        public Selection_Controller(GameController game, string lineTexture)
        {
            Model = new Selection_Model();
			SelectedItems = new List<IHasActions>();
            gameController = game;

            

            LeftView = new Selection_View();
            LeftView.Width = 4f/gameController.gameView.TileSize;

            RightView = new Selection_View();
            RightView.Width = 4f / gameController.gameView.TileSize;

            TopView = new Selection_View();
            TopView.Height = 4f / gameController.gameView.TileSize;

            BottomView = new Selection_View();
            BottomView.Height = 4f / gameController.gameView.TileSize;
        }


        public void ButtonPressed(FloatCoords mouseCoords)
        {
            FirstPoint = WorldPositionCalculator.DrawCoordsToCellFloatCoords(WorldPositionCalculator.TransformWindowCoords((Coords)mouseCoords, gameController.camera.GetViewMatrix()), gameController.gameView.TileSize);
            active = true;
        }

        public void ButtonRelease(bool CTRL)
        {
            UpdateSelection(CTRL);
            active = false;
        }

        public void Update(object sender, OnTickEventArgs eventArgs)
        {
            if (active)
            {
                MouseState temp = Mouse.GetState();
                Coords tempcoords = new Coords { x = temp.X, y = temp.Y };
                FloatCoords SecondPoint = WorldPositionCalculator.DrawCoordsToCellFloatCoords(WorldPositionCalculator.TransformWindowCoords(tempcoords, gameController.camera.GetViewMatrix()), gameController.gameView.TileSize);
                SetCoords(FirstPoint, SecondPoint);
                DrawBox();
            }
        }

        public void SetCoords(FloatCoords firstCoords, FloatCoords secondCoords)
        {
            TopLeft.x = firstCoords.x < secondCoords.x ? firstCoords.x : secondCoords.x;
            TopLeft.y = firstCoords.y < secondCoords.y ? firstCoords.y : secondCoords.y;

            BottomRight.x = firstCoords.x > secondCoords.x ? firstCoords.x : secondCoords.x;
            BottomRight.y = firstCoords.y > secondCoords.y ? firstCoords.y : secondCoords.y;
        }

        public void UpdateSelection(bool CTRL)
        {
            if (CTRL && SelectedItems.OfType<IHasActions>().Any())
            {
                SelectedItems.AddRange( SelectBuildings(CTRL));
                args = new EventArgsWithPayload<List<IHasActions>>(SelectedItems);
                onSelectionChanged?.Invoke(this, args);
            }
            else
            {
                
                List<IHasActions>selection = SelectUnits(CTRL);
                if (selection.Count > 0)
                {
                    SelectedItems = CTRL ? SelectedItems.Union(selection).ToList() : selection;
                    args = new EventArgsWithPayload<List<IHasActions>>(SelectedItems);
                    onSelectionChanged?.Invoke(this, args);
                }
                else
                {
                    SelectedItems = SelectBuildings(CTRL);
                    args = new EventArgsWithPayload<List<IHasActions>>(SelectedItems);
                    onSelectionChanged?.Invoke(this, args);
                }
            }
        }

        public List<IHasActions> SelectBuildings(bool CTRL)
        {
            List<IHasActions> Selected;
            if (DistanceCalculator.getDistance2d(TopLeft, BottomRight) < 0.5)
            {
                Selected = new List<IHasActions>();
                WorldCellModel cell = gameController.gameModel.World.GetCellFromCoords((Coords)TopLeft).worldCellModel;
                if (cell.BuildingOnTop != null)
                {
                    Selected.Add((IHasActions)cell.BuildingOnTop);
                }
            }
            else
            {
                Selected = (from Item in gameController.PlayerFaction.FactionModel.Buildings
                            where TopLeft.x <= Item.Model.TopLeft.x + (Item.View.Width / 2)
                            && TopLeft.y <= Item.Model.TopLeft.y + (Item.View.Height / 2)
                            && BottomRight.x >= Item.Model.TopLeft.x + (Item.View.Width / 2)
                            && BottomRight.y >= Item.Model.TopLeft.y + (Item.View.Height / 2)
                            select Item).Cast<IHasActions>().ToList();
            }
            return Selected;
        }

        public List<IHasActions> SelectUnits(bool CTRL)
        {
            List<Unit_Controller> Selected;
            if (DistanceCalculator.getDistance2d(TopLeft, BottomRight) < 0.5)
            {
                Selected = (from Item in gameController.PlayerFaction.FactionModel.Units
                            where DistanceCalculator.getDistance2d(TopLeft, Item.LocationController.LocationModel.floatCoords) < 0.5
                            || DistanceCalculator.getDistance2d(TopLeft, Item.LocationController.LocationModel.floatCoords) < Item.UnitView.Height
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

        public void move(bool CTRL)
        {
            MouseState temp = Mouse.GetState();
            Coords tempcoords = new Coords { x = temp.X, y = temp.Y };
            FloatCoords target = WorldPositionCalculator.DrawCoordsToCellFloatCoords(WorldPositionCalculator.TransformWindowCoords(tempcoords, gameController.camera.GetViewMatrix()), gameController.gameView.TileSize);
            foreach (IHasActions unit in SelectedItems)
            {
                if(unit.GetType() == typeof(Unit_Controller))
                {
                    ((IMoveable)unit).MoveTo(target, CTRL);
                }
            }
        }

        public void DrawBox()
        {
            LeftView.Coords = TopLeft;
            LeftView.Height = Math.Abs(BottomRight.y - TopLeft.y);

            RightView.Coords = new FloatCoords {x = BottomRight.x - RightView.Width, y = TopLeft.y };
            RightView.Height = Math.Abs(BottomRight.y - TopLeft.y);

            TopView.Coords = TopLeft;
            TopView.Width = Math.Abs(BottomRight.x - TopLeft.x);

            BottomView.Coords = new FloatCoords { x = TopLeft.x, y = BottomRight.y - BottomView.Height };
            BottomView.Width = Math.Abs(BottomRight.x - TopLeft.x);


            gameController.gameModel.ItemList.Add(LeftView);
            gameController.gameModel.ItemList.Add(RightView);
            gameController.gameModel.ItemList.Add(TopView);
            gameController.gameModel.ItemList.Add(BottomView);
        }


        
    }
}