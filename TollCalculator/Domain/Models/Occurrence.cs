using System;

namespace TollCalculator.Domain.Models
{
    public class Occurrence:IComparable<Occurrence>
    {
        public Occurrence(DateTimeOffset passDate)
        {
            Date = passDate;
        }
        public static Occurrence Parse(string input) => new Occurrence(DateTimeOffset.Parse(input));
        public DateTimeOffset Date { get; }

        public int CompareTo(Occurrence other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Date.CompareTo(other.Date);
        }

        public static int Compare(Occurrence occurence1, Occurrence occurence2) => occurence1.CompareTo(occurence2);
    }
}