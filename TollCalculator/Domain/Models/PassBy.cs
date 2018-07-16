using System;

namespace TollCalculator.Domain
{
    public class PassBy:IComparable<PassBy>
    {
        public PassBy(DateTimeOffset passDate)
        {
            Date = passDate;
        }
        public static PassBy Parse(string input) => new PassBy(DateTimeOffset.Parse(input));
        public DateTimeOffset Date { get; }

        public int CompareTo(PassBy other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Date.CompareTo(other.Date);
        }

        public static int Compare(PassBy passBy1, PassBy passBy2) => passBy1.CompareTo(passBy2);
    }
}