using System.Collections.Generic;
using System.Linq;
using kbs2.Desktop.View.Camera;
using kbs2.GamePackage.Interfaces;
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
        public List<IViewable> DrawList = new List<IViewable>();
        // List for drawing items without offset
        public List<IViewable> DrawGuiList = new List<IViewable>();

        // Calculate the size (Width) of a tile
        public int TileSize => (int)(graphicsDevice.Viewport.Width / camera.CameraModel.TileCount);

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
            
            // Temp Code ==========================
            spriteBatch.Begin();

            // Is niets niet kijken
            DrawHorizontalLine((int) gameModel.Selection.View.Coords.y);
            DrawHorizontalLine((int) (gameModel.Selection.View.Coords.y + gameModel.Selection.View.Height));
            DrawVerticalLine((int)gameModel.Selection.View.Coords.x);
            DrawVerticalLine((int) (gameModel.Selection.View.Coords.x + gameModel.Selection.View.Width));

            spriteBatch.End();

            // End temp code ==========================================
            
            DrawGui();
        }

        // Draws every item in the DrawList with camera offset
        private void DrawNonGui()
        {
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

            foreach (IViewable DrawItem in DrawList)
            {
                if (DrawItem == null) continue;
                Texture2D texture = content.Load<Texture2D>(DrawItem.Texture);
                spriteBatch.Draw(texture, new Rectangle((int)(DrawItem.Coords.x * TileSize), (int)(DrawItem.Coords.y * TileSize), (int)(DrawItem.Width * TileSize), (int)(DrawItem.Height * TileSize)), DrawItem.Color);
            }

            spriteBatch.End();
        }

        // Draws every item in the DrawList without offset
        private void DrawGui()
        {
            spriteBatch.Begin();

            foreach (IViewable DrawItem in DrawGuiList)
            {
                Texture2D texture = content.Load<Texture2D>(DrawItem.Texture);
                spriteBatch.Draw(texture, new Rectangle((int)DrawItem.Coords.x, (int)DrawItem.Coords.y, (int)(DrawItem.Width * TileSize), (int)(DrawItem.Height * TileSize)), DrawItem.Color);
            }

            spriteBatch.End();
        }

        // Returns everything that is in the view
        public void UpdateOnScreen()
        {
            DrawList.Clear();
            DrawGuiList.Clear();

            Vector2 TopLeft = Vector2.Transform(new Vector2(graphicsDevice.Viewport.X, graphicsDevice.Viewport.Y), camera.GetInverseViewMatrix());

            Vector2 BottomRight = Vector2.Transform(new Vector2(graphicsDevice.Viewport.X + graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Y + graphicsDevice.Viewport.Height), camera.GetInverseViewMatrix());

            foreach (var item in gameModel.ItemList)
            {
                if (item.Coords.x < (TopLeft.X / TileSize) - item.Width || item.Coords.y < (TopLeft.Y / TileSize) - item.Height || item.Coords.x > BottomRight.X / TileSize || item.Coords.y > BottomRight.Y / TileSize) continue;
                DrawList.Add(item);
            }

            DrawList = (from Item in DrawList
                        orderby Item.ZIndex ascending
                        select Item).ToList();

            foreach (var item in gameModel.GuiItemList)
            {
                if (item.Coords.x < (TopLeft.X / TileSize) - item.Width || item.Coords.y < (TopLeft.Y / TileSize) - item.Height || item.Coords.x > BottomRight.X / TileSize || item.Coords.y > BottomRight.Y / TileSize) continue;
                DrawGuiList.Add(item);
            }

            DrawGuiList = (from Item in DrawGuiList
                              orderby Item.ZIndex ascending
                        select Item).ToList();

            gameModel.ItemList.Clear();
            gameModel.GuiItemList.Clear();
        }


        // ====================================================================================================== V
        public void DrawHorizontalLine(int PositionY)
        {
            Texture2D texture = Content.Load<Texture2D>(gameModel.Selection.View.Texture);
            if (gameModel.Selection.View.Width > 0)
            {
                for (int i = 0; i <= gameModel.Selection.View.Width - 10; i += 10)
                {
                    if (gameModel.Selection.View.Width - i >= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle((int)(gameModel.Selection.View.Coords.x + i), PositionY, 10, 5),
                            Color.White);
                    }
                }
            }
            else if (gameModel.Selection.View.Width < 0)
            {
                for (int i = -10; i >= gameModel.Selection.View.Width; i -= 10)
                {
                    if (gameModel.Selection.View.Width - i <= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle((int)(gameModel.Selection.View.Coords.x + i), PositionY, 10, 5),
                            Color.White);
                    }
                }
            }
        }

        public void DrawVerticalLine(int PositionX)
        {
            Texture2D texture = Content.Load<Texture2D>(gameModel.Selection.View.Texture);
            if (gameModel.Selection.View.Height > 0)
            {
                for (int i = -2; i <= gameModel.Selection.View.Height; i += 10)
                {
                    if (gameModel.Selection.View.Height - i >= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle(PositionX, (int)(gameModel.Selection.View.Coords.y + i), 10, 5),
                            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, MathHelper.ToRadians(90),
                            new Vector2(0, 0), SpriteEffects.None, 0);
                    }
                }
            }

            else if (gameModel.Selection.View.Height < 0)
            {
                for (int i = 0; i >= gameModel.Selection.View.Height; i -= 10)
                {
                    if (gameModel.Selection.View.Height - i <= 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle(PositionX - 10, (int)(gameModel.Selection.View.Coords.y + i), 10, 5),
                            Color.White);
                    }
                }
            }
        }
    }
}
