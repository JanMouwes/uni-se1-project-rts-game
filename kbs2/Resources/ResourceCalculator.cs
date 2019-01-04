using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.Resources.Enums;

namespace kbs2.Resources
{
    public class ResourceCalculator : IDisposable
    {
        public Dictionary<ResourceType, float> Resources;

        /// <summary>
        /// <para>Constructor</para>
        /// <para>Populates resources-dictionary with all resources</para>
        /// 
        /// </summary>
        public ResourceCalculator()
        {
            Resources = new Dictionary<ResourceType, float>();

            foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
            {
                Resources.Add(resource, 0);
            }
        }

        public void AddResource(float amount, ResourceType resource)
        {
            if (amount > 0)
            {
                Resources[resource] += amount;
            }
        }

        public float CalculateResourceWorth()
        {
            float[] resourceTemp = (from resource in Resources
                orderby resource.Value ascending
                select resource.Value).ToArray();

            float lastValue = 0;

            float totalValue = 0;

            for (int i = 0; i < Resources.Count; i++)
            {
                totalValue += resourceTemp[i] - lastValue * (float) ((Resources.Count - 1 - i) * 0.5);
                lastValue = resourceTemp[i];
            }

            return totalValue;
        }

        public void Dispose()
        {
            Resources = null;
        }
    }
}