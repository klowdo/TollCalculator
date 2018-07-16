namespace TollCalculator.Domain
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
    }
}
