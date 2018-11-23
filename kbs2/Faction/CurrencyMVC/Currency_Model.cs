using System;
using System.Collections.Generic;
using kbs2.Faction.Enums;

namespace kbs2.Faction.CurrencyMVC
{
    public class Currency_Model
    {

        public Dictionary<ResourceType, float> resources;
        public Currency_Model()
        {
            resources = new Dictionary<ResourceType, float>();

            foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType ))){
                resources.Add(resource, 0);
            }
        }

        public float currency = 500; 
    }
}
