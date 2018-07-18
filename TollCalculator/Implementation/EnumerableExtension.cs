using System;
using System.Collections;
using System.Collections.Generic;

namespace TollCalculator.Implementation
{
    public static class EnumerableExtension{
        public static IEnumerable<T> ForEachWhile<T>(this IEnumerable<T> enumerable, Func<bool> condition) {
            return new FuncEnumerable<T>(enumerable, condition);
        }
        public class FuncEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> _enumerable;
            private readonly Func<bool> _condition;

            public FuncEnumerable(IEnumerable<T> enumerable, Func<bool> condition) {
                _enumerable = enumerable;
                _condition = condition;
            }
            public IEnumerator<T> GetEnumerator() {
                return new FuncEnumerator<T>(_enumerable.GetEnumerator(), _condition);
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        public class FuncEnumerator<T> : IEnumerator<T>
        {
            private readonly IEnumerator<T> _enumerator;
            private readonly Func<bool> _condition;

            public FuncEnumerator(IEnumerator<T> enumerator, Func<bool> condition) {
                _enumerator = enumerator;
                _condition = condition;
            }
            public bool MoveNext() => _condition() && _enumerator.MoveNext();

            public void Reset() => _enumerator.Reset();

            public T Current => _enumerator.Current;

            object IEnumerator.Current => Current;

            public void Dispose() => _enumerator.Dispose();
        }
    }
}