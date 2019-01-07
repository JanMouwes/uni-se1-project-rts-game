namespace kbs2.UserInterface.MiniMap
{
    public class MiniMapModel
    {
        public CameraController MiniMap { get; set; }
        public GameController GameController { get; set; }

        public MiniMapModel(CameraController camera, GameController gameController)
        {
            MiniMap = camera;
            GameController = gameController;
        }

        private void DrawNonGui()
        {
            GameController.spriteBatch.Begin(transformMatrix: MiniMap.GetViewMatrix());

            foreach (IViewImage DrawItem in GameController.gameView.DrawList)
            {
                int TileSize = GameController.gameView.TileSize;

                if (DrawItem == null) continue;
                Texture2D texture = GameController.Content.Load<Texture2D>(DrawItem.Texture);
                GameController.spriteBatch.Draw(texture,
                    new Rectangle((int)(DrawItem.Coords.x * TileSize), (int)(DrawItem.Coords.y * TileSize),
                        (int)(DrawItem.Width * TileSize), (int)(DrawItem.Height * TileSize)), DrawItem.Colour);
            }

            GameController.spriteBatch.End();
        }
    }
}
