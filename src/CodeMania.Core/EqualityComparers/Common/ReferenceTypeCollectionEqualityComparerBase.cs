using System.Collections.Generic;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.Common
{
	public abstract class ReferenceTypeCollectionEqualityComparerBase<T, TCollection> : CollectionEqualityComparerBase<T, TCollection>, IReferenceTypeEqualityComparer<TCollection>
		where T : class
		where TCollection : class, IEnumerable<T>
	{
		protected override bool EqualsCore(TCollection x, TCollection y)
		{
			EqualityComparerContext context = EqualityComparerContext.Current;
			bool isAcquired = false;

			if (!context.IsAcquired)
			{
				context.IsAcquired = isAcquired = true;
				context.Free();
			}

			try
			{
				return Equals(x, y, context);
			}
			finally
			{
				if (isAcquired)
				{
					context.IsAcquired = false;
					context.Free();
				}
			}
		}

		protected override int GetHashCodeCore(TCollection obj)
		{
			EqualityComparerContext context = EqualityComparerContext.Current;
			bool isAcquired = false;

			if (!context.IsAcquired)
			{
				context.IsAcquired = isAcquired = true;
				context.Free();
			}

			try
			{
				return GetHashCode(obj, context);
			}
			finally
			{
				if (isAcquired)
				{
					context.IsAcquired = false;
					context.Free();
				}
			}
		}

		public virtual bool Equals(TCollection x, TCollection y, EqualityComparerContext context)
		{
			if (x == null) return y == null;
			if (ReferenceEquals(x, y)) return true;
			if (x.GetType() != y.GetType()) return false;

			return EqualsCore(x, y, context);
		}

		public virtual int GetHashCode(TCollection obj, EqualityComparerContext context)
		{
			if (obj == null) return 0;

			return GetHashCodeCore(obj, context);
		}

		protected abstract bool EqualsCore(TCollection x, TCollection y, EqualityComparerContext context);
		protected abstract int GetHashCodeCore(TCollection obj, EqualityComparerContext context);

		protected override bool AreEquals(T x, T y) => AreEquals(EqualityComparerContext.Current, x, y);

		protected override int CalcCombinedHashCode(T element, int hashCode) => CalcCombinedHashCode(EqualityComparerContext.Current, element, hashCode);

		protected virtual int CalcCombinedHashCode(EqualityComparerContext context, T element, int hashCode) =>
			HashHelper.CombineHashCodes(
				hashCode * 397,
				element != null
					? context.TryAdd(element)
						? ElementEqualityComparer.GetHashCode(element)
						: ~hashCode
					: ~(hashCode ^ HashHelper.HashSeed));

		protected virtual bool AreEquals(EqualityComparerContext context, T x, T y) =>
			(!context.TryAdd(x) | !context.TryAdd(y)) || ElementEqualityComparer.Equals(x, y);
	}
}