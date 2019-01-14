using System.Collections.Generic;
using CodeMania.Core.Internals;
using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers.BlittableTypeArrayEqualityComparers
{
	[PublicAPI]
	public sealed unsafe class UInt16ArrayMemoryEqualityComparer : EqualityComparer<ushort[]>
	{
		public override bool Equals(ushort[] x, ushort[] y)
		{
			if (x == null) return y == null;
			if (ReferenceEquals(x, y)) return true;

			if (y != null && x.Length == y.Length)
			{
				if (x.Length == 0 && y.Length == 0) return true;

				fixed (void* xPtr = &x[0])
				fixed (void* yPtr = &y[0])
				{
					return UnsafeNativeMethods.memcmp(xPtr, yPtr, x.LongLength * sizeof(ushort)) == 0;
				}
			}

			return false;
		}

		public override int GetHashCode(ushort[] obj)
		{
			if (obj == null || obj.Length == 0) return 0;

			fixed (void* startAddr = &obj[0])
			{
				int hashCode = HashHelper.HashSeed;

				int i = 0;

				// reinterpret as int*
				int* intAddr = (int*) startAddr;

				var arrayByteSize = obj.Length * sizeof(ushort);

				if (arrayByteSize >= sizeof(int))
				{
					for (; i < arrayByteSize; i += sizeof(int))
					{
						hashCode = HashHelper.CombineHashCodes(hashCode * 397, *intAddr++);
					}
				}

				// reinterpret as pointer to source type
				ushort* ptrToRest = (ushort*) intAddr;

				// process the rest of array
				for (; i < arrayByteSize; i++)
				{
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, *ptrToRest++);
				}

				return hashCode;
			}
		}
	}
}