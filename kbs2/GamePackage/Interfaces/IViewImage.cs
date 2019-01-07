namespace kbs2.GamePackage.Interfaces
{
    public interface IViewImage : IViewItem
    {
        float Width { get; }
        float Height { get; }
        string Texture { get; }
    }
}