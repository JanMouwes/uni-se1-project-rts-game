using System;
using System.Collections.Generic;
using kbs2.Faction.Enums;

namespace kbs2.Faction.CurrencyMVC
{
    public class Currency_Model
    {
        //default 500 
        public float currency;
        public float UpkeepCost { get; set; }

        public Currency_Model(float startingCurrency)
        {
            currency = startingCurrency;
        }
    }
}
