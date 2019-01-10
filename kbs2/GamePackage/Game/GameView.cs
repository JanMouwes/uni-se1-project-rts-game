using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.Actions;
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

        private Dictionary<string, Texture2D> cachedTextures = new Dictionary<string, Texture2D>();
        private Dictionary<string, SpriteFont> cachedSpritefonts = new Dictionary<string, SpriteFont>();

        // List for drawing items with the camera offset
        public List<Unit_Controller> DrawList => SortByZIndex(gameModel.ItemList);

        // List for drawing items without offset
        public List<Unit_Controller> DrawGuiList => SortByZIndex(gameModel.GuiItemList.Select(item => (Unit_Controller) item));

        // List for drawing text with the camera offset
        public List<IViewText> DrawText
        {
            get
            {
                Vector2 topLeft = Vector2.Transform(new Vector2(graphicsDevice.Viewport.X, graphicsDevice.Viewport.Y), camera.GetInverseViewMatrix()) / 20;

                Vector2 bottomRight = Vector2.Transform(new Vector2(graphicsDevice.Viewport.X + graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Y + graphicsDevice.Viewport.Height), camera.GetInverseViewMatrix()) / 20;

                return (from viewText in SortByZIndex(gameModel.TextList)
                    where !(viewText.Coords.x < topLeft.X
                            || viewText.Coords.y < topLeft.Y
                            || viewText.Coords.x > bottomRight.X
                            || viewText.Coords.y > bottomRight.Y)
                    select viewText).ToList();
            }
        }

        // List for drawing text without offset
        public List<IViewText> DrawGuiText => SortByZIndex(gameModel.GuiTextList);

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

            DrawNonGui();

            DrawGui();
        }

        private Texture2D ProvideTexture(string texture)
        {
            if (!cachedTextures.ContainsKey(texture)) cachedTextures[texture] = content.Load<Texture2D>(texture);

            return cachedTextures[texture];
        }

        private SpriteFont ProvideSpritefont(string spritefont)
        {
            if (!cachedSpritefonts.ContainsKey(spritefont)) cachedSpritefonts[spritefont] = content.Load<SpriteFont>(spritefont);

            return cachedSpritefonts[spritefont];
        }

        private double CalculateRotation(double rotation) => (rotation > 0 || rotation < 0) ? (float) (Math.PI / (180 / rotation)) : 0;

        // Draws every item in the DrawList with camera offset
        private void DrawNonGui()
        {
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
            foreach (Unit_Controller drawItem in DrawList)
            {
                Texture2D texture = ProvideTexture(drawItem.Texture);

                Color colour = drawItem.ViewMode == ViewMode.Fog ? Color.DarkGray : drawItem.Colour;
                spriteBatch.Draw(
                    texture: texture,
                    destinationRectangle: new Rectangle((int) (drawItem.Coords.x * TileSize), (int) (drawItem.Coords.y * TileSize), (int) (drawItem.Width * TileSize), (int) (drawItem.Height * TileSize)),
                    sourceRectangle: null,
                    color: colour,
                    rotation: (float) CalculateRotation(drawItem.Rotation),
                    origin: Vector2.Zero,
                    effects: SpriteEffects.None,
                    layerDepth: 1
                );
            }

            foreach (IViewText drawItem in DrawText)
            {
                SpriteFont font = ProvideSpritefont(drawItem.SpriteFont);
                Color color = drawItem.ViewMode == ViewMode.Fog ? Color.DarkGray : drawItem.Colour;
                spriteBatch.DrawString(font, drawItem.Text, new Vector2(drawItem.Coords.x * TileSize, drawItem.Coords.y * TileSize), color);
            }

            spriteBatch.End();
        }

        // Draws every item in the DrawList without offset
        private void DrawGui()
        {
            spriteBatch.Begin();

            foreach (Unit_Controller drawItem in DrawGuiList)
            {
                Texture2D texture = ProvideTexture(drawItem.Texture);
                spriteBatch.Draw(texture, new Rectangle((int) drawItem.Coords.x, (int) drawItem.Coords.y, (int) drawItem.Width, (int) drawItem.Height), drawItem.Colour);
            }

            foreach (IViewText drawItem in DrawGuiText)
            {
                SpriteFont font = ProvideSpritefont(drawItem.SpriteFont);
                spriteBatch.DrawString(font, drawItem.Text, new Vector2(drawItem.Coords.x, drawItem.Coords.y), drawItem.Colour);
            }

            spriteBatch.End();
        }

        private List<Unit_Controller> GetCellsOnScreen(IEnumerable<Unit_Controller> drawList, Vector2 topLeft, Vector2 bottomRight)
        {
            List<Unit_Controller> returnList = new List<Unit_Controller>();

            returnList = (from item in drawList
                where (item.Coords.x > (topLeft.X / TileSize) - item.Width &&
                       item.Coords.y > (topLeft.Y / TileSize) - item.Height &&
                       item.Coords.x < bottomRight.X / TileSize && item.Coords.y < bottomRight.Y / TileSize)
                orderby item.ZIndex ascending
                select item).ToList();

            return returnList;
        }

        private List<TItem> SortByZIndex<TItem>(IEnumerable<TItem> listToSort) where TItem : IViewItem => (from item in listToSort where item != null orderby item.ZIndex ascending select item).ToList();

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