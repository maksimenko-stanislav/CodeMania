using System;
using System.Collections.Generic;
using System.Diagnostics;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.BlittableTypeArrayEqualityComparers
{
	public abstract class BlittableTypeArrayEqualityComparerBase<T> : EqualityComparer<T[]>
		where T : struct
	{
		protected static unsafe int GetHashCode(byte* dataStart, long dataLength)
		{
			Debug.Assert(new IntPtr(dataStart) != IntPtr.Zero, "new IntPtr(dataStart) != IntPtr.Zero");
			Debug.Assert(dataLength > 0, "dataLength > 0");

			const int prime = 397;

			unchecked
			{
				int hashCode = HashHelper.HashSeed;

				int* intPtr = (int*) dataStart;

				int blockSize = sizeof(int) * 8; // 32 bytes;

				byte* dataEnd = dataStart + dataLength;

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
				if ((dataEnd - (byte*) intPtr) / sizeof(int) > 0)
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