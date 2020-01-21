using System.Collections.Generic;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.Common.ReferenceType
{
	public sealed class ReferenceTypeListEqualityComparer<T> : ReferenceTypeCollectionEqualityComparerBase<T, List<T>>
		where T : class
	{
		public static ReferenceTypeListEqualityComparer<T> Default = new ReferenceTypeListEqualityComparer<T>();

		protected override bool EqualsCore(List<T> x, List<T> y, EqualityComparerContext context)
		{
			if (x.Count != y.Count) return false;

			int i = 0;

			for (; i < x.Count - x.Count % 4; i += 4)
			{
				if (!AreEquals(context, x[i + 0], y[i + 0]) ||
				    !AreEquals(context, x[i + 1], y[i + 1]) ||
				    !AreEquals(context, x[i + 2], y[i + 2]) ||
				    !AreEquals(context, x[i + 3], y[i + 3]))
				{
					return false;
				}
			}

			for (; i < x.Count; i++)
			{
				if (!AreEquals(context, x[i], y[i]))
				{
					return false;
				}
			}

			return true;
		}

		protected override int GetHashCodeCore(List<T> obj, EqualityComparerContext context)
		{
			if (obj.Count == 0) return EmptyCollectionHashCode;

			int hashCode = HashHelper.HashSeed;

			unchecked
			{
				int i;
				for (i = 0; i < obj.Count - obj.Count % 4; i += 4)
				{
					hashCode = CalcCombinedHashCode(context, obj[i + 0], hashCode);
					hashCode = CalcCombinedHashCode(context, obj[i + 1], hashCode);
					hashCode = CalcCombinedHashCode(context, obj[i + 2], hashCode);
					hashCode = CalcCombinedHashCode(context, obj[i + 3], hashCode);
				}

				for (i = 0; i < obj.Count; i++)
				{
					hashCode = CalcCombinedHashCode(context, obj[i], hashCode);
				}
			}

			return hashCode;
		}
	}
}