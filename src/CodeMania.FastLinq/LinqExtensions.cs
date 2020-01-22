using System;
using System.Runtime.CompilerServices;

namespace CodeMania.FastLinq
{
    public static partial class LinqExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TPredicate CheckPredicate<TPredicate>(TPredicate predicate) where TPredicate : Delegate =>
            predicate ?? throw new ArgumentNullException(nameof(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TPredicate CheckSelector<TPredicate>(TPredicate predicate) where TPredicate : Delegate =>
            predicate ?? throw new ArgumentNullException(nameof(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T CheckSource<T>(T source) where T : class =>
            source ?? throw new ArgumentNullException(nameof(source));
    }
}