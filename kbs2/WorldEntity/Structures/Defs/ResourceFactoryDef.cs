using System.Collections.Generic;
using kbs2.Resources.Enums;
using kbs2.World;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

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