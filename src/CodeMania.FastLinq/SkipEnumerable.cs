using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static class SkipEnumerable<T>
    {
        internal static SkipEnumerable<T, TEnumerator> From<TEnumerator>(TEnumerator enumerator, int count)
            where TEnumerator : IEnumerator<T>
        {
            return new SkipEnumerable<T, TEnumerator>(enumerator, count);
        }
    }

    public struct SkipEnumerable<T, TEnumerator> : IEnumerable<T>
        where TEnumerator : IEnumerator<T>
    {
        private TEnumerator enumerator;
        private readonly int count;

        internal SkipEnumerable(TEnumerator enumerator, int count)
        {
            this.enumerator = enumerator;
            this.count = count;
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            private SkipEnumerable<T, TEnumerator> enumerable;
            private int counter;

            internal Enumerator(in SkipEnumerable<T, TEnumerator> enumerable)
            {
                this.enumerable = enumerable;
                counter = enumerable.count;
            }

            public bool MoveNext()
            {
                ref TEnumerator enumerator = ref enumerable.enumerator;

                while (counter-- > 0 && enumerator.MoveNext()) { }

                return counter <= 0 && enumerator.MoveNext();
            }

            public T Current
            {
                get
                {
                    ref TEnumerator enumerator = ref enumerable.enumerator;
                    return enumerator.Current;
                }
            }

            object IEnumerator.Current => Current;

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public void Dispose()
            {
                ref TEnumerator enumerator = ref enumerable.enumerator;
                enumerator.Dispose();
            }
        }
    }
}