using System;

namespace TollCalculator.Domain
{
    public class TimeRange
    {
        private TimeSpan _start;
        private TimeSpan _end;

        public TimeRange(Time start, Time end)
        {
            _start = TimeSpan.FromHours(start.Hours)
                .Add(TimeSpan.FromMinutes(start.Minutes));
            _end = TimeSpan.FromHours(end.Hours)
                .Add(TimeSpan.FromMinutes(end.Minutes));
        }
        public TimeRange(Time start, TimeSpan span)
        {
            _start = TimeSpan.FromHours(start.Hours)
                .Add(TimeSpan.FromMinutes(start.Minutes));
            _end = _start.Add(span);
        }
        public Time Start => new Time(_start.Hours, _end.Minutes);
        public Time End => new Time(_end.Hours,_end.Minutes); 

        public bool IsInRange(DateTimeOffset date)
        {
            return date.TimeOfDay >= _start && date.TimeOfDay <= _end; 
        }
    }
}