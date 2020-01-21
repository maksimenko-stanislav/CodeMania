using System;
using System.Collections.Generic;
using CodeMania.Core.Internals;
using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers.Common
{
	public abstract class CollectionEqualityComparerBase<T, TCollection> : IEqualityComparer<TCollection>
		where TCollection : IEnumerable<T>
	{
		protected static readonly Func<T, T, bool> AreEqualsFunc;
		protected static readonly Func<T, int> GetHashCodeFunc;

		protected static readonly int EmptyCollectionHashCode =
			HashHelper.CombineHashCodes(typeof(T).GetHashCode(), typeof(TCollection).GetHashCode());

		static CollectionEqualityComparerBase()
		{
			AreEqualsFunc = EqualityComparerFuncCache<T>.EqualsFunc;
			GetHashCodeFunc = EqualityComparerFuncCache<T>.GetHashCodeFunc;
		}

		protected readonly IEqualityComparer<T> ElementEqualityComparer;

		[UsedImplicitly]
		protected CollectionEqualityComparerBase() : this(null)
		{
		}

		protected CollectionEqualityComparerBase([CanBeNull] IEqualityComparer<T> elementEqualityComparer)
		{
			ElementEqualityComparer = elementEqualityComparer ?? FuncEqualityComparer<T>.Create(AreEqualsFunc, GetHashCodeFunc);
		}

		protected abstract bool EqualsCore(TCollection x, TCollection y);
		protected abstract int GetHashCodeCore(TCollection obj);

		public virtual bool Equals(TCollection x, TCollection y)
		{
			if (x != null)
			{
				if (y != null)
				{
					if (ReferenceEquals(x, y)) return true;
					if (x.GetType() != y.GetType()) return false;

					return EqualsCore(x, y);
				}

				return false;
			}

			return y == null;
		}

		// ReSharper disable once ConditionIsAlwaysTrueOrFalse
		public virtual int GetHashCode(TCollection obj) => obj == null ? 0 : GetHashCodeCore(obj);

		protected abstract int CalcCombinedHashCode(T element, int hashCode);

		protected abstract bool AreEquals(T x, T y);
	}
}