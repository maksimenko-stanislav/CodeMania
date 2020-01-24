using System;
using System.Runtime.CompilerServices;

namespace CodeMania.FastLinq
{
    // TODO: Add Single[OrDefault] methods
    // TODO: Add Skip/Take methods
    public static partial class LinqExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckPredicate<TPredicate>(TPredicate predicate) where TPredicate : Delegate
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T CheckSource<T>(T source) where T : class =>
            source ?? throw new ArgumentNullException(nameof(source));
    }
}