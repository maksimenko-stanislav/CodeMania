using System.Collections.Generic;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.Common.ReferenceType
{
	public sealed class ReferenceTypeCollectionEqualityComparer<T> : ReferenceTypeCollectionEqualityComparerBase<T, IEnumerable<T>>
		where T : class
	{
		public static ReferenceTypeCollectionEqualityComparer<T> Default = new ReferenceTypeCollectionEqualityComparer<T>();

		protected override bool EqualsCore(IEnumerable<T> x, IEnumerable<T> y, EqualityComparerContext context)
		{
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

					if (!AreEquals(context, xValue, yValue))
					{
						return false;
					}
				}
			}

			return true;
		}

		protected override int GetHashCodeCore(IEnumerable<T> obj, EqualityComparerContext context)
		{
			unchecked
			{
				int hashCode = HashHelper.HashSeed;

				foreach (var element in obj)
				{
					hashCode = CalcCombinedHashCode(context, element, hashCode);
				}

				return hashCode;
			}
		}
	}
}