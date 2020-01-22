using System;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        private const string NoMatchingElement = "Sequence contains no matching element.";
        private const string NoElements = "Sequence contains no elements.";

        #region First

        public static T First<T, TArg>(this IEnumerable<T> source, TArg arg, ParametrizedPredicate<T, TArg> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            if (source is List<T> list)
            {
                return list.First(arg, predicate);
            }

            if (source is T[] array)
            {
                return array.First(arg, predicate);
            }

            if (source is HashSet<T> set)
            {
                return set.First(arg, predicate);
            }

            if (source is SortedSet<T> sortedSet)
            {
                return sortedSet.First(arg, predicate);
            }

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return item;
                }
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TArg>(this List<T> source, TArg arg, ParametrizedPredicate<T, TArg> predicate)
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

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TArg>(this T[] source, TArg arg, ParametrizedPredicate<T, TArg> predicate)
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

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TArg>(this HashSet<T> source, TArg arg, ParametrizedPredicate<T, TArg> predicate)
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

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TArg>(this SortedSet<T> source, TArg arg, ParametrizedPredicate<T, TArg> predicate)
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

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static KeyValuePair<TKey, TValue> First<TKey, TValue, TArg>(this Dictionary<TKey, TValue> source, TArg arg, ParametrizedPredicate<KeyValuePair<TKey, TValue>, TArg> predicate)
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

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static KeyValuePair<TKey, TValue> First<TKey, TValue, TArg>(this SortedDictionary<TKey, TValue> source, TArg arg, ParametrizedPredicate<KeyValuePair<TKey, TValue>, TArg> predicate)
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

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            if (source is List<T> list)
            {
                return list.First(predicate);
            }

            if (source is T[] array)
            {
                return array.First(predicate);
            }

            if (source is HashSet<T> set)
            {
                return set.First(predicate);
            }

            if (source is SortedSet<T> sortedSet)
            {
                return sortedSet.First(predicate);
            }

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T>(this List<T> source, Predicate<T> predicate)
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

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T>(this T[] source, Predicate<T> predicate)
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

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T>(this HashSet<T> source, Predicate<T> predicate)
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

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T>(this SortedSet<T> source, Predicate<T> predicate)
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

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static KeyValuePair<TKey, TValue> First<TKey, TValue>(this Dictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> predicate)
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

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static KeyValuePair<TKey, TValue> First<TKey, TValue>(this SortedDictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> predicate)
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

            throw new InvalidOperationException(NoMatchingElement);
        }

        // optimized

        // where

        public static T First<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return item;
            }

            throw new InvalidOperationException(NoElements);
        }

        public static T First<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> source, Func<T, int, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return item;
            }

            throw new InvalidOperationException(NoElements);
        }

        public static T First<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> source, Func<T, int, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return item;
            }

            throw new InvalidOperationException(NoElements);
        }

        public static T First<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source, Func<T, int, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return item;
            }

            throw new InvalidOperationException(NoElements);
        }

        public static T First<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> source, Func<T, int, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TArg, TEnumerator>(this WhereEnumerable<T, TEnumerator> source, TArg arg, ParametrizedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TArg, TEnumerator>(this WhereEnumerable<T, TEnumerator> source, TArg arg, ParametrizedIndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TSrcArg, TArg, TEnumerator>(this WhereEnumerable<T, TSrcArg, TEnumerator> source, TArg arg, ParametrizedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TSrcArg, TArg, TEnumerator>(this WhereEnumerable<T, TSrcArg, TEnumerator> source, TArg arg, ParametrizedIndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source, TArg arg, ParametrizedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source, TArg arg, ParametrizedIndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TSrcArg, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TSrcArg, TEnumerator> source, TArg arg, ParametrizedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static T First<T, TSrcArg, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TSrcArg, TEnumerator> source, TArg arg, ParametrizedIndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        // select

        public static TResult First<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source)
            where TEnumerator : IEnumerator<TSource>
        {
            foreach (var item in source)
            {
                return item;
            }

            throw new InvalidOperationException(NoElements);
        }

        public static TResult First<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source)
            where TEnumerator : IEnumerator<TSource>
        {
            foreach (var item in source)
            {
                return item;
            }

            throw new InvalidOperationException(NoElements);
        }

        public static TResult First<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source)
            where TEnumerator : IEnumerator<TSource>
        {
            foreach (var item in source)
            {
                return item;
            }

            throw new InvalidOperationException(NoElements);
        }

        public static TResult First<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source)
            where TEnumerator : IEnumerator<TSource>
        {
            foreach (var item in source)
            {
                return item;
            }

            throw new InvalidOperationException(NoElements);
        }

        public static TResult First<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        // TODO: parametrized first

        public static TResult First<TSource, TResult, TArg, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, TArg arg, ParametrizedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TResult, TArg, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, TArg arg, ParametrizedIndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TResult, TArg, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, TArg arg, ParametrizedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TResult, TArg, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, TArg arg, ParametrizedIndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TSrcArg, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, ParametrizedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TSrcArg, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, ParametrizedIndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TSrcArg, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, ParametrizedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        public static TResult First<TSource, TSrcArg, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, ParametrizedIndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return item;
            }

            throw new InvalidOperationException(NoMatchingElement);
        }

        #endregion
    }
}