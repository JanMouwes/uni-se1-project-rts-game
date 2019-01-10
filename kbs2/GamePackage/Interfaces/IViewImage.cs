namespace kbs2.GamePackage.Interfaces
{
    public interface Unit_Controller : IViewItem
    {
        float Width { get; }
        float Height { get; }
        string Texture { get; }
    }
}