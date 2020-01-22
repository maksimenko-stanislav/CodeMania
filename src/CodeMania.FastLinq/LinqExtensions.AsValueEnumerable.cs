using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        public static SelectEnumerable<T, T, List<T>.Enumerator> AsValueEnumerable<T, TEnumerator>(this List<T> source)
            where TEnumerator : IEnumerator<T>
        {
            return new SelectEnumerable<T, T, List<T>.Enumerator>(source.GetEnumerator(), x => x);
        }
    }
}