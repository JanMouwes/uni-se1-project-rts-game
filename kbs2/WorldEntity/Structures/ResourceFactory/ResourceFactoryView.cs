using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.WorldEntity.Structures.ResourceFactory
{
    public class ResourceFactoryView : Unit_Controller
    {
        private readonly ResourceFactoryModel model;

        public double Rotation { get; }
        public FloatCoords Coords => (FloatCoords) model.TopLeft;
        
        public float Width => model.Def.ViewValues.Width;
        public float Height => model.Def.ViewValues.Height;
        public string Texture => model.Def.ViewValues.Image;
        
        public ViewMode ViewMode => model.ViewMode;
        
        public int ZIndex => 2;
        public Color Colour => Color.White;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Data-model</param>
        public ResourceFactoryView(ResourceFactoryModel model)
        {
            this.model = model;
        }
    }
}