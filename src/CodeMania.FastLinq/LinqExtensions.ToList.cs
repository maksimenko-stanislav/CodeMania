using System;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        #region ToList

        public static List<T> ToList<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> enumerable)
            where TEnumerator : IEnumerator<T>
        {
            return enumerable.AddToList(new List<T>());
        }

        public static List<T> AddToList<T, TEnumerator>(this WhereIndexedEnumerable<T, TEnumerator> enumerable, List<T> destination)
            where TEnumerator : IEnumerator<T>
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            foreach (var item in enumerable)
            {
                destination.Add(item);
            }

            return destination;
        }

        public static List<T> ToList<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> enumerable)
            where TEnumerator : IEnumerator<T>
        {
            return enumerable.AddToList(new List<T>());
        }

        public static List<T> AddToList<T, TArg, TEnumerator>(this WhereIndexedEnumerable<T, TArg, TEnumerator> enumerable, List<T> destination)
            where TEnumerator : IEnumerator<T>
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            foreach (var item in enumerable)
            {
                destination.Add(item);
            }

            return destination;
        }

        public static List<T> ToList<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> enumerable)
            where TEnumerator : IEnumerator<T>
        {
            return enumerable.AddToList(new List<T>());
        }

        public static List<T> AddToList<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> enumerable, List<T> destination)
            where TEnumerator : IEnumerator<T>
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            foreach (var item in enumerable)
            {
                destination.Add(item);
            }

            return destination;
        }

        public static List<T> ToList<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> enumerable)
            where TEnumerator : IEnumerator<T>
        {
            return enumerable.AddToList(new List<T>());
        }

        public static List<T> AddToList<T, TArg, TEnumerator>(this WhereEnumerable<T, TArg, TEnumerator> enumerable, List<T> destination)
            where TEnumerator : IEnumerator<T>
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            foreach (var item in enumerable)
            {
                destination.Add(item);
            }

            return destination;
        }

        public static List<TResult> ToList<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> enumerable)
            where TEnumerator : IEnumerator<TSource>
        {
            return enumerable.AddToList(new List<TResult>());
        }

        public static List<TResult> AddToList<TSource, TResult, TEnumerator>(this SelectEnumerable<TSource, TResult, TEnumerator> enumerable, List<TResult> destination)
            where TEnumerator : IEnumerator<TSource>
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            foreach (var item in enumerable)
            {
                destination.Add(item);
            }

            return destination;
        }

        public static List<TResult> ToList<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> enumerable)
            where TEnumerator : IEnumerator<TSource>
        {
            return enumerable.AddToList(new List<TResult>());
        }

        public static List<TResult> AddToList<TSource, TArg, TResult, TEnumerator>(this SelectEnumerable<TSource, TArg, TResult, TEnumerator> enumerable, List<TResult> destination)
            where TEnumerator : IEnumerator<TSource>
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            foreach (var item in enumerable)
            {
                destination.Add(item);
            }

            return destination;
        }

        public static List<TResult> ToList<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> enumerable)
            where TEnumerator : IEnumerator<TSource>
        {
            return enumerable.AddToList(new List<TResult>());
        }

        public static List<TResult> AddToList<TSource, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TResult, TEnumerator> enumerable, List<TResult> destination)
            where TEnumerator : IEnumerator<TSource>
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            foreach (var item in enumerable)
            {
                destination.Add(item);
            }

            return destination;
        }

        public static List<TResult> ToList<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> enumerable)
            where TEnumerator : IEnumerator<TSource>
        {
            return enumerable.AddToList(new List<TResult>());
        }

        public static List<TResult> AddToList<TSource, TArg, TResult, TEnumerator>(this SelectIndexedEnumerable<TSource, TArg, TResult, TEnumerator> enumerable, List<TResult> destination)
            where TEnumerator : IEnumerator<TSource>
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            foreach (var item in enumerable)
            {
                destination.Add(item);
            }

            return destination;
        }

        public static List<T> ToList<T, TEnumerator>(this SkipEnumerable<T, TEnumerator> enumerable)
            where TEnumerator : IEnumerator<T>
        {
            return enumerable.AddToList(new List<T>());
        }

        public static List<T> AddToList<T, TEnumerator>(this SkipEnumerable<T, TEnumerator> enumerable, List<T> destination)
            where TEnumerator : IEnumerator<T>
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            foreach (var item in enumerable)
            {
                destination.Add(item);
            }

            return destination;
        }

        public static List<T> ToList<T, TEnumerator>(this TakeEnumerable<T, TEnumerator> enumerable)
            where TEnumerator : IEnumerator<T>
        {
            return enumerable.AddToList(new List<T>());
        }

        public static List<T> AddToList<T, TEnumerator>(this TakeEnumerable<T, TEnumerator> enumerable, List<T> destination)
            where TEnumerator : IEnumerator<T>
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            foreach (var item in enumerable)
            {
                destination.Add(item);
            }

            return destination;
        }

        #endregion
    }
}