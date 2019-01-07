using System;
using kbs2.GamePackage.EventArgs;

namespace kbs2.GamePackage.DayCycle
{
    public class TimeController
    {
        /// <summary>
        /// Time of game-day in game-hours
        /// </summary>
        public const int INGAME_DAY_LENGTH_HOURS = 3;

        /// <summary>
        /// Time of game-day in game-minutes 
        /// </summary>
        public const int INGAME_DAY_LENGTH_MINUTES = INGAME_DAY_LENGTH_HOURS * INGAME_HOUR_LENGTH_MINUTES;

        /// <summary>
        /// Time of game-hour in game-minutes
        /// </summary>
        public const int INGAME_HOUR_LENGTH_MINUTES = 3;

        /// <summary>
        /// Time of game-minute in real seconds
        /// </summary>
        public const int INGAME_MINUTE_LENGTH = 1;

        /// <summary>
        /// Current game-time
        /// </summary>
        private IngameTime currentTime;

        /// <summary>
        /// Total real seconds past
        /// </summary>
        private int secondsPast;

        public delegate void TimePassedHandler(object sender, EventArgsWithPayload<IngameTime> eventArgs);

        public event TimePassedHandler MinutePassed;
        public event TimePassedHandler HourPassed;
        public event TimePassedHandler DayPassed;


        /// <summary>
        /// Updates the game-time
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="eventArgs">Event-args</param>
        public void UpdateTime(object sender, OnTickEventArgs eventArgs)
        {
            if (Math.Floor(eventArgs.GameTime.TotalGameTime.TotalSeconds) < secondsPast + INGAME_MINUTE_LENGTH) return;

            secondsPast = (int) eventArgs.GameTime.TotalGameTime.TotalSeconds;

            TickMinute();
            
            Console.WriteLine(currentTime);
        }

        /// <summary>
        /// Pass minute
        /// </summary>
        private void TickMinute()
        {
            currentTime.CurrentMinute++;

            MinutePassed?.Invoke(this, new EventArgsWithPayload<IngameTime>(currentTime));

            if (currentTime.CurrentMinute < INGAME_HOUR_LENGTH_MINUTES) return;

            currentTime.CurrentMinute = 0;

            TickHour();
        }

        /// <summary>
        /// Pass hour
        /// </summary>
        private void TickHour()
        {
            currentTime.CurrentHour++;

            HourPassed?.Invoke(this, new EventArgsWithPayload<IngameTime>(currentTime));

            if (currentTime.CurrentHour < INGAME_DAY_LENGTH_HOURS) return;

            currentTime.CurrentHour = 0;

            TickDay();
        }

        /// <summary>
        /// Pass day
        /// </summary>
        private void TickDay()
        {
            currentTime.CurrentDay++;

            DayPassed?.Invoke(this, new EventArgsWithPayload<IngameTime>(currentTime));
        }
    }
}