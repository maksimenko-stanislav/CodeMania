using System.Collections.Generic;
using CodeMania.Core.Internals;
using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers.BlittableTypeArrayEqualityComparers
{
	[PublicAPI]
	public sealed unsafe class Int32ArrayMemoryEqualityComparer : BlittableTypeArrayEqualityComparerBase<int>
	{
		public override bool Equals(int[] x, int[] y)
		{
			if (x == null) return y == null;
			if (ReferenceEquals(x, y)) return true;

			if (y != null && x.Length == y.Length)
			{
				if (x.Length == 0 && y.Length == 0) return true;

				fixed (void* xPtr = &x[0])
				fixed (void* yPtr = &y[0])
				{
					return UnsafeNativeMethods.memcmp(xPtr, yPtr, x.LongLength * sizeof(int)) == 0;
				}
			}

			return false;
		}

		public override int GetHashCode(int[] obj)
		{
			if (obj == null || obj.Length == 0) return 0;

			fixed (void* startAddr = &obj[0])
			{
				return GetHashCode((byte*) startAddr, sizeof(int) * obj.Length);
			}
		}
	}
}