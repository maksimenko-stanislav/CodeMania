using System.Collections.Generic;
using System.Security;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers
{
	internal static class EqualityComparisonHelper
	{
		public static bool AreEquals<T>(T? x, T? y, IEqualityComparer<T?> comparer) where T : struct => comparer.Equals(x, y);

		public static int CalcCombinedHashCode<T>(T? element, int hashCode, IEqualityComparer<T?> comparer) where T : struct =>
			element != null
				? HashHelper.CombineHashCodes(hashCode * 397, comparer.GetHashCode(element))
				: ~(hashCode ^ HashHelper.HashSeed);

		public static bool AreEquals<T>(T x, T y, IEqualityComparer<T> comparer) where T : struct => comparer.Equals(x, y);

		public static int CalcCombinedHashCode<T>(T element, int hashCode, IEqualityComparer<T> comparer) where T : struct =>
			HashHelper.CombineHashCodes(hashCode * 397, comparer.GetHashCode(element));

		[SuppressUnmanagedCodeSecurity]
		internal static unsafe int GetHashCode(void* ptr, long dataLength)
		{
			byte* dataStart = (byte*) ptr;

			const int prime = 397;
			const int blockSize = sizeof(int) * 8; // 32 bytes;

			unchecked
			{
				int hashCode = HashHelper.HashSeed;

				int* intPtr = (int*) dataStart;

				byte* dataEnd = dataStart + dataLength;

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