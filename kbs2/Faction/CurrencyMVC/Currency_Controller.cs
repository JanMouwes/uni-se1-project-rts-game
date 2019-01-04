using System;
using System.Collections.Generic;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Faction.Enums;
using kbs2.GamePackage.EventArgs;

namespace kbs2.Faction.CurrencyMVC
{
    public class Currency_Controller
    {
        public readonly Currency_Model Model;
        public readonly CurrencyView View;

        public Currency_Controller(float startingBalance)
        {
            Model = new Currency_Model(startingBalance);
            View = new CurrencyView(Model);
        }

        // Alter currency
        public void AlterCurrency(float amount) => Model.Currency += amount;
    }
}