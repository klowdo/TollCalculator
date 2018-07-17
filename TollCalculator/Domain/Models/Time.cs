using System;

namespace TollCalculator.Domain.Models
{
    public class Time
    {
        public Time(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }
        public int Hours { get; }
        public int Minutes { get;  }
        public static Time CreateFrom(DateTimeOffset date) => new Time(date.Hour, date.Minute);
    }
}
