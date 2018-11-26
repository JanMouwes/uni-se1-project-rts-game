using System;
using System.Collections.Generic;
using kbs2.Factory.Enums;

namespace kbs2.Factory.ResourceMVC
{
    public class ResourceModel
    {
        public Dictionary<ResourceType, float> resources;

        public ResourceModel()
        {
            resources = new Dictionary<ResourceType, float>();

            foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
            {
                resources.Add(resource, 0);
            }
        }
    }
}
