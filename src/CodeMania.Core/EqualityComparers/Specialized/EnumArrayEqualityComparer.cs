using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CodeMania.Core.EqualityComparers.Specialized
{
	public sealed class EnumArrayEqualityComparer<T> : IEqualityComparer<T[]>
		where T : struct, Enum
	{
		private static readonly int HashSeed = typeof(T[]).GetHashCode();
		private static readonly int EnumValueByteSize = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));

		public static readonly EnumArrayEqualityComparer<T> Default = new EnumArrayEqualityComparer<T>();

		public bool Equals(T[] x, T[] y)
		{
			if (x == null) return y == null;
			if (ReferenceEquals(x, y)) return true;

			if (y != null && x.Length == y.Length)
			{
				if (x.Length == 0 && y.Length == 0) return true;

				ReadOnlySpan<T> xT = x;
				ReadOnlySpan<T> yT = y;
				ReadOnlySpan<byte> xSpan = MemoryMarshal.Cast<T, byte>(xT);
				ReadOnlySpan<byte> ySpan = MemoryMarshal.Cast<T, byte>(yT);

				return xSpan.SequenceEqual(ySpan);
			}

			return false;
		}

		public unsafe int GetHashCode(T[] obj)
		{
			if (obj == null) return 0;
			if (obj.Length == 0) return HashSeed;

			ReadOnlySpan<byte> objSpan = MemoryMarshal.Cast<T, byte>(obj);
			fixed (byte* objPtr = &MemoryMarshal.GetReference(objSpan))
			{
				return EqualityComparisonHelper.GetHashCode(objPtr, obj.Length * EnumValueByteSize);
			}
		}
	}
}