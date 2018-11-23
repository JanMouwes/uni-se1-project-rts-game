using System;
using System.Collections.Generic;
using kbs2.Faction.Enums;

namespace kbs2.Faction.CurrencyMVC
{
    public class Currency_Controller
    {
        public Currency_Model model = new Currency_Model();

        public void AddCurrency(float amount)
        {
            if(amount>0){
                model.currency += amount;
            }
        }

        public void RemoveCurrency(float amount)
        {
            if (amount > 0)
            {
                model.currency -= amount;
            }
        }

        public void OnTick() //TODO subscribe to onTick event
        {
            //todo this
        }

        public void AddResource(float amount, ResourceType resource)
        {
            if(amount>0){
                model.resources[resource] += amount;
            }
        }

        public void ResourcesToCurrency(){

            float[] resourceTemp = new float[model.resources.Count];
            int counter = 0;

            foreach(KeyValuePair<ResourceType,float> x in model.resources ){

                resourceTemp[counter] = x.Value;
                counter++;
            }
            Array.Sort(resourceTemp);

            float lastvalue = 0;

            for (int i = 0; i < model.resources.Count;i++){
                AddCurrency(resourceTemp[i] - lastvalue * (float)((model.resources.Count -1 - i) * 0.5));
                lastvalue = resourceTemp[i];
            }
        }
    }
}
