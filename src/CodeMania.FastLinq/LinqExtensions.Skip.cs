using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        // TODO: Do we need to check count < 0?

        public static SkipEnumerable<T, ArrayEnumerable<T>.Enumerator> Skip<T>(this T[] source, int count)
        {
            return SkipEnumerable<T>.From(new ArrayEnumerable<T>(CheckSource(source)).GetEnumerator(), count);
        }

        public static SkipEnumerable<T, List<T>.Enumerator> Skip<T>(this List<T> source, int count)
        {
            return SkipEnumerable<T>.From(CheckSource(source).GetEnumerator(), count);
        }

        public static SkipEnumerable<T, HashSet<T>.Enumerator> Skip<T>(this HashSet<T> source, int count)
        {
            return SkipEnumerable<T>.From(CheckSource(source).GetEnumerator(), count);
        }

        public static SkipEnumerable<T, SortedSet<T>.Enumerator> Skip<T>(this SortedSet<T> source, int count)
        {
            return SkipEnumerable<T>.From(CheckSource(source).GetEnumerator(), count);
        }

        public static SkipEnumerable<KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator> Skip<TKey, TValue>(this Dictionary<TKey, TValue> source, int count)
        {
            return SkipEnumerable<KeyValuePair<TKey, TValue>>.From(CheckSource(source).GetEnumerator(), count);
        }

        public static SkipEnumerable<KeyValuePair<TKey, TValue>, SortedDictionary<TKey, TValue>.Enumerator> Skip<TKey, TValue>(this SortedDictionary<TKey, TValue> source, int count)
        {
            return SkipEnumerable<KeyValuePair<TKey, TValue>>.From(CheckSource(source).GetEnumerator(), count);
        }

        public static SkipEnumerable<T, SkipEnumerable<T, TEnumerator>.Enumerator> Skip<T, TEnumerator>(this SkipEnumerable<T, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<T>
        {
            return SkipEnumerable<T>.From(source.GetEnumerator(), count);
        }

        public static SkipEnumerable<T, TakeEnumerable<T, TEnumerator>.Enumerator> Skip<T, TEnumerator>(this TakeEnumerable<T, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<T>
        {
            return SkipEnumerable<T>.From(source.GetEnumerator(), count);
        }

        public static SkipEnumerable<T, WhereEnumerable<T, TEnumerator>.Enumerator> Skip<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<T>
        {
            return SkipEnumerable<T>.From(source.GetEnumerator(), count);
        }

        public static SkipEnumerable<T, WhereEnumerable<T, TArg, TEnumerator>.Enumerator> Skip<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<T>
        {
            return SkipEnumerable<T>.From(source.GetEnumerator(), count);
        }

        public static SkipEnumerable<T, WhereIndexedEnumerable<T, TEnumerator>.Enumerator> Skip<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<T>
        {
            return SkipEnumerable<T>.From(source.GetEnumerator(), count);
        }

        public static SkipEnumerable<T, WhereIndexedEnumerable<T, TArg, TEnumerator>.Enumerator> Skip<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<T>
        {
            return SkipEnumerable<T>.From(source.GetEnumerator(), count);
        }

        public static SkipEnumerable<TResult, SelectEnumerable<TSource, TResult, TEnumerator>.Enumerator> Skip<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<TSource>
        {
            return SkipEnumerable<TResult>.From(source.GetEnumerator(), count);
        }

        public static SkipEnumerable<TResult, SelectEnumerable<TSource, TArg, TResult, TEnumerator>.Enumerator> Skip<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<TSource>
        {
            return SkipEnumerable<TResult>.From(source.GetEnumerator(), count);
        }

        public static SkipEnumerable<TResult, SelectIndexedEnumerable<TSource, TResult, TEnumerator>.Enumerator> Skip<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<TSource>
        {
            return SkipEnumerable<TResult>.From(source.GetEnumerator(), count);
        }

        public static SkipEnumerable<TResult, SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator>.Enumerator> Skip<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<TSource>
        {
            return SkipEnumerable<TResult>.From(source.GetEnumerator(), count);
        }
    }
}