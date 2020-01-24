using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        // TODO: Do we need to check count < 0?

        public static TakeEnumerable<T, ArrayEnumerable<T>.Enumerator> Take<T>(this T[] source, int count)
        {
            return TakeEnumerable<T>.From(new ArrayEnumerable<T>(CheckSource(source)).GetEnumerator(), count);
        }

        public static TakeEnumerable<T, List<T>.Enumerator> Take<T>(this List<T> source, int count)
        {
            return TakeEnumerable<T>.From(CheckSource(source).GetEnumerator(), count);
        }

        public static TakeEnumerable<T, HashSet<T>.Enumerator> Take<T>(this HashSet<T> source, int count)
        {
            return TakeEnumerable<T>.From(CheckSource(source).GetEnumerator(), count);
        }

        public static TakeEnumerable<T, SortedSet<T>.Enumerator> Take<T>(this SortedSet<T> source, int count)
        {
            return TakeEnumerable<T>.From(CheckSource(source).GetEnumerator(), count);
        }

        public static TakeEnumerable<KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator> Take<TKey, TValue>(this Dictionary<TKey, TValue> source, int count)
        {
            return TakeEnumerable<KeyValuePair<TKey, TValue>>.From(CheckSource(source).GetEnumerator(), count);
        }

        public static TakeEnumerable<KeyValuePair<TKey, TValue>, SortedDictionary<TKey, TValue>.Enumerator> Take<TKey, TValue>(this SortedDictionary<TKey, TValue> source, int count)
        {
            return TakeEnumerable<KeyValuePair<TKey, TValue>>.From(CheckSource(source).GetEnumerator(), count);
        }

        public static TakeEnumerable<T, TakeEnumerable<T, TEnumerator>.Enumerator> Take<T, TEnumerator>(this TakeEnumerable<T, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<T>
        {
            return TakeEnumerable<T>.From(source.GetEnumerator(), count);
        }

        public static TakeEnumerable<T, SkipEnumerable<T, TEnumerator>.Enumerator> Take<T, TEnumerator>(this SkipEnumerable<T, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<T>
        {
            return TakeEnumerable<T>.From(source.GetEnumerator(), count);
        }

        public static TakeEnumerable<T, WhereEnumerable<T, TEnumerator>.Enumerator> Take<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<T>
        {
            return TakeEnumerable<T>.From(source.GetEnumerator(), count);
        }

        public static TakeEnumerable<T, WhereEnumerable<T, TArg, TEnumerator>.Enumerator> Take<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<T>
        {
            return TakeEnumerable<T>.From(source.GetEnumerator(), count);
        }

        public static TakeEnumerable<T, WhereIndexedEnumerable<T, TEnumerator>.Enumerator> Take<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<T>
        {
            return TakeEnumerable<T>.From(source.GetEnumerator(), count);
        }

        public static TakeEnumerable<T, WhereIndexedEnumerable<T, TArg, TEnumerator>.Enumerator> Take<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<T>
        {
            return TakeEnumerable<T>.From(source.GetEnumerator(), count);
        }

        public static TakeEnumerable<TResult, SelectEnumerable<TSource, TResult, TEnumerator>.Enumerator> Take<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<TSource>
        {
            return TakeEnumerable<TResult>.From(source.GetEnumerator(), count);
        }

        public static TakeEnumerable<TResult, SelectEnumerable<TSource, TArg, TResult, TEnumerator>.Enumerator> Take<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<TSource>
        {
            return TakeEnumerable<TResult>.From(source.GetEnumerator(), count);
        }

        public static TakeEnumerable<TResult, SelectIndexedEnumerable<TSource, TResult, TEnumerator>.Enumerator> Take<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<TSource>
        {
            return TakeEnumerable<TResult>.From(source.GetEnumerator(), count);
        }

        public static TakeEnumerable<TResult, SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator>.Enumerator> Take<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> source, int count)
            where TEnumerator : IEnumerator<TSource>
        {
            return TakeEnumerable<TResult>.From(source.GetEnumerator(), count);
        }
    }
}