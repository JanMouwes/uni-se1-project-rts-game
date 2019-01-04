using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.WorldEntity.Structures.ResourceFactory
{
    public class ResourceFactoryView : IViewImage
    {
        private readonly ResourceFactoryModel model;

        public FloatCoords Coords => (FloatCoords) model.TopLeft;
        
        public float Width => model.Def.ViewValues.Width;
        public float Height => model.Def.ViewValues.Height;
        public string Texture => model.Def.ViewValues.Image;
        
        public ViewMode ViewMode => model.ViewMode;
        
        public int ZIndex => 2;
        public Color Colour => Color.White;

        public ResourceFactoryView(ResourceFactoryModel model)
        {
            this.model = model;
        }
    }
}