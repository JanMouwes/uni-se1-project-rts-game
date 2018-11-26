using System;
using System.Collections.Generic;
using kbs2.Faction.CurrencyMVC;
using kbs2.Factory.Enums;

namespace kbs2.Factory.ResourceMVC
{
    public class ResourceController
    {
        public ResourceModel model = new ResourceModel();
        public Currency_Controller currencyController = new Currency_Controller();

        public void AddResource(float amount, ResourceType resource)
        {
            if (amount > 0)
            {
                model.resources[resource] += amount;
            }
        }

        public void ResourcesToCurrency()
        {

            float[] resourceTemp = new float[model.resources.Count];
            int counter = 0;

            foreach (KeyValuePair<ResourceType, float> x in model.resources)
            {

                resourceTemp[counter] = x.Value;
                counter++;
            }
            Array.Sort(resourceTemp);

            float lastvalue = 0;

            for (int i = 0; i < model.resources.Count; i++)
            {
                currencyController.AddCurrency(resourceTemp[i] - lastvalue * (float)((model.resources.Count - 1 - i) * 0.5));
                lastvalue = resourceTemp[i];
            }
        }

    }
}
