namespace kbs2.GamePackage.Interfaces
{
    public interface IViewText : IViewItem
    {
        string SpriteFont { get; }
        string Text { get; }
    }
}