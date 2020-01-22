using System;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        #region Where

        // non-indexed

        public static WhereEnumerable<T, TArg, IEnumerator<T>> Where<T, TArg>(
            this IEnumerable<T> source,
            TArg arg,
            ParametrizedPredicate<T, TArg> predicate)
        {
            return new WhereEnumerable<T, TArg, IEnumerator<T>>(
                CheckSource(source).GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<T, TArg, List<T>.Enumerator> Where<T, TArg>(
            this List<T> source,
            TArg arg,
            ParametrizedPredicate<T, TArg> predicate)
        {
            return new WhereEnumerable<T, TArg, List<T>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<T, TArg, ArrayEnumerable<T>.Enumerator> Where<T, TArg>(
            this T[] source,
            TArg arg,
            ParametrizedPredicate<T, TArg> predicate)
        {
            return new WhereEnumerable<T, TArg, ArrayEnumerable<T>.Enumerator>(
                new ArrayEnumerable<T>(CheckSource(source)).GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<KeyValuePair<TKey, TValue>, TArg, Dictionary<TKey, TValue>.Enumerator> Where<TKey, TValue, TArg>(
            this Dictionary<TKey, TValue> source,
            TArg arg,
            ParametrizedPredicate<KeyValuePair<TKey, TValue>, TArg> predicate)
        {
            return new WhereEnumerable<KeyValuePair<TKey, TValue>, TArg, Dictionary<TKey, TValue>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        // indexed

        public static WhereIndexedEnumerable<T, TArg, IEnumerator<T>> Where<T, TArg>(
            this IEnumerable<T> source,
            TArg arg,
            ParametrizedIndexedPredicate<T, TArg> predicate)
        {
            return new WhereIndexedEnumerable<T, TArg, IEnumerator<T>>(
                CheckSource(source).GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<T, TArg, List<T>.Enumerator> Where<T, TArg>(
            this List<T> source,
            TArg arg,
            ParametrizedIndexedPredicate<T, TArg> predicate)
        {
            return new WhereIndexedEnumerable<T, TArg, List<T>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<T, TArg, ArrayEnumerable<T>.Enumerator> Where<T, TArg>(
            this T[] source,
            TArg arg,
            ParametrizedIndexedPredicate<T, TArg> predicate)
        {
            return new WhereIndexedEnumerable<T, TArg, ArrayEnumerable<T>.Enumerator>(
                new ArrayEnumerable<T>(CheckSource(source)).GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<KeyValuePair<TKey, TValue>, TArg, Dictionary<TKey, TValue>.Enumerator> Where<TKey, TValue, TArg>(
            this Dictionary<TKey, TValue> source,
            TArg arg,
            ParametrizedIndexedPredicate<KeyValuePair<TKey, TValue>, TArg> predicate)
        {
            return new WhereIndexedEnumerable<KeyValuePair<TKey, TValue>, TArg, Dictionary<TKey, TValue>.Enumerator>(
                CheckSource(source).GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        // optimized

        // where -> where

        // WhereEnumerable<T, TArg, TEnumerator>
        public static WhereEnumerable<T, TArg, WhereEnumerable<T, TEnumerator>.Enumerator> Where<T, TArg, TEnumerator>(
            this WhereEnumerable<T, TEnumerator> source,
            TArg arg,
            ParametrizedPredicate<T, TArg> predicate) where TEnumerator : IEnumerator<T>
        {
            return new WhereEnumerable<T, TArg, WhereEnumerable<T, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<T, TArg, WhereEnumerable<T, TArg, TEnumerator>.Enumerator> Where<T, TArg, TEnumerator>(
            this WhereEnumerable<T, TArg, TEnumerator> source,
            TArg arg,
            ParametrizedPredicate<T, TArg> predicate) where TEnumerator : IEnumerator<T>
        {
            return new WhereEnumerable<T, TArg, WhereEnumerable<T, TArg, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<T, TArg, WhereIndexedEnumerable<T, TEnumerator>.Enumerator> Where<T, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TEnumerator> source,
            TArg arg,
            ParametrizedPredicate<T, TArg> predicate) where TEnumerator : IEnumerator<T>
        {
            return new WhereEnumerable<T, TArg, WhereIndexedEnumerable<T, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<T, TArg, WhereIndexedEnumerable<T, TArg, TEnumerator>.Enumerator> Where<T, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TArg, TEnumerator> source,
            TArg arg,
            ParametrizedPredicate<T, TArg> predicate) where TEnumerator : IEnumerator<T>
        {
            return new WhereEnumerable<T, TArg, WhereIndexedEnumerable<T, TArg, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        // WhereIndexedEnumerable<T, TArg, TEnumerator>
        public static WhereIndexedEnumerable<T, TArg, WhereEnumerable<T, TEnumerator>.Enumerator> Where<T, TArg, TEnumerator>(
            this WhereEnumerable<T, TEnumerator> source,
            TArg arg,
            ParametrizedIndexedPredicate<T, TArg> predicate) where TEnumerator : IEnumerator<T>
        {
            return new WhereIndexedEnumerable<T, TArg, WhereEnumerable<T, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<T, TArg, WhereEnumerable<T, TArg, TEnumerator>.Enumerator> Where<T, TArg, TEnumerator>(
            this WhereEnumerable<T, TArg, TEnumerator> source,
            TArg arg,
            ParametrizedIndexedPredicate<T, TArg> predicate) where TEnumerator : IEnumerator<T>
        {
            return new WhereIndexedEnumerable<T, TArg, WhereEnumerable<T, TArg, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<T, TArg, WhereIndexedEnumerable<T, TEnumerator>.Enumerator> Where<T, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TEnumerator> source,
            TArg arg,
            ParametrizedIndexedPredicate<T, TArg> predicate) where TEnumerator : IEnumerator<T>
        {
            return new WhereIndexedEnumerable<T, TArg, WhereIndexedEnumerable<T, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<T, TArg, WhereIndexedEnumerable<T, TArg, TEnumerator>.Enumerator> Where<T, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TArg, TEnumerator> source,
            TArg arg,
            ParametrizedIndexedPredicate<T, TArg> predicate) where TEnumerator : IEnumerator<T>
        {
            return new WhereIndexedEnumerable<T, TArg, WhereIndexedEnumerable<T, TArg, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        // WhereEnumerable<T, TEnumerator>
        public static WhereEnumerable<T, WhereEnumerable<T, TEnumerator>.Enumerator> Where<T, TEnumerator>(
            this WhereEnumerable<T, TEnumerator> source,
            Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            return new WhereEnumerable<T, WhereEnumerable<T, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<T, WhereEnumerable<T, TArg, TEnumerator>.Enumerator> Where<T, TArg, TEnumerator>(
            this WhereEnumerable<T, TArg, TEnumerator> source,
            Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            return new WhereEnumerable<T, WhereEnumerable<T, TArg, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<T, WhereIndexedEnumerable<T, TEnumerator>.Enumerator> Where<T, TEnumerator>(
            this WhereIndexedEnumerable<T, TEnumerator> source,
            Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            return new WhereEnumerable<T, WhereIndexedEnumerable<T, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<T, WhereIndexedEnumerable<T, TArg, TEnumerator>.Enumerator> Where<T, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TArg, TEnumerator> source,
            Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            return new WhereEnumerable<T, WhereIndexedEnumerable<T, TArg, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        // WhereIndexedEnumerable<T, TEnumerator>
        public static WhereIndexedEnumerable<T, WhereEnumerable<T, TEnumerator>.Enumerator> Where<T, TEnumerator>(
            this WhereEnumerable<T, TEnumerator> source,
            Func<T, int, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            return new WhereIndexedEnumerable<T, WhereEnumerable<T, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<T, WhereEnumerable<T, TArg, TEnumerator>.Enumerator> Where<T, TArg, TEnumerator>(
            this WhereEnumerable<T, TArg, TEnumerator> source,
            Func<T, int, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            return new WhereIndexedEnumerable<T, WhereEnumerable<T, TArg, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<T, WhereIndexedEnumerable<T, TEnumerator>.Enumerator> Where<T, TEnumerator>(
            this WhereIndexedEnumerable<T, TEnumerator> source,
            Func<T, int, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            return new WhereIndexedEnumerable<T, WhereIndexedEnumerable<T, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<T, WhereIndexedEnumerable<T, TArg, TEnumerator>.Enumerator> Where<T, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TArg, TEnumerator> source,
            Func<T, int, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            return new WhereIndexedEnumerable<T, WhereIndexedEnumerable<T, TArg, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        // select -> where

        // WhereEnumerable<T, TArg, TEnumerator>
        public static WhereEnumerable<TResult, TArg, SelectEnumerable<TSource, TResult, TSourceEnumerator>.Enumerator> Where<TSource, TArg, TResult, TSourceEnumerator>(
            this SelectEnumerable<TSource, TResult, TSourceEnumerator> source,
            TArg arg,
            ParametrizedPredicate<TResult, TArg> predicate)
            where TSourceEnumerator : IEnumerator<TSource>
        {
            return new WhereEnumerable<TResult, TArg, SelectEnumerable<TSource, TResult, TSourceEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<TResult, TArg, SelectEnumerable<TSource, TSrcArg, TResult, TSourceEnumerator>.Enumerator> Where<TSource, TSrcArg, TArg, TResult, TSourceEnumerator>(
            this SelectEnumerable<TSource, TSrcArg, TResult, TSourceEnumerator> source,
            TArg arg,
            ParametrizedPredicate<TResult, TArg> predicate)
            where TSourceEnumerator : IEnumerator<TSource>
        {
            return new WhereEnumerable<TResult, TArg, SelectEnumerable<TSource, TSrcArg, TResult, TSourceEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<TResult, TArg, SelectIndexedEnumerable<TSource, TResult, TSourceEnumerator>.Enumerator> Where<TSource, TResult, TArg, TSourceEnumerator>(
            this SelectIndexedEnumerable<TSource, TResult, TSourceEnumerator> source,
            TArg arg,
            ParametrizedPredicate<TResult, TArg> predicate)
            where TSourceEnumerator : IEnumerator<TSource>
        {
            return new WhereEnumerable<TResult, TArg, SelectIndexedEnumerable<TSource, TResult, TSourceEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<TResult, TArg, SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator>.Enumerator> Where<TSource, TSrcArg, TArg, TResult, TEnumerator>(
            this SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator> source,
            TArg arg,
            ParametrizedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            return new WhereEnumerable<TResult, TArg, SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        // WhereIndexedEnumerable<T, TArg, TEnumerator>
        public static WhereIndexedEnumerable<TResult, TArg, SelectEnumerable<TSource, TResult, TSourceEnumerator>.Enumerator> Where<TSource, TResult, TArg, TSourceEnumerator>(
            this SelectEnumerable<TSource, TResult, TSourceEnumerator> source,
            TArg arg,
            ParametrizedIndexedPredicate<TResult, TArg> predicate)
            where TSourceEnumerator : IEnumerator<TSource>
        {
            return new WhereIndexedEnumerable<TResult, TArg, SelectEnumerable<TSource, TResult, TSourceEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<TResult, TArg, SelectEnumerable<TSource, TSrcArg, TResult, TSourceEnumerator>.Enumerator> Where<TSource, TSrcArg, TArg, TResult, TSourceEnumerator>(
            this SelectEnumerable<TSource, TSrcArg, TResult, TSourceEnumerator> source,
            TArg arg,
            ParametrizedIndexedPredicate<TResult, TArg> predicate) where TSourceEnumerator : IEnumerator<TSource>
        {
            return new WhereIndexedEnumerable<TResult, TArg, SelectEnumerable<TSource, TSrcArg, TResult, TSourceEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<TResult, TArg, SelectIndexedEnumerable<TSource, TResult, TSourceEnumerator>.Enumerator> Where<TSource, TResult, TArg, TSourceEnumerator>(
            this SelectIndexedEnumerable<TSource, TResult, TSourceEnumerator> source,
            TArg arg,
            ParametrizedIndexedPredicate<TResult, TArg> predicate) where TSourceEnumerator : IEnumerator<TSource>
        {
            return new WhereIndexedEnumerable<TResult, TArg, SelectIndexedEnumerable<TSource, TResult, TSourceEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<TResult, TArg, SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator>.Enumerator> Where<TSource, TSrcArg, TArg, TResult, TEnumerator>(
            this SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator> source,
            TArg arg,
            ParametrizedIndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            return new WhereIndexedEnumerable<TResult, TArg, SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                arg,
                CheckPredicate(predicate));
        }

        // WhereEnumerable<T, TEnumerator>
        public static WhereEnumerable<TResult, SelectEnumerable<TSource, TResult, TEnumerator>.Enumerator> Where<TSource, TResult, TEnumerator>(
            this SelectEnumerable<TSource, TResult, TEnumerator> source,
            Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            return new WhereEnumerable<TResult, SelectEnumerable<TSource, TResult, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<TResult, SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator>.Enumerator> Where<TSource, TSrcArg, TResult, TEnumerator>(
            this SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator> source,
            Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            return new WhereEnumerable<TResult, SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<TResult, SelectIndexedEnumerable<TSource, TResult, TEnumerator>.Enumerator> Where<TSource, TResult, TEnumerator>(
            this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source,
            Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            return new WhereEnumerable<TResult, SelectIndexedEnumerable<TSource, TResult, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereEnumerable<TResult, SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator>.Enumerator> Where<TSource, TSrcArg, TResult, TEnumerator>(
            this SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator> source,
            Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            return new WhereEnumerable<TResult, SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        // WhereIndexedEnumerable<T, TEnumerator>
        public static WhereIndexedEnumerable<TResult, SelectEnumerable<TSource, TResult, TEnumerator>.Enumerator> Where<TSource, TResult, TEnumerator>(
            this SelectEnumerable<TSource, TResult, TEnumerator> source,
            Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            return new WhereIndexedEnumerable<TResult, SelectEnumerable<TSource, TResult, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<TResult, SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator>.Enumerator> Where<TSource, TSrcArg, TResult, TEnumerator>(
            this SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator> source,
            Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            return new WhereIndexedEnumerable<TResult, SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<TResult, SelectIndexedEnumerable<TSource, TResult, TEnumerator>.Enumerator> Where<TSource, TResult, TEnumerator>(
            this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source,
            Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            return new WhereIndexedEnumerable<TResult, SelectIndexedEnumerable<TSource, TResult, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        public static WhereIndexedEnumerable<TResult, SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator>.Enumerator> Where<TSource, TSrcArg, TResult, TEnumerator>(
            this SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator> source,
            Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            return new WhereIndexedEnumerable<TResult, SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator>.Enumerator>(
                source.GetEnumerator(),
                CheckPredicate(predicate));
        }

        #endregion
    }
}