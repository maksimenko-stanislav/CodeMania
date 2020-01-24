using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public struct WhereEnumerable<T, TEnumerator> : IEnumerable<T>
        where TEnumerator : IEnumerator<T>
    {
        private TEnumerator enumerator;
        private readonly Func<T, bool> predicate;

        internal WhereEnumerable(TEnumerator enumerator, Func<T, bool> predicate)
        {
            this.enumerator = enumerator;
            this.predicate = predicate;
        }

        public Enumerator GetEnumerator()
        {
            if (predicate == null)
            {
                throw new InvalidOperationException("Use non-default constructor to create current enumerable type.");
            }

            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            private WhereEnumerable<T, TEnumerator> enumerable;

            private T current;

            public Enumerator(in WhereEnumerable<T, TEnumerator> enumerable)
            {
                this.enumerable = enumerable;
                current = default;
            }

            public bool MoveNext()
            {
                bool hasNext;

                ref TEnumerator enumerator = ref enumerable.enumerator;

                var predicate = enumerable.predicate;

                while ((hasNext = enumerator.MoveNext()) && !predicate(enumerator.Current))
                {
                }

                if (hasNext)
                {
                    current = enumerator.Current;

                    return true;
                }

                return false;
            }

            public void Dispose()
            {
                ref TEnumerator enumerator = ref enumerable.enumerator;
                enumerator.Dispose();
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public T Current => current;

            object IEnumerator.Current => Current;
        }
    }

    public struct WhereEnumerable<T, TArgument, TEnumerator> : IEnumerable<T>
        where TEnumerator : IEnumerator<T>
    {
        private TEnumerator enumerator;
        private readonly TArgument argument;
        private readonly Predicate<T, TArgument> predicate;

        internal WhereEnumerable(TEnumerator enumerator, TArgument argument, Predicate<T, TArgument> predicate)
        {
            this.enumerator = enumerator;
            this.argument = argument;
            this.predicate = predicate;
        }

        public Enumerator GetEnumerator()
        {
            if (predicate == null)
            {
                throw new InvalidOperationException("Use non-default constructor to create current enumerable type.");
            }

            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            private WhereEnumerable<T, TArgument, TEnumerator> enumerable;

            private T current;

            public Enumerator(in WhereEnumerable<T, TArgument, TEnumerator> enumerable)
            {
                this.enumerable = enumerable;
                current = default;
            }

            public bool MoveNext()
            {
                bool hasNext;

                ref TEnumerator enumerator = ref enumerable.enumerator;

                var argument = enumerable.argument;
                var predicate = enumerable.predicate;

                while ((hasNext = enumerator.MoveNext()) && !predicate(enumerator.Current, argument))
                {
                }

                if (hasNext)
                {
                    current = enumerator.Current;

                    return true;
                }

                return false;
            }

            public void Dispose()
            {
                ref TEnumerator enumerator = ref enumerable.enumerator;
                enumerator.Dispose();
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public T Current => current;

            object IEnumerator.Current => Current;
        }
    }
}