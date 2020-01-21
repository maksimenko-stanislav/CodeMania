using System.Collections.Generic;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.Common
{
	public abstract class ListEqualityComparerBase<T> : CollectionEqualityComparerBase<T, List<T>>
	{
		protected override bool EqualsCore(List<T> x, List<T> y)
		{
			int i = 0;

			for (; i < x.Count - x.Count % 4; i += 4)
			{
				if (!AreEquals(x[i + 0], y[i + 0]) ||
				    !AreEquals(x[i + 1], y[i + 1]) ||
				    !AreEquals(x[i + 2], y[i + 2]) ||
				    !AreEquals(x[i + 3], y[i + 3]))
				{
					return false;
				}
			}

			for (; i < x.Count; i++)
			{
				if (!AreEquals(x[i], y[i]))
				{
					return false;
				}
			}

			return true;
		}

		protected override int GetHashCodeCore(List<T> obj)
		{
			if (obj.Count == 0) return EmptyCollectionHashCode;

			int hashCode = HashHelper.HashSeed;

			unchecked
			{
				int i;
				for (i = 0; i < obj.Count - obj.Count % 4; i += 4)
				{
					hashCode = CalcCombinedHashCode(obj[i + 0], hashCode);
					hashCode = CalcCombinedHashCode(obj[i + 1], hashCode);
					hashCode = CalcCombinedHashCode(obj[i + 2], hashCode);
					hashCode = CalcCombinedHashCode(obj[i + 3], hashCode);
				}

				for (i = 0; i < obj.Count; i++)
				{
					hashCode = CalcCombinedHashCode(obj[i], hashCode);
				}
			}

			return hashCode;
		}
	}
}