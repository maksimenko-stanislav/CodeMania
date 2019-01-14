using System;
using System.Collections.Generic;
using CodeMania.Core.Internals;
using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers.BlittableTypeArrayEqualityComparers
{
	[PublicAPI]
	public sealed unsafe class DateTimeArrayMemoryEqualityComparer : EqualityComparer<DateTime[]>
	{
		public override bool Equals(DateTime[] x, DateTime[] y)
		{
			if (x == null) return y == null;
			if (ReferenceEquals(x, y)) return true;

			if (y != null && x.Length == y.Length)
			{
				if (x.Length == 0 && y.Length == 0) return true;

				fixed (void* xPtr = &x[0])
				fixed (void* yPtr = &y[0])
				{
					return UnsafeNativeMethods.memcmp(xPtr, yPtr, x.LongLength * sizeof(DateTime)) == 0;
				}
			}

			return false;
		}

		public override int GetHashCode(DateTime[] obj)
		{
			if (obj == null || obj.Length == 0) return 0;

			fixed (void* startAddr = &obj[0])
			{
				int hashCode = HashHelper.HashSeed;

				int i = 0;

				// reinterpret as int*
				int* intAddr = (int*) startAddr;

				var arrayByteSize = obj.Length * sizeof(DateTime);

				for (; i < arrayByteSize; i += sizeof(int))
				{
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, *intAddr++);
				}

				// skip the rest because we know that sizeof(DateTime) == 8

				return hashCode;
			}
		}
	}
}