using System;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        #region FirstOrDefault

        public static T FirstOrDefault<T, TArg>(this IEnumerable<T> source, TArg arg, Predicate<T, TArg> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            if (source is List<T> list)
            {
                return list.FirstOrDefault(arg, predicate);
            }

            if (source is T[] array)
            {
                return array.FirstOrDefault(arg, predicate);
            }

            if (source is HashSet<T> set)
            {
                return set.FirstOrDefault(arg, predicate);
            }

            if (source is SortedSet<T> sortedSet)
            {
                return sortedSet.FirstOrDefault(arg, predicate);
            }

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return item;
                }
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg>(this List<T> source, TArg arg, Predicate<T, TArg> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return item;
                }
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg>(this T[] source, TArg arg, Predicate<T, TArg> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return item;
                }
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg>(this HashSet<T> source, TArg arg, Predicate<T, TArg> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return item;
                }
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg>(this SortedSet<T> source, TArg arg, Predicate<T, TArg> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return item;
                }
            }

            return default;
        }

        public static KeyValuePair<TKey, TValue> FirstOrDefault<TKey, TValue, TArg>(this Dictionary<TKey, TValue> source, TArg arg, Predicate<KeyValuePair<TKey, TValue>, TArg> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return item;
                }
            }

            return default;
        }

        public static KeyValuePair<TKey, TValue> FirstOrDefault<TKey, TValue, TArg>(this SortedDictionary<TKey, TValue> source, TArg arg, Predicate<KeyValuePair<TKey, TValue>, TArg> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return item;
                }
            }

            return default;
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            if (source is List<T> list)
            {
                return list.FirstOrDefault(predicate);
            }

            if (source is T[] array)
            {
                return array.FirstOrDefault(predicate);
            }

            if (source is HashSet<T> set)
            {
                return set.FirstOrDefault(predicate);
            }

            if (source is SortedSet<T> sortedSet)
            {
                return sortedSet.FirstOrDefault(predicate);
            }

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return default;
        }

        public static T FirstOrDefault<T>(this List<T> source, Predicate<T> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return default;
        }

        public static T FirstOrDefault<T>(this T[] source, Predicate<T> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return default;
        }

        public static T FirstOrDefault<T>(this HashSet<T> source, Predicate<T> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return default;
        }

        public static T FirstOrDefault<T>(this SortedSet<T> source, Predicate<T> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return default;
        }

        public static KeyValuePair<TKey, TValue> FirstOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return default;
        }

        public static KeyValuePair<TKey, TValue> FirstOrDefault<TKey, TValue>(this SortedDictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return default;
        }

        // optimized

        // where

        public static T FirstOrDefault<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> source, Func<T, int, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> source, Func<T, int, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source, Func<T, int, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> source, Func<T, int, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg, TEnumerator>(this WhereEnumerable<T, TEnumerator> source, TArg arg, Predicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg, TEnumerator>(this WhereEnumerable<T, TEnumerator> source, TArg arg, IndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TSrcArg, TArg, TEnumerator>(this WhereEnumerable<T, TSrcArg, TEnumerator> source, TArg arg, Predicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TSrcArg, TArg, TEnumerator>(this WhereEnumerable<T, TSrcArg, TEnumerator> source, TArg arg, IndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source, TArg arg, Predicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source, TArg arg, IndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TSrcArg, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TSrcArg, TEnumerator> source, TArg arg, Predicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TSrcArg, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TSrcArg, TEnumerator> source, TArg arg, IndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            return default;
        }

        // select

        public static TResult FirstOrDefault<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source)
            where TEnumerator : IEnumerator<TSource>
        {
            foreach (var item in source)
            {
                return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source)
            where TEnumerator : IEnumerator<TSource>
        {
            foreach (var item in source)
            {
                return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source)
            where TEnumerator : IEnumerator<TSource>
        {
            foreach (var item in source)
            {
                return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source)
            where TEnumerator : IEnumerator<TSource>
        {
            foreach (var item in source)
            {
                return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            return default;
        }

        // TODO: parametrized first

        public static TResult FirstOrDefault<TSource, TResult, TArg, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, TArg arg, Predicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TResult, TArg, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, TArg arg, IndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TResult, TArg, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, TArg arg, Predicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TResult, TArg, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, TArg arg, IndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TSrcArg, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, Predicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TSrcArg, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, IndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TSrcArg, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, Predicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            return default;
        }

        public static TResult FirstOrDefault<TSource, TSrcArg, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, IndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TEnumerator>(this SkipEnumerable<T, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TEnumerator>(this SkipEnumerable<T, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TEnumerator>(this TakeEnumerable<T, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return item;
            }

            return default;
        }

        public static T FirstOrDefault<T, TEnumerator>(this TakeEnumerable<T, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            return default;
        }

        #endregion
    }
}