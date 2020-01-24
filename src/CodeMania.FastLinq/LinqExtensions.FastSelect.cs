using System;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        #region FastSelect

        public static SelectEnumerable<TSource, TResult, IEnumerator<TSource>> FastSelect<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            return SelectEnumerable.From(CheckSource(source).GetEnumerator(), selector);
        }

        public static SelectEnumerable<TSource, TResult, List<TSource>.Enumerator> FastSelect<TSource, TResult>(
            this List<TSource> source,
            Func<TSource, TResult> selector)
        {
            return SelectEnumerable.From(CheckSource(source).GetEnumerator(), selector);
        }

        public static SelectEnumerable<TSource, TResult, ArrayEnumerable<TSource>.Enumerator> FastSelect<TSource, TResult>(
            this TSource[] source,
            Func<TSource, TResult> selector)
        {
            return SelectEnumerable.From(new ArrayEnumerable<TSource>(CheckSource(source)).GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSource, TResult, IEnumerator<TSource>> FastSelect<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, int, TResult> selector)
        {
            return SelectIndexedEnumerable.From(CheckSource(source).GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSource, TResult, List<TSource>.Enumerator> FastSelect<TSource, TResult>(
            this List<TSource> source,
            Func<TSource, int, TResult> selector)
        {
            return SelectIndexedEnumerable.From(CheckSource(source).GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSource, TResult, ArrayEnumerable<TSource>.Enumerator> FastSelect<TSource, TResult>(
            this TSource[] source,
            Func<TSource, int, TResult> selector)
        {
            return SelectIndexedEnumerable.From(new ArrayEnumerable<TSource>(CheckSource(source)).GetEnumerator(), selector);
        }

        public static SelectEnumerable<TSource, TResult, HashSet<TSource>.Enumerator> FastSelect<TSource, TResult>(
            this HashSet<TSource> source,
            Func<TSource, TResult> selector)
        {
            return SelectEnumerable.From(CheckSource(source).GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSource, TResult, HashSet<TSource>.Enumerator> FastSelect<TSource, TResult>(
            this HashSet<TSource> source,
            Func<TSource, int, TResult> selector)
        {
            return SelectIndexedEnumerable.From(CheckSource(source).GetEnumerator(), selector);
        }

        public static SelectEnumerable<TSource, TResult, SortedSet<TSource>.Enumerator> FastSelect<TSource, TResult>(
            this SortedSet<TSource> source,
            Func<TSource, TResult> selector)
        {
            return SelectEnumerable.From(CheckSource(source).GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSource, TResult, SortedSet<TSource>.Enumerator> FastSelect<TSource, TResult>(
            this SortedSet<TSource> source,
            Func<TSource, int, TResult> selector)
        {
            return SelectIndexedEnumerable.From(CheckSource(source).GetEnumerator(), selector);
        }

        public static SelectEnumerable<KeyValuePair<TKey, TValue>, TResult, Dictionary<TKey, TValue>.Enumerator> FastSelect<TKey, TValue, TResult>(
            this Dictionary<TKey, TValue> source,
            Func<KeyValuePair<TKey, TValue>, TResult> selector)
        {
            return SelectEnumerable.From(CheckSource(source).GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<KeyValuePair<TKey, TValue>, TResult, Dictionary<TKey, TValue>.Enumerator> FastSelect<TKey, TValue, TResult>(
            this Dictionary<TKey, TValue> source,
            Func<KeyValuePair<TKey, TValue>, int, TResult> selector)
        {
            return SelectIndexedEnumerable.From(CheckSource(source).GetEnumerator(), selector);
        }

        public static SelectEnumerable<KeyValuePair<TKey, TValue>, TResult, SortedDictionary<TKey, TValue>.Enumerator> FastSelect<TKey, TValue, TResult>(
            this SortedDictionary<TKey, TValue> source,
            Func<KeyValuePair<TKey, TValue>, TResult> selector)
        {
            return SelectEnumerable.From(CheckSource(source).GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<KeyValuePair<TKey, TValue>, TResult, SortedDictionary<TKey, TValue>.Enumerator> FastSelect<TKey, TValue, TResult>(
            this SortedDictionary<TKey, TValue> source,
            Func<KeyValuePair<TKey, TValue>, int, TResult> selector)
        {
            return SelectIndexedEnumerable.From(CheckSource(source).GetEnumerator(), selector);
        }

        #endregion
    }
}