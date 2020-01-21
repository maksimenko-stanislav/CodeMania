using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.Common.ReferenceType
{
	public sealed class ReferenceTypeArrayEqualityComparer<T> : ReferenceTypeCollectionEqualityComparerBase<T, T[]>
		where T : class
	{
		public static ReferenceTypeArrayEqualityComparer<T> Default = new ReferenceTypeArrayEqualityComparer<T>();

		protected override bool EqualsCore(T[] x, T[] y, EqualityComparerContext context)
		{
			if (x.Length != y.Length) return false;

			int i = 0;

			for (; i < x.Length - x.Length % 4; i += 4)
			{
				if (!AreEquals(context, x[i + 0], y[i + 0]) ||
				    !AreEquals(context, x[i + 1], y[i + 1]) ||
				    !AreEquals(context, x[i + 2], y[i + 2]) ||
				    !AreEquals(context, x[i + 3], y[i + 3]))
				{
					return false;
				}
			}

			for (; i < x.Length; i++)
			{
				if (!AreEquals(context, x[i], y[i]))
				{
					return false;
				}
			}

			return true;
		}

		protected override int GetHashCodeCore(T[] obj, EqualityComparerContext context)
		{
			if (obj.Length == 0) return EmptyCollectionHashCode;

			int hashCode = HashHelper.HashSeed;

			unchecked
			{
				int i;
				for (i = 0; i < obj.Length - obj.Length % 4; i += 4)
				{
					hashCode = CalcCombinedHashCode(context, obj[i + 0], hashCode);
					hashCode = CalcCombinedHashCode(context, obj[i + 1], hashCode);
					hashCode = CalcCombinedHashCode(context, obj[i + 2], hashCode);
					hashCode = CalcCombinedHashCode(context, obj[i + 3], hashCode);
				}

				for (i = 0; i < obj.Length; i++)
				{
					hashCode = CalcCombinedHashCode(context, obj[i], hashCode);
				}
			}

			return hashCode;
		}
	}
}