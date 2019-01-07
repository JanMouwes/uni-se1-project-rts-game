using kbs2.Resources.Enums;

namespace kbs2.WorldEntity.Structures.Defs
{
    public class ResourceFactoryDef : BuildingDef
    {
        /// <summary>
        /// Building's resource-type
        /// </summary>
        public ResourceType ResourceType { get; set; }
    }
}