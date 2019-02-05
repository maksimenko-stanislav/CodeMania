using System;
using System.Runtime.InteropServices;

namespace CodeMania.Core.Internals
{
	public static unsafe class UnsafeNativeMethods
	{
        // TODO Write custom dll loader based on OS Runtime

		[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int memcmp(void* b1, void* b2, long count);

        public static unsafe int Memcmp(IntPtr b1, IntPtr b2, long count)
        {
            if (b1 == IntPtr.Zero)
                throw new ArgumentException("Zero pointer.", nameof(b1));
            if (b2 == IntPtr.Zero)
                throw new ArgumentException("Zero pointer.", nameof(b2));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return memcmp(b1.ToPointer(), b2.ToPointer(), count);
        }
    }
}