using System;
using System.Collections.Generic;
using kbs2.Faction.Enums;

namespace kbs2.Faction.CurrencyMVC
{
    public class Currency_Controller
    {
        public Currency_Model model = new Currency_Model();

        // Adds amount to currency
        public void AddCurrency(float amount)
        {
            if(amount>0){
                model.currency += amount;
            }
        }

        // Removes amount to currency
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

    }
}
