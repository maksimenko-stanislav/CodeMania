using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public struct SelectEnumerable<TSource, TResult, TEnumerator> : IEnumerable<TResult>
        where TEnumerator : IEnumerator<TSource>
    {
        private TEnumerator enumerator;
        private readonly Func<TSource, TResult> selector;

        internal SelectEnumerable(TEnumerator enumerator, Func<TSource, TResult> selector)
        {
            this.enumerator = enumerator;
            this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public Enumerator GetEnumerator()
        {
            if (selector == null)
            {
                throw new InvalidOperationException("Use non-default constructor to create current enumerable type.");
            }

            return new Enumerator(this);
        }

        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<TResult>
        {
            private SelectEnumerable<TSource, TResult, TEnumerator> enumerable;
            private TResult current;

            internal Enumerator(in SelectEnumerable<TSource, TResult, TEnumerator> enumerable)
            {
                this.enumerable = enumerable;
                current = default;
            }

            public bool MoveNext()
            {
                ref TEnumerator enumerator = ref enumerable.enumerator;

                var hasItems = enumerator.MoveNext();

                if (hasItems)
                {
                    current = enumerable.selector(enumerator.Current);

                    return true;
                }

                return false;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public TResult Current => current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                ref TEnumerator enumerator = ref enumerable.enumerator;
                enumerator.Dispose();
            }
        }
    }

    public struct SelectEnumerable<TSource, TArg, TResult, TEnumerator> : IEnumerable<TResult>
        where TEnumerator : IEnumerator<TSource>
    {
        private TEnumerator enumerator;
        private readonly Selector<TSource, TArg, TResult> selector;
        private readonly TArg arg;

        internal SelectEnumerable(TEnumerator enumerator, TArg arg, Selector<TSource, TArg, TResult> selector)
        {
            this.enumerator = enumerator;
            this.arg = arg;
            this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public Enumerator GetEnumerator()
        {
            if (selector == null)
            {
                throw new InvalidOperationException("Use non-default constructor to create current enumerable type.");
            }

            return new Enumerator(this);
        }

        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<TResult>
        {
            private SelectEnumerable<TSource, TArg, TResult, TEnumerator> enumerable;
            private TResult current;

            internal Enumerator(in SelectEnumerable<TSource, TArg, TResult, TEnumerator> enumerable)
            {
                this.enumerable = enumerable;
                current = default;
            }

            public bool MoveNext()
            {
                ref TEnumerator enumerator = ref enumerable.enumerator;

                var hasItems = enumerator.MoveNext();

                if (hasItems)
                {
                    current = enumerable.selector(enumerator.Current, enumerable.arg);

                    return true;
                }

                return false;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public TResult Current => current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                ref TEnumerator enumerator = ref enumerable.enumerator;
                enumerator.Dispose();
            }
        }
    }
}