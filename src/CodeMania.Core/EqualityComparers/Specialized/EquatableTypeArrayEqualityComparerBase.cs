using System;
using System.Collections.Generic;

namespace CodeMania.Core.EqualityComparers.Specialized
{
	public abstract class EquatableTypeArrayEqualityComparerBase<T> : IEqualityComparer<T[]>
		where T : IEquatable<T>
	{
		protected static readonly int HashSeed = typeof(T[]).GetHashCode();

		public virtual bool Equals(T[] x, T[] y)
		{
			if (x == null) return y == null;
			if (ReferenceEquals(x, y)) return true;

			if (y != null && x.Length == y.Length)
			{
				if (x.Length == 0 && y.Length == 0) return true;

				ReadOnlySpan<T> xSpan = x;
				ReadOnlySpan<T> ySpan = y;

				return xSpan.SequenceEqual(ySpan);
			}

			return false;
		}

		public abstract int GetHashCode(T[] obj);
	}
}