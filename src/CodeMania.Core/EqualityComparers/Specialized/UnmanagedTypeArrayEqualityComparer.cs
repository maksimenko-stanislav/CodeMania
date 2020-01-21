using System;
using System.Security;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.Specialized
{
	public sealed class UnmanagedTypeArrayEqualityComparer<T> : EquatableTypeArrayEqualityComparerBase<T>
		where T : unmanaged, IEquatable<T>
	{
		public static readonly UnmanagedTypeArrayEqualityComparer<T> Default = new UnmanagedTypeArrayEqualityComparer<T>();

		[SuppressUnmanagedCodeSecurity]
		public override unsafe int GetHashCode(T[] obj)
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
							hashCode = HashHelper.CombineHashCodes(hashCode * prime, *(intPtr + 0));
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
					for (; intPtr < dataEnd; intPtr++)
					{
						hashCode = HashHelper.CombineHashCodes(hashCode * prime, *intPtr);
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