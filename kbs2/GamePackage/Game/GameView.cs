using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kbs2.Desktop.View.Camera;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace kbs2.GamePackage
{
    public class GameView
    {
        private GameModel gameModel;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private CameraController camera;
        private GraphicsDevice graphicsDevice;
        private ContentManager content;

        // List for drawing items with the camera offset
        public List<IViewImage> DrawList = new List<IViewImage>();

        // List for drawing items without offset
        public List<IViewImage> DrawGuiList = new List<IViewImage>();

        // List for drawing text with the camera offset
        public List<IViewText> DrawText = new List<IViewText>();

        // List for drawing text without offset
        public List<IViewText> DrawGuiText = new List<IViewText>();

        // Calculate the size (Width) of a tile
        public int TileSize => (int) (graphicsDevice.Viewport.Width / camera.CameraModel.TileCount);

        // Constructor
        public GameView(GameModel gameModel, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, CameraController camera, GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.gameModel = gameModel;
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            this.camera = camera;
            this.graphicsDevice = graphicsDevice;
            this.content = content;
        }

        public void Draw()
        {
            // Clears the graphicsDevice to make room for the new draw items
            graphicsDevice.Clear(Color.Black);

            // Updates everything on screen
            UpdateOnScreen();

            DrawNonGui();

            DrawGui();
        }

        // Draws every item in the DrawList with camera offset
        private void DrawNonGui()
        {
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

            foreach (IViewImage DrawItem in DrawList)
            {
                Texture2D texture = content.Load<Texture2D>(DrawItem.Texture);
                Color color = DrawItem.ViewMode == ViewMode.Fog ? Color.DarkGray : DrawItem.Colour;
                spriteBatch.Draw(texture, new Rectangle((int)(DrawItem.Coords.x * TileSize), (int)(DrawItem.Coords.y * TileSize), (int)(DrawItem.Width * TileSize), (int)(DrawItem.Height * TileSize)), color);
            }

            foreach (IViewText DrawItem in DrawText)
            {
                SpriteFont font = content.Load<SpriteFont>(DrawItem.SpriteFont);
                Color color = DrawItem.ViewMode == ViewMode.Fog ? Color.DarkGray : DrawItem.Colour;
                spriteBatch.DrawString(font, DrawItem.Text, new Vector2(DrawItem.Coords.x * TileSize, DrawItem.Coords.y * TileSize), color);
            }

            spriteBatch.End();
        }

        // Draws every item in the DrawList without offset
        private void DrawGui()
        {
            spriteBatch.Begin();

            foreach (IViewImage DrawItem in DrawGuiList)
            {
                Texture2D texture = content.Load<Texture2D>(DrawItem.Texture);
                spriteBatch.Draw(texture, new Rectangle((int) DrawItem.Coords.x, (int) DrawItem.Coords.y, (int) DrawItem.Width, (int) DrawItem.Height), DrawItem.Colour);
            }

            foreach (IViewText DrawItem in DrawGuiText)
            {
                SpriteFont font = content.Load<SpriteFont>(DrawItem.SpriteFont);
                spriteBatch.DrawString(font, DrawItem.Text, new Vector2(DrawItem.Coords.x, DrawItem.Coords.y), DrawItem.Colour);
            }

            spriteBatch.End();
        }

        private List<IViewImage> GetCellsOnScreen(IEnumerable<IViewImage> drawList, Vector2 topLeft, Vector2 bottomRight)
        {
            List<IViewImage> returnList = new List<IViewImage>();

            returnList = (from item in drawList
                where (item.Coords.x > (topLeft.X / TileSize) - item.Width &&
                       item.Coords.y > (topLeft.Y / TileSize) - item.Height &&
                       item.Coords.x < bottomRight.X / TileSize && item.Coords.y < bottomRight.Y / TileSize)
                orderby item.ZIndex ascending
                select item).ToList();

            return returnList;
        }

        private async Task UpdateCellsOnScreen(IEnumerable<IViewImage> drawList, Vector2 topLeft, Vector2 bottomRight)
        {
            await Task.Run(() => DrawList = GetCellsOnScreen(drawList, topLeft, bottomRight));
        }

        // Returns everything that is in the view
        public void UpdateOnScreen()
        {            
            // Clears the Draw Lists 
            DrawList.Clear();
            DrawGuiList.Clear();
            DrawText.Clear();
            DrawGuiText.Clear();

            List<TItem> SortByZIndex<TItem>(IEnumerable<TItem> listToSort) where TItem : IViewItem =>
                (from item in listToSort orderby item.ZIndex ascending select item).ToList();

            Vector2 topLeft = Vector2.Transform(new Vector2(graphicsDevice.Viewport.X, graphicsDevice.Viewport.Y), camera.GetInverseViewMatrix());

            Vector2 bottomRight = Vector2.Transform(new Vector2( graphicsDevice.Viewport.X + graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Y + graphicsDevice.Viewport.Height), camera.GetInverseViewMatrix());

            // Gets the cells in the current view and adds them to the Drawlist
            DrawList.AddRange(GetCellsOnScreen(gameModel.ItemList, topLeft, bottomRight));

            // Add all items to the DrawGuiList in the correct Zindex order
            DrawGuiList = SortByZIndex(gameModel.GuiItemList);

            // Add sorted Text to the drawList in the correct z-index order
            foreach (IViewText item in gameModel.TextList)
            {
                if (item.Coords.x < (topLeft.X / TileSize) || item.Coords.y < (topLeft.Y / TileSize) ||
                    item.Coords.x > bottomRight.X / TileSize || item.Coords.y > bottomRight.Y / TileSize) continue;
                DrawText.Add(item);
            }

            DrawText = SortByZIndex(DrawText);

            // Add all Text to the DrawGuiList in the correct Zindex order
            DrawGuiText = SortByZIndex(gameModel.GuiTextList);
        }

        /// <summary>
        /// Draws line. Stolen from <see href="https://gamedev.stackexchange.com/questions/44015/how-can-i-draw-a-simple-2d-line-in-xna-without-using-3d-primitives-and-shders">here</see>.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="texture"></param>
        private void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Texture2D texture)
        {
            Vector2 edge = end - start;
            
            // calculate angle to rotate line
            float angle = (float) Math.Atan2(edge.Y, edge.X);

            sb.Draw(texture,
                new Rectangle( // rectangle defines shape of line and position of start of line
                    (int) start.X,
                    (int) start.Y,
                    (int) edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                angle, //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }
    }
}