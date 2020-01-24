using System;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        #region All

        // TODO: Add FastAll from Dictionary
        public static bool FastAll<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            if (source is List<T> list)
            {
                // doesn't allocate boxed enumerable
                foreach (var item in list)
                {
                    if (!predicate(item))
                    {
                        return false;
                    }
                }
            }
            else if (source is T[] array)
            {
                // doesn't allocate boxed enumerable
                foreach (var item in array)
                {
                    if (!predicate(item))
                    {
                        return false;
                    }
                }
            }
            else if (source is HashSet<T> set)
            {
                // doesn't allocate boxed enumerable
                foreach (var item in set)
                {
                    if (!predicate(item))
                    {
                        return false;
                    }
                }
            }
            else if (source is SortedSet<T> sortedSet)
            {
                // doesn't allocate boxed enumerable
                foreach (var item in sortedSet)
                {
                    if (!predicate(item))
                    {
                        return false;
                    }
                }
            }
            else
            {
                // May allocate boxed enumerator
                foreach (var item in source)
                {
                    if (!predicate(item))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool All<T, TArg>(this IEnumerable<T> source, TArg arg, Func<T, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool All<T, TArg>(this List<T> source, TArg arg, Func<T, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool All<T, TArg>(this T[] source, TArg arg, Func<T, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool All<TKey, TValue, TArg>(this Dictionary<TKey, TValue> source, TArg arg, Func<KeyValuePair<TKey, TValue>, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool All<TKey, TValue, TArg>(this SortedDictionary<TKey, TValue> source, TArg arg, Func<KeyValuePair<TKey, TValue>, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool All<T, TArg>(this HashSet<T> source, TArg arg, Func<T, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool All<T, TArg>(this SortedSet<T> source, TArg arg, Func<T, TArg, bool> predicate)
        {
            CheckSource(source);
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                {
                    return false;
                }
            }

            return true;
        }

        // optimized

        public static bool All<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item))
                    return false;
            }

            return true;
        }


        public static bool All<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item))
                    return false;
            }

            return true;
        }

        public static bool All<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item))
                    return false;
            }

            return true;
        }

        public static bool All<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> source, Func<T, bool> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item))
                    return false;
            }

            return true;
        }

        public static bool All<T, TSrcArg, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TSrcArg, TEnumerator> source,
            TArg arg,
            Predicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                    return false;
            }

            return true;
        }

        public static bool All<T, TSrcArg, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TSrcArg, TEnumerator> source,
            TArg arg,
            IndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (!predicate(item, arg, i++))
                    return false;
            }

            return true;
        }

        public static bool All<T, TSrcArg, TArg, TEnumerator>(
            this WhereEnumerable<T, TSrcArg, TEnumerator> source,
            TArg arg,
            Predicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                    return false;
            }

            return true;
        }

        public static bool All<T, TSrcArg, TArg, TEnumerator>(
            this WhereEnumerable<T, TSrcArg, TEnumerator> source,
            TArg arg,
            IndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (!predicate(item, arg, i++))
                    return false;
            }

            return true;
        }

        public static bool All<T, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TEnumerator> source,
            TArg arg,
            Predicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                    return false;
            }

            return true;
        }

        public static bool All<T, TArg, TEnumerator>(
            this WhereIndexedEnumerable<T, TEnumerator> source,
            TArg arg,
            IndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (!predicate(item, arg, i++))
                    return false;
            }

            return true;
        }

        public static bool All<T, TArg, TEnumerator>(
            this WhereEnumerable<T, TEnumerator> source,
            TArg arg,
            Predicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                    return false;
            }

            return true;
        }

        public static bool All<T, TArg, TEnumerator>(
            this WhereEnumerable<T, TEnumerator> source,
            TArg arg,
            IndexedPredicate<T, TArg> predicate)
            where TEnumerator : IEnumerator<T>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (!predicate(item, arg, i++))
                    return false;
            }

            return true;
        }

        // select

        public static bool All<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (!predicate(item, i++))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (!predicate(item, i++))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (!predicate(item, i++))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source, Func<TResult, int, bool> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (!predicate(item, i++))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, TArg arg, Predicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, TArg arg, IndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (!predicate(item, arg, i++))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, TArg arg, Predicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, TArg arg, IndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (!predicate(item, arg, i++))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TArg, TSrcArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, Predicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TArg, TSrcArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, IndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (!predicate(item, arg, i++))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TArg, TSrcArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, Predicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            foreach (var item in source)
            {
                if (!predicate(item, arg))
                    return false;
            }

            return true;
        }

        public static bool All<TSource, TArg, TSrcArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TSrcArg, TResult, TEnumerator> source, TArg arg, IndexedPredicate<TResult, TArg> predicate)
            where TEnumerator : IEnumerator<TSource>
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (var item in source)
            {
                if (!predicate(item, arg, i++))
                    return false;
            }

            return true;
        }

        #endregion

        // TODO: Add Skip/Take support
    }
}