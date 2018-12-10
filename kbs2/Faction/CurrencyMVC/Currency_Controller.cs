using System;
using System.Collections.Generic;
using kbs2.Faction.Enums;

namespace kbs2.Faction.CurrencyMVC
{
    public class Currency_Controller
    {
        public Currency_Model model = new Currency_Model();
        public DayController dayController = new DayController();
        int temp;
        float reward;

        // Adds amount to currency
        public void AddCurrency(float amount)
        {
            if (amount > 0)
            {
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

        public void DailyReward(object sender, OnTickEventArgs eventArgs)
        {
            if (eventArgs.Day > temp)
            {
                reward = (float)(12 + (temp * 0.33));
                AddCurrency(reward);
                temp = eventArgs.Day;
                Console.WriteLine(model.currency);
            }
        }


    }
}
