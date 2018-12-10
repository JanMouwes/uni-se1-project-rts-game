using System;
using System.Collections.Generic;
using kbs2.Desktop.GamePackage.EventArgs;
using kbs2.Faction.Enums;
using kbs2.GamePackage.DayCycle;

namespace kbs2.Faction.CurrencyMVC
{
    public class Currency_Controller
    {
        public Currency_Model model = new Currency_Model();
        public DayController dayController = new DayController();
        public CurrencyView view;

        private int temp;
        private float reward;

        public Currency_Controller() => view = new CurrencyView(model);

        // Adds amount to currency
        public void AddCurrency(float amount)
        {
            if (amount > 0) model.currency += amount;
        }

        // Removes amount to currency
        public void RemoveCurrency(float amount)
        {
            if (amount > 0) model.currency -= amount;
        }

        // if new day gives reward
        public void DailyReward(object sender, OnTickEventArgs eventArgs)
        {
            if (eventArgs.Day > temp)
            {
                reward = (float)(12 + (temp * 0.33)); //TODO Find a propper way/formula
                AddCurrency(reward);
                temp++;
            }
        }


    }
}
