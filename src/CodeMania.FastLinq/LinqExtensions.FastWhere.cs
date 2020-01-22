using System;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        #region FastWhere

        // non-indexed

        public static WhereEnumerable<T, IEnumerator<T>> FastWhere<T>(
            this IEnumerable<T> source,
            Func<T, bool> predicate)
        {
            return new WhereEnumerable<T, IEnumerator<T>>(
                CheckSource(source).GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<T, List<T>.Enumerator> FastWhere<T>(
            this List<T> source,
            Func<T, bool> predicate)
        {
            return new WhereEnumerable<T, List<T>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<T, ArrayEnumerable<T>.Enumerator> FastWhere<T>(
            this T[] source,
            Func<T, bool> predicate)
        {
            return new WhereEnumerable<T, ArrayEnumerable<T>.Enumerator>(
                new ArrayEnumerable<T>(CheckSource(source)).GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator> FastWhere<TKey, TValue>(
            this Dictionary<TKey, TValue> source,
            Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            return new WhereEnumerable<KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<T, HashSet<T>.Enumerator> FastWhere<T>(
            this HashSet<T> source,
            Func<T, bool> predicate)
        {
            return new WhereEnumerable<T, HashSet<T>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<T, SortedSet<T>.Enumerator> FastWhere<T>(
            this SortedSet<T> source,
            Func<T, bool> predicate)
        {
            return new WhereEnumerable<T, SortedSet<T>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<KeyValuePair<TKey, TValue>, SortedDictionary<TKey, TValue>.Enumerator> FastWhere<TKey, TValue>(
            this SortedDictionary<TKey, TValue> source,
            Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            return new WhereEnumerable<KeyValuePair<TKey, TValue>, SortedDictionary<TKey, TValue>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                CheckPredicate(predicate));
        }

        // indexed

        public static WhereIndexedEnumerable<T, IEnumerator<T>> FastWhere<T>(
            this IEnumerable<T> source,
            Func<T, int, bool> predicate)
        {
            return new WhereIndexedEnumerable<T, IEnumerator<T>>(
                CheckSource(source).GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<T, List<T>.Enumerator> FastWhere<T>(
            this List<T> source,
            Func<T, int, bool> predicate)
        {
            return new WhereIndexedEnumerable<T, List<T>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<T, ArrayEnumerable<T>.Enumerator> FastWhere<T>(
            this T[] source,
            Func<T, int, bool> predicate)
        {
            return new WhereIndexedEnumerable<T, ArrayEnumerable<T>.Enumerator>(
                new ArrayEnumerable<T>(CheckSource(source)).GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator> FastWhere<TKey, TValue>(
            this Dictionary<TKey, TValue> source,
            Func<KeyValuePair<TKey, TValue>, int, bool> predicate)
        {
           return new WhereIndexedEnumerable<KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<T, HashSet<T>.Enumerator> FastWhere<T>(
            this HashSet<T> source,
            Func<T, int, bool> predicate)
        {
            return new WhereIndexedEnumerable<T, HashSet<T>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<T, SortedSet<T>.Enumerator> FastWhere<T>(
            this SortedSet<T> source,
            Func<T, int, bool> predicate)
        {
            return new WhereIndexedEnumerable<T, SortedSet<T>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<KeyValuePair<TKey, TValue>, SortedDictionary<TKey, TValue>.Enumerator> FastWhere<TKey, TValue>(
            this SortedDictionary<TKey, TValue> source,
            Func<KeyValuePair<TKey, TValue>, int, bool> predicate)
        {
            return new WhereIndexedEnumerable<KeyValuePair<TKey, TValue>, SortedDictionary<TKey, TValue>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                CheckPredicate(predicate));
        }

        #endregion
    }
}