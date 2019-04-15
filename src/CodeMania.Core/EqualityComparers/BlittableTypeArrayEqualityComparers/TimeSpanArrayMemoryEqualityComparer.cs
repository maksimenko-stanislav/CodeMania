using System;
using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers.BlittableTypeArrayEqualityComparers
{
	[PublicAPI]
	public sealed class TimeSpanArrayMemoryEqualityComparer : BlittableTypeArrayEqualityComparerBase<TimeSpan>
	{
	}
}