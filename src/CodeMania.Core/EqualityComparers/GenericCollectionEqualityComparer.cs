using System.Collections.Generic;
using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers
{
	public sealed class GenericCollectionEqualityComparer<T> : EnumerableComparerBase<T, IEnumerable<T>>
	{
		[UsedImplicitly]
		public static GenericCollectionEqualityComparer<T> Instance { get; } = new GenericCollectionEqualityComparer<T>();

		public override bool Equals(IEnumerable<T> x, IEnumerable<T> y)
		{
			if (x == null) return y == null;
			if (ReferenceEquals(x, y)) return true;

			if (y != null)
			{
				if (x.GetType() != y.GetType()) return false;

				if (x is ICollection<T> xCollection && y is ICollection<T> yCollection && xCollection.Count != yCollection.Count)
				{
					return false;
				}

				if (x is IReadOnlyCollection<T> xReadOnlyCollection && y is IReadOnlyCollection<T> yReadOnlyCollection && xReadOnlyCollection.Count != yReadOnlyCollection.Count)
				{
					return false;
				}

				using (var xEnumerator = x.GetEnumerator())
				using (var yEnumerator = y.GetEnumerator())
				{
					while (true)
					{
						var canMoveX = xEnumerator.MoveNext();
						var canMoveY = yEnumerator.MoveNext();

						if (canMoveX != canMoveY) return false;

						if (!canMoveX)
						{
							break;
						}

						if (!AreEqualsFunc(xEnumerator.Current, yEnumerator.Current))
						{
							return false;
						}
					}
				}

				return true;
			}

			return false;
		}
	}
}