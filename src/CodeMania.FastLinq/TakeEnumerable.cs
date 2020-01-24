using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    internal static class TakeEnumerable<T>
    {
        internal static TakeEnumerable<T, TEnumerator> From<TEnumerator>(TEnumerator enumerator, int count)
            where TEnumerator : IEnumerator<T>
        {
            return new TakeEnumerable<T, TEnumerator>(enumerator, count);
        }
    }

    public struct TakeEnumerable<T, TEnumerator> : IEnumerable<T>
        where TEnumerator : IEnumerator<T>
    {
        private TEnumerator enumerator;
        private readonly int count;

        internal TakeEnumerable(TEnumerator enumerator, int count)
        {
            this.enumerator = enumerator;
            this.count = count;
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            private TakeEnumerable<T, TEnumerator> enumerable;
            private int counter;

            internal Enumerator(in TakeEnumerable<T, TEnumerator> enumerable)
            {
                this.enumerable = enumerable;
                counter = 0;
            }

            public bool MoveNext()
            {
                ref TEnumerator enumerator = ref enumerable.enumerator;

                return counter++ < enumerable.count && enumerator.MoveNext();
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