﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public struct SelectIndexedEnumerable<TSource, TResult, TEnumerator> : IEnumerable<TResult>
        where TEnumerator : IEnumerator<TSource>
    {
        private TEnumerator enumerator;
        private readonly Func<TSource, int, TResult> selector;

        internal SelectIndexedEnumerable(TEnumerator enumerator, Func<TSource, int, TResult> selector)
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
            private SelectIndexedEnumerable<TSource, TResult, TEnumerator> enumerable;

            private TResult current;
            private int index;

            internal Enumerator(in SelectIndexedEnumerable<TSource, TResult, TEnumerator> enumerable)
            {
                this.enumerable = enumerable;
                current = default;
                index = 0;
            }

            public bool MoveNext()
            {
                ref TEnumerator enumerator = ref enumerable.enumerator;

                var hasItems = enumerator.MoveNext();

                if (hasItems)
                {
                    current = enumerable.selector(enumerator.Current, index++);

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

    public struct SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> : IEnumerable<TResult>
        where TEnumerator : IEnumerator<TSource>
    {
        private TEnumerator enumerator;
        private readonly ParametrizedIndexedProjection<TSource, TArg, TResult> selector;
        private readonly TArg argument;

        internal SelectIndexedEnumerable(TEnumerator enumerator, TArg argument, ParametrizedIndexedProjection<TSource, TArg, TResult> selector)
        {
            this.enumerator = enumerator;
            this.argument = argument;
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
            private SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> enumerable;

            private TResult current;
            private int index;

            internal Enumerator(in SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> enumerable)
            {
                this.enumerable = enumerable;
                current = default;
                index = 0;
            }

            public bool MoveNext()
            {
                ref TEnumerator enumerator = ref enumerable.enumerator;

                var hasItems = enumerator.MoveNext();

                if (hasItems)
                {
                    current = enumerable.selector(enumerator.Current, enumerable.argument, index++);

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