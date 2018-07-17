using System;

namespace TollCalculator.Domain.Models
{
    public sealed class Money
    {
        public readonly decimal Value;
        public readonly CurrencyCode CurrencyCode;

        public Money(decimal value, CurrencyCode currencyCode)
        {
            Value = value;
            CurrencyCode = currencyCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is Money otherMoney)
                return Equals(otherMoney);
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value.GetHashCode() * 397) ^ (int) CurrencyCode;
            }
        }

        private bool Equals(Money other)
        {
            return Value == other.Value && CurrencyCode == other.CurrencyCode;
        }

        public static implicit operator decimal(Money money) => money.Value;


        public static Money operator +(Money money1, Money money2)
        {
            if(money1.CurrencyCode != money2.CurrencyCode)
                throw new InvalidOperationException("Can not use addition on diffrent currencies");
            return new Money(money1.Value + money2.Value, money1.CurrencyCode);
        }
        public static Money operator -(Money money1, Money money2)
        {
            if (money1.CurrencyCode != money2.CurrencyCode)
                throw new InvalidOperationException("Can not use subtraction on diffrent currencies");
            return new Money(money1.Value - money2.Value, money1.CurrencyCode);
        }
        public static Money operator *(Money money1, decimal money2)
        {
            return new Money(money1.Value * money2, money1.CurrencyCode);
        }
        public static Money operator *(Money money1, int money2)
        {
            return new Money(money1.Value * money2, money1.CurrencyCode);
        }

        public static Money Create(decimal value, CurrencyCode currencyCode) => new Money(value, currencyCode);
        public static Money CreateSek(decimal value) => new Money(value, CurrencyCode.SEK);
        public static Money Zero(CurrencyCode currencyCode) => new Money(decimal.Zero, currencyCode);
        public override string ToString() => $"{Value} {CurrencyCode}";
    }
}