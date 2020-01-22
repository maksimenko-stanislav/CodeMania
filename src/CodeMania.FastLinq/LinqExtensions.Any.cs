using System;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        #region Any

        // TODO: Add FastAny from Dictionary
        public static bool FastAny<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            if (source is List<T> list)
            {
                // doesn't allocate boxed enumerable
                foreach (var item in list)
                {
                    if (predicate(item))
                    {
                        return true;
                    }
                }
            }
            else if (source is T[] array)
            {
                // doesn't allocate boxed enumerable
                foreach (var item in array)
                {
                    if (predicate(item))
                    {
                        return true;
                    }
                }
            }
            else if (source is HashSet<T> set)
            {
                // doesn't allocate boxed enumerable
                foreach (var item in set)
                {
                    if (predicate(item))
                    {
                        return true;
                    }
                }
            }
            else if (source is SortedSet<T> sortedSet)
            {
                // doesn't allocate boxed enumerable
                foreach (var item in sortedSet)
                {
                    if (predicate(item))
                    {
                        return true;
                    }
                }
            }
            else
            {
                // May allocate boxed enumerator
                foreach (var item in source)
                {
                    if (predicate(item))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool Any<T, TArg>(this IEnumerable<T> source, TArg arg, Func<T, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            // May allocate boxed enumerator
            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Any<T, TArg>(this List<T> source, TArg arg, Func<T, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Any<T, TArg>(this T[] source, TArg arg, Func<T, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Any<TKey, TValue, TArg>(this Dictionary<TKey, TValue> source, TArg arg, Func<KeyValuePair<TKey, TValue>, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Any<TKey, TValue, TArg>(this SortedDictionary<TKey, TValue> source, TArg arg, Func<KeyValuePair<TKey, TValue>, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Any<T, TArg>(this HashSet<T> source, TArg arg, Func<T, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Any<T, TArg>(this SortedSet<T> source, TArg arg, Func<T, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                {
                    return true;
                }
            }

            return false;
        }

        // optimized

        public static bool Any<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return true;
            }

            return false;
        }

        public static bool Any<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }

            return false;
        }

        public static bool Any<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return true;
            }

            return false;
        }

        public static bool Any<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }

            return false;
        }

        public static bool Any<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return true;
            }

            return false;
        }

        public static bool Any<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }

            return false;
        }

        public static bool Any<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            foreach (var item in source)
            {
                return true;
            }

            return false;
        }

        public static bool Any<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }

            return false;
        }

        public static bool Any<T, TSrcArg, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TSrcArg, TEnumerator> source,
            TArg arg,
            ParametrizedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return true;
            }

            return false;
        }

        public static bool Any<T, TSrcArg, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TSrcArg, TEnumerator> source,
            TArg arg,
            ParametrizedIndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return true;
            }

            return false;
        }

        public static bool Any<T, TSrcArg, TArg, TEnumerator>(
            this WhereEnumerable<T, TSrcArg, TEnumerator> source,
            TArg arg,
            ParametrizedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return true;
            }

            return false;
        }

        public static bool Any<T, TSrcArg, TArg, TEnumerator>(
            this WhereEnumerable<T, TSrcArg, TEnumerator> source,
            TArg arg,
            ParametrizedIndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return true;
            }

            return false;
        }

        public static bool Any<T, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TEnumerator> source,
            TArg arg,
            ParametrizedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return true;
            }

            return false;
        }

        public static bool Any<T, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TEnumerator> source,
            TArg arg,
            ParametrizedIndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return true;
            }

            return false;
        }

        public static bool Any<T, TArg, TEnumerator>(
            this WhereEnumerable<T, TEnumerator> source,
            TArg arg,
            ParametrizedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return true;
            }

            return false;
        }

        public static bool Any<T, TArg, TEnumerator>(
            this WhereEnumerable<T, TEnumerator> source,
            TArg arg,
            ParametrizedIndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return true;
            }

            return false;
        }

        // select

        public static bool Any<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source)
            where TEnumerator : IEnumerator<TSource>
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext()) return true;
            }

            return false;
        }

        public static bool Any<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source)
            where TEnumerator : IEnumerator<TSource>
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext()) return true;
            }

            return false;
        }

        public static bool Any<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source)
            where TEnumerator : IEnumerator<TSource>
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext()) return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source)
            where TEnumerator : IEnumerator<TSource>
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext()) return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, i++))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, TArg arg, ParametrizedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, TArg arg, ParametrizedIndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, TArg arg, ParametrizedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, TArg arg, ParametrizedIndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TSrcArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, ParametrizedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TSrcArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, ParametrizedIndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TSrcArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, ParametrizedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (predicate(item, arg))
                    return true;
            }

            return false;
        }

        public static bool Any<TSource, TArg, TSrcArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, ParametrizedIndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (predicate(item, arg, i++))
                    return true;
            }

            return false;
        }

        #endregion
    }
}