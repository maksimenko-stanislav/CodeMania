using System.Collections.Generic;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.Common
{
	public abstract class EnumerableEqualityComparerBase<T> : CollectionEqualityComparerBase<T, IEnumerable<T>>
	{
		protected override bool EqualsCore(IEnumerable<T> x, IEnumerable<T> y)
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

						var xValue = xEnumerator.Current;
						var yValue = yEnumerator.Current;

						if (!AreEquals(xValue, yValue))
						{
							return false;
						}
					}
				}

				return true;
			}

			return false;
		}

		protected override int GetHashCodeCore(IEnumerable<T> obj)
		{
			unchecked
			{
				int hashCode = HashHelper.HashSeed;

				foreach (var element in obj)
				{
					hashCode = CalcCombinedHashCode(element, hashCode);
				}

				return hashCode;
			}
		}
	}
}