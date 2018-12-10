using System;
using kbs2.Desktop.GamePackage.EventArgs;
using Microsoft.Xna.Framework;

namespace kbs2.GamePackage.DayCycle
{
    public class DayController
    {

        public DayModel dayModel = new DayModel();
        private int currentMin;
        private int past;

        public int min;
        public int hrs;

        public void UpdateTime(object sender, OnTickEventArgs eventArgs)
        {
            if ((int)eventArgs.GameTime.TotalGameTime.TotalSeconds > past)
            {
                past = (int)eventArgs.GameTime.TotalGameTime.TotalSeconds;
                eventArgs.Day = dayModel.currentDay;
                Console.WriteLine("dag: " + eventArgs.Day);
                MinutePassed();
            }
        }

        private void MinutePassed()
        {
            ++currentMin;
            if (currentMin >= DayModel.day )
            {
                // Next day 
                ++dayModel.currentDay;
                //Console.WriteLine(dayModel.currentDay + "dayss passed");
                currentMin = 0;
            }
            min = currentMin % 6;
            hrs = currentMin / 6;

            Console.WriteLine("Minuut " + hrs.ToString() + "   :   " + min.ToString());
        }




    }
}
