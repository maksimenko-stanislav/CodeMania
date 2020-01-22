﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public struct WhereIndexedEnumerable<T, TEnumerator> : IEnumerable<T>
        where TEnumerator : IEnumerator<T>
    {
        private TEnumerator enumerator;
        private readonly Func<T, int, bool> predicate;

        internal WhereIndexedEnumerable(TEnumerator enumerator, Func<T, int, bool> predicate)
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
            private WhereIndexedEnumerable<T, TEnumerator> enumerable;
            private T current;
            private int index;

            public Enumerator(in WhereIndexedEnumerable<T, TEnumerator> enumerable)
            {
                this.enumerable = enumerable;
                current = default;
                index = 0;
            }

            public bool MoveNext()
            {
                bool hasNext;

                ref TEnumerator enumerator = ref enumerable.enumerator;

                var predicate = enumerable.predicate;

                int i = index;
                while ((hasNext = enumerator.MoveNext()) && !predicate(enumerator.Current, i++))
                {
                }

                index = i;

                if (hasNext)
                {
                    current = enumerator.Current;

                    return true;
                }

                return false;
            }

            public void Dispose()
            {
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public T Current => current;

            object IEnumerator.Current => Current;
        }
    }

    public struct WhereIndexedEnumerable<T, TArgument, TEnumerator> : IEnumerable<T>
        where TEnumerator : IEnumerator<T>
    {
        private TEnumerator enumerator;
        private readonly TArgument argument;
        private readonly ParametrizedIndexedPredicate<T, TArgument> predicate;

        internal WhereIndexedEnumerable(TEnumerator enumerator, TArgument argument, ParametrizedIndexedPredicate<T, TArgument> predicate)
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
            private WhereIndexedEnumerable<T, TArgument, TEnumerator> enumerable;
            private T current;
            private int index;

            public Enumerator(in WhereIndexedEnumerable<T, TArgument, TEnumerator> enumerable)
            {
                this.enumerable = enumerable;
                current = default;
                index = 0;
            }

            public bool MoveNext()
            {
                bool hasNext;

                ref TEnumerator enumerator = ref enumerable.enumerator;

                var predicate = enumerable.predicate;

                int i = index;
                while ((hasNext = enumerator.MoveNext()) && !predicate(enumerator.Current, enumerable.argument, i++))
                {
                }

                index = i;

                if (hasNext)
                {
                    current = enumerator.Current;

                    return true;
                }

                return false;
            }

            public void Dispose()
            {
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