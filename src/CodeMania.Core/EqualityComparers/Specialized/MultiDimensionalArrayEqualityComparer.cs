using System;
using System.Collections.Generic;

namespace CodeMania.Core.EqualityComparers.Specialized
{
	public class MultiDimensionalArrayEqualityComparer : IEqualityComparer<Array>
	{
		public static readonly MultiDimensionalArrayEqualityComparer Default = new MultiDimensionalArrayEqualityComparer();

		public bool Equals(Array x, Array y)
		{
			if (x == null) return y == null;
			if (ReferenceEquals(x, y)) return true;

			if (y != null && x.Rank == y.Rank && x.LongLength == y.LongLength && AreDimensionsEquals(x, y))
			{
				return EqualsCore(x, y);
			}

			return false;
		}

		private bool AreDimensionsEquals(Array x, Array y)
		{
			for (int i = 0; i < x.Rank; i++)
			{
				if (x.GetLowerBound(i) != y.GetLowerBound(i) || x.GetUpperBound(i) != y.GetUpperBound(i))
					return false;
			}

			return true;
		}

		protected virtual bool EqualsCore(Array x, Array y)
		{
			throw new NotImplementedException();
		}

		public int GetHashCode(Array obj)
		{
			throw new NotImplementedException();
		}
	}
}