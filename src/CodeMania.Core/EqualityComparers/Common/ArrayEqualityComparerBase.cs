using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.Common
{
	public abstract class ArrayEqualityComparerBase<T> : CollectionEqualityComparerBase<T, T[]>
	{
		protected override bool EqualsCore(T[] x, T[] y)
		{
			int i = 0;

			for (; i < x.Length - x.Length % 4; i += 4)
			{
				if (!AreEquals(x[i + 0], y[i + 0]) ||
				    !AreEquals(x[i + 1], y[i + 1]) ||
				    !AreEquals(x[i + 2], y[i + 2]) ||
				    !AreEquals(x[i + 3], y[i + 3]))
				{
					return false;
				}
			}

			for (; i < x.Length; i++)
			{
				if (!AreEquals(x[i], y[i]))
				{
					return false;
				}
			}

			return true;
		}

		protected override int GetHashCodeCore(T[] obj)
		{
			if (obj.Length == 0) return EmptyCollectionHashCode;

			int hashCode = HashHelper.HashSeed;

			unchecked
			{
				int i;
				for (i = 0; i < obj.Length - obj.Length % 4; i += 4)
				{
					hashCode = CalcCombinedHashCode(obj[i + 0], hashCode);
					hashCode = CalcCombinedHashCode(obj[i + 1], hashCode);
					hashCode = CalcCombinedHashCode(obj[i + 2], hashCode);
					hashCode = CalcCombinedHashCode(obj[i + 3], hashCode);
				}

				for (i = 0; i < obj.Length; i++)
				{
					hashCode = CalcCombinedHashCode(obj[i], hashCode);
				}
			}

			return hashCode;
		}
	}
}