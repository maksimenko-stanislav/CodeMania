using System.Collections.Generic;
using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers
{
	public sealed class ListEqualityComparer<T> : EnumerableComparerBase<T, List<T>>
	{
		[UsedImplicitly]
		public static ListEqualityComparer<T> Instance { get; } = new ListEqualityComparer<T>();

		public override bool Equals(List<T> x, List<T> y)
		{
			if (x == null) return y == null;
			if (ReferenceEquals(x, y)) return true;

			if (y != null)
			{
				if (x.Count != y.Count) return false;

				for (var i = 0; i < x.Count; i++)
				{
					if (!AreEqualsFunc(x[i], y[i])) return false;
				}

				return true;
			}

			return false;
		}
	}
}