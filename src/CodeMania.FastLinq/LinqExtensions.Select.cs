using System;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        #region Select

        // non-indexed

        public static SelectEnumerable<TSource, TArg, TResult, IEnumerator<TSource>> Select<TSource, TArg, TResult>(
            this IEnumerable<TSource> source,
            TArg arg,
            Selector<TSource, TArg, TResult> selector)
        {
            return SelectEnumerable.From(CheckSource(source).GetEnumerator(), arg, selector);
        }

        public static SelectEnumerable<TSource, TArg, TResult, List<TSource>.Enumerator> Select<TSource, TArg, TResult>(
            this List<TSource> source,
            TArg arg,
            Selector<TSource, TArg, TResult> selector)
        {
            return SelectEnumerable.From(CheckSource(source).GetEnumerator(), arg, selector);
        }

        public static SelectEnumerable<TSource, TArg, TResult, ArrayEnumerable<TSource>.Enumerator> Select<TSource, TArg, TResult>(
            this TSource[] source,
            TArg arg,
            Selector<TSource, TArg, TResult> selector)
        {
            return SelectEnumerable.From(new ArrayEnumerable<TSource>(CheckSource(source)).GetEnumerator(), arg, selector);
        }

        // indexed

        public static SelectIndexedEnumerable<TSource, TArg, TResult, IEnumerator<TSource>> Select<TSource, TArg, TResult>(
            this IEnumerable<TSource> source,
            TArg arg,
            IndexedSelector<TSource, TArg, TResult> selector)
        {
            return SelectIndexedEnumerable.From(CheckSource(source).GetEnumerator(), arg, selector);
        }

        public static SelectIndexedEnumerable<TSource, TArg, TResult, List<TSource>.Enumerator> Select<TSource, TArg, TResult>(
            this List<TSource> source,
            TArg arg,
            IndexedSelector<TSource, TArg, TResult> selector)
        {
            return SelectIndexedEnumerable.From(CheckSource(source).GetEnumerator(), arg, selector);
        }

        public static SelectIndexedEnumerable<TSource, TArg, TResult, ArrayEnumerable<TSource>.Enumerator> Select<TSource, TArg, TResult>(
            this TSource[] source,
            TArg arg,
            IndexedSelector<TSource, TArg, TResult> selector)
        {
            return SelectIndexedEnumerable.From(new ArrayEnumerable<TSource>(CheckSource(source)).GetEnumerator(), arg, selector);
        }

        // optimized

        // select -> select

        public static SelectEnumerable<TSrcResult, TArg, TResult, SelectEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TSrcArg, TArg, TSrcResult, TResult, TSrcEnumerator>(
            this SelectEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator> source,
            TArg arg,
            Selector<TSrcResult, TArg, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectEnumerable<TSrcResult, TArg, TResult, SelectIndexedEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TSrcArg, TArg, TSrcResult, TResult, TSrcEnumerator>(
            this SelectIndexedEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator> source,
            TArg arg,
            Selector<TSrcResult, TArg, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectEnumerable<TSrcResult, TArg, TResult, SelectEnumerable<TSource, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TArg, TSrcResult, TResult, TSrcEnumerator>(
            this SelectEnumerable<TSource, TSrcResult, TSrcEnumerator> source,
            TArg arg,
            Selector<TSrcResult, TArg, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectEnumerable<TSrcResult, TArg, TResult, SelectIndexedEnumerable<TSource, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TArg, TSrcResult, TResult, TSrcEnumerator>(
            this SelectIndexedEnumerable<TSource, TSrcResult, TSrcEnumerator> source,
            TArg arg,
            Selector<TSrcResult, TArg, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectEnumerable<TSrcResult, TResult, SelectEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TSrcArg, TSrcResult, TResult, TSrcEnumerator>(
            this SelectEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator> source,
            Func<TSrcResult, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectEnumerable<TSrcResult, TResult, SelectIndexedEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TSrcArg, TSrcResult, TResult, TSrcEnumerator>(
            this SelectIndexedEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator> source,
            Func<TSrcResult, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectEnumerable<TSrcResult, TResult, SelectEnumerable<TSource, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TSrcResult, TResult, TSrcEnumerator>(
            this SelectEnumerable<TSource, TSrcResult, TSrcEnumerator> source,
            Func<TSrcResult, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectEnumerable<TSrcResult, TResult, SelectIndexedEnumerable<TSource, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TSrcResult, TResult, TSrcEnumerator>(
            this SelectIndexedEnumerable<TSource, TSrcResult, TSrcEnumerator> source,
            Func<TSrcResult, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSrcResult, TArg, TResult, SelectEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TSrcArg, TArg, TSrcResult, TResult, TSrcEnumerator>(
            this SelectEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator> source,
            TArg arg,
            IndexedSelector<TSrcResult, TArg, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectIndexedEnumerable<TSrcResult, TArg, TResult, SelectIndexedEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TSrcArg, TArg, TSrcResult, TResult, TSrcEnumerator>(
            this SelectIndexedEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator> source,
            TArg arg,
            IndexedSelector<TSrcResult, TArg, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectIndexedEnumerable<TSrcResult, TArg, TResult, SelectEnumerable<TSource, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TArg, TSrcResult, TResult, TSrcEnumerator>(
            this SelectEnumerable<TSource, TSrcResult, TSrcEnumerator> source,
            TArg arg,
            IndexedSelector<TSrcResult, TArg, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectIndexedEnumerable<TSrcResult, TArg, TResult, SelectIndexedEnumerable<TSource, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TArg, TSrcResult, TResult, TSrcEnumerator>(
            this SelectIndexedEnumerable<TSource, TSrcResult, TSrcEnumerator> source,
            TArg arg,
            IndexedSelector<TSrcResult, TArg, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectIndexedEnumerable<TSrcResult, TResult, SelectEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TSrcArg, TSrcResult, TResult, TSrcEnumerator>(
            this SelectEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator> source,
            Func<TSrcResult, int, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSrcResult, TResult, SelectIndexedEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TSrcArg, TSrcResult, TResult, TSrcEnumerator>(
            this SelectIndexedEnumerable<TSource, TSrcArg, TSrcResult, TSrcEnumerator> source,
            Func<TSrcResult, int, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSrcResult, TResult, SelectEnumerable<TSource, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TSrcResult, TResult, TSrcEnumerator>(
            this SelectEnumerable<TSource, TSrcResult, TSrcEnumerator> source,
            Func<TSrcResult, int, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSrcResult, TResult, SelectIndexedEnumerable<TSource, TSrcResult, TSrcEnumerator>.Enumerator> Select<TSource, TSrcResult, TResult, TSrcEnumerator>(
            this SelectIndexedEnumerable<TSource, TSrcResult, TSrcEnumerator> source,
            Func<TSrcResult, int, TResult> selector)
            where TSrcEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), selector);
        }

        // where -> select

        public static SelectEnumerable<TSource, TArg, TResult, WhereEnumerable<TSource, TArg, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereEnumerable<TSource, TArg, TEnumerator> source,
            TArg arg,
            Selector<TSource, TArg, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectEnumerable<TSource, TArg, TResult, WhereIndexedEnumerable<TSource, TArg, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereIndexedEnumerable<TSource, TArg, TEnumerator> source,
            TArg arg,
            Selector<TSource, TArg, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectEnumerable<TSource, TArg, TResult, WhereEnumerable<TSource, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereEnumerable<TSource, TEnumerator> source,
            TArg arg,
            Selector<TSource, TArg, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectEnumerable<TSource, TArg, TResult, WhereIndexedEnumerable<TSource, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereIndexedEnumerable<TSource, TEnumerator> source,
            TArg arg,
            Selector<TSource, TArg, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectEnumerable<TSource, TResult, WhereEnumerable<TSource, TArg, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereEnumerable<TSource, TArg, TEnumerator> source,
            Func<TSource, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectEnumerable<TSource, TResult, WhereIndexedEnumerable<TSource, TArg, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereIndexedEnumerable<TSource, TArg, TEnumerator> source,
            Func<TSource, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectEnumerable<TSource, TResult, WhereEnumerable<TSource, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereEnumerable<TSource, TEnumerator> source,
            Func<TSource, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectEnumerable<TSource, TResult, WhereIndexedEnumerable<TSource, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereIndexedEnumerable<TSource, TEnumerator> source,
            Func<TSource, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSource, TArg, TResult, WhereEnumerable<TSource, TArg, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereEnumerable<TSource, TArg, TEnumerator> source,
            TArg arg,
            IndexedSelector<TSource, TArg, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectIndexedEnumerable<TSource, TArg, TResult, WhereIndexedEnumerable<TSource, TArg, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereIndexedEnumerable<TSource, TArg, TEnumerator> source,
            TArg arg,
            IndexedSelector<TSource, TArg, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectIndexedEnumerable<TSource, TArg, TResult, WhereEnumerable<TSource, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereEnumerable<TSource, TEnumerator> source,
            TArg arg,
            IndexedSelector<TSource, TArg, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectIndexedEnumerable<TSource, TArg, TResult, WhereIndexedEnumerable<TSource, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereIndexedEnumerable<TSource, TEnumerator> source,
            TArg arg,
            IndexedSelector<TSource, TArg, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), arg, selector);
        }

        public static SelectIndexedEnumerable<TSource, TResult, WhereEnumerable<TSource, TArg, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereEnumerable<TSource, TArg, TEnumerator> source,
            Func<TSource, int, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSource, TResult, WhereIndexedEnumerable<TSource, TArg, TEnumerator>.Enumerator> Select<TSource, TArg, TResult, TEnumerator>(
            this WhereIndexedEnumerable<TSource, TArg, TEnumerator> source,
            Func<TSource, int, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSource, TResult, WhereEnumerable<TSource, TEnumerator>.Enumerator> Select<TSource, TResult, TEnumerator>(
            this WhereEnumerable<TSource, TEnumerator> source,
            Func<TSource, int, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectIndexedEnumerable<TSource, TResult, WhereIndexedEnumerable<TSource, TEnumerator>.Enumerator> Select<TSource, TResult, TEnumerator>(
            this WhereIndexedEnumerable<TSource, TEnumerator> source,
            Func<TSource, int, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectIndexedEnumerable.From(source.GetEnumerator(), selector);
        }

        public static SelectEnumerable<TSource, TResult, WhereEnumerable<TSource, TEnumerator>.Enumerator> Select<TSource, TResult, TEnumerator>(
            this WhereEnumerable<TSource, TEnumerator> source,
            Func<TSource, TResult> selector)
            where TEnumerator : IEnumerator<TSource>
        {
            return SelectEnumerable.From(source.GetEnumerator(), selector);
        }

        #endregion

        // TODO: Add Skip/Take support
    }
}