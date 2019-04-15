using System.Collections.Generic;
using System.Security;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.BlittableTypeArrayEqualityComparers
{
	public unsafe class BlittableTypeArrayEqualityComparerBase<T> : EqualityComparer<T[]>
		where T : unmanaged
	{
		private static readonly int HashSeed = typeof(T[]).GetHashCode();

		[SuppressUnmanagedCodeSecurity]
		public override bool Equals(T[] x, T[] y)
		{
			if (x == null) return y == null;
			if (ReferenceEquals(x, y)) return true;

			if (y != null && x.Length == y.Length)
			{
				if (x.Length == 0 && y.Length == 0) return true;

				fixed (T* xPtr = &x[0])
				fixed (T* yPtr = &y[0])
				{
					return UnsafeNativeMethods.Memcmp(xPtr, yPtr, x.LongLength * sizeof(T)) == 0;
				}
			}

			return false;
		}

		[SuppressUnmanagedCodeSecurity]
		public override int GetHashCode(T[] obj)
		{
			if (obj == null) return 0;
			if (obj.Length == 0) return HashSeed;

			fixed (T* ptr = &obj[0])
			{
				T* dataStart = ptr;
				long itemCount = obj.Length;

				long dataLength = itemCount * sizeof(T);

				const int prime = 397;
				const int blockSize = sizeof(int) * 8; // 32 bytes;

				unchecked
				{
					int hashCode = HashHelper.HashSeed;

					int* intPtr = (int*) dataStart;

					T* dataEnd = dataStart + itemCount;

					if (dataLength > blockSize)
					{
						int* blockPtr = intPtr;
						int* endAddr = (intPtr + dataLength) - dataLength % blockSize;

						do
						{
							hashCode = HashHelper.CombineHashCodes(hashCode * prime, *intPtr);
							hashCode = HashHelper.CombineHashCodes(hashCode * prime, *(intPtr + 1));
							hashCode = HashHelper.CombineHashCodes(hashCode * prime, *(intPtr + 2));
							hashCode = HashHelper.CombineHashCodes(hashCode * prime, *(intPtr + 3));
							hashCode = HashHelper.CombineHashCodes(hashCode * prime, *(intPtr + 4));
							hashCode = HashHelper.CombineHashCodes(hashCode * prime, *(intPtr + 5));
							hashCode = HashHelper.CombineHashCodes(hashCode * prime, *(intPtr + 6));
							hashCode = HashHelper.CombineHashCodes(hashCode * prime, *(intPtr + 7));

							intPtr += 8;
							blockPtr += blockSize;
						} while (blockPtr < endAddr);
					}

					// if there are rest of data which len greater than sizeof(int) then iterate through ints.
					if ((dataEnd - (T*) intPtr) / sizeof(int) > 0)
					{
						for (; intPtr < dataEnd; intPtr++)
						{
							hashCode = HashHelper.CombineHashCodes(hashCode * prime, *intPtr);
						}
					}

					byte* bytePtr = (byte*) intPtr;

					for (; bytePtr < dataEnd; bytePtr++)
					{
						hashCode = HashHelper.CombineHashCodes(hashCode * prime, *bytePtr);
					}

					return hashCode;
				}
			}
		}
	}
}