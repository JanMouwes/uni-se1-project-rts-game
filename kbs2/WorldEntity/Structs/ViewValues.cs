using kbs2.GamePackage.Interfaces;

namespace kbs2.WorldEntity.Structs
{
    public struct ViewValues
    {
        public string Image { get; }
        public float Width { get; }
        public float Height { get; }

        public ViewValues(string image, float width, float height)
        {
            Image = image;
            Width = width;
            Height = height;
        }
    }
}