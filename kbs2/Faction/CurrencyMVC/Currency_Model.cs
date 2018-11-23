using System;
using System.Collections.Generic;
using kbs2.Faction.Enums;

namespace kbs2.Faction.CurrencyMVC
{
    public class Currency_Model
    {
        public Currency_Model()
        {
            foreach(ResourceType resource in Enum.GetValues(typeof(ResourceType ))){
                resources.Add(resource, 0);
            }
        }

        public float currency; 
        public Dictionary<ResourceType, float> resources;
    }
}
