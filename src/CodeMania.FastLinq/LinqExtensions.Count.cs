using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        public static int Count<T, TEnumerator>(this WhereEnumerable<T, TEnumerator> source)
            where TEnumerator : IEnumerator<T>
        {
            int i = 0;

            foreach (var _ in source)
            {
                i++;
            }

            return i;
        }

        // TODO: add other methods
        // TODO: Add Skip/Take support
    }
}