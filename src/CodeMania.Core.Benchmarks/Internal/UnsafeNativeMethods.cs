using System;
using System.Runtime.InteropServices;

namespace CodeMania.Core.Benchmarks.Internal
{
	public static unsafe class UnsafeNativeMethods
	{
		private delegate int MemCmpDelegate(void* b1, void* b2, ulong count);
		private delegate void* MemCpyDelegate(void* b1, void* b2, ulong count);

		private static readonly MemCmpDelegate MemcmpFunc;
		private static readonly MemCpyDelegate MemcpyFunc;

		static UnsafeNativeMethods()
		{
#if NETSTANDARD || NETCOREAPP
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				MemcmpFunc = memcmp_linux;
				MemcpyFunc = memcpy_linux;
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				MemcmpFunc = memcmp_osx;
				MemcpyFunc = memcpy_osx;
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				MemcmpFunc = memcmp_win;
				MemcpyFunc = memcpy_win;
			}
#else
			MemcmpFunc = memcmp_win;
			MemcpyFunc = memcpy_win;
#endif
		}

#region memcpy

		internal static void Memcpy(void* b1, void* b2, long count) => MemcpyFunc(b1, b2, (ulong) count);

		[DllImport("msvcrt", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
		public static extern void* memcpy_win(void* dest, void* src, ulong count);

#if NETSTANDARD || NETCOREAPP
		[DllImport("libc", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
		public static extern void* memcpy_linux(void* dest, void* src, ulong count);

		[DllImport("libSystem", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
		public static extern void* memcpy_osx(void* dest, void* src, ulong count);
#endif

		public static IntPtr Memcpy(IntPtr b1, IntPtr b2, long count)
		{
			if (b1 == IntPtr.Zero)
				throw new ArgumentException("Zero pointer.", nameof(b1));
			if (b2 == IntPtr.Zero)
				throw new ArgumentException("Zero pointer.", nameof(b2));
			if (count < 0)
				throw new ArgumentOutOfRangeException(nameof(count));

			void* res = MemcpyFunc(b1.ToPointer(), b2.ToPointer(), (ulong) count);

			return new IntPtr(res);
		}

#endregion

#region memcmp

		internal static int Memcmp(void* b1, void* b2, long count) => MemcmpFunc(b1, b2, (ulong) count);

		[DllImport("msvcrt", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
		private static extern int memcmp_win(void* b1, void* b2, ulong count);

#if NETSTANDARD || NETCOREAPP
		[DllImport("libc", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
		private static extern int memcmp_linux(void* b1, void* b2, ulong count);

		[DllImport("libSystem", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
		private static extern int memcmp_osx(void* b1, void* b2, ulong count);
#endif

		public static int Memcmp(IntPtr b1, IntPtr b2, long count)
		{
			if (b1 == IntPtr.Zero)
				throw new ArgumentException("Zero pointer.", nameof(b1));
			if (b2 == IntPtr.Zero)
				throw new ArgumentException("Zero pointer.", nameof(b2));
			if (count < 0)
				throw new ArgumentOutOfRangeException(nameof(count));

			return MemcmpFunc(b1.ToPointer(), b2.ToPointer(), (ulong) count);
		}

#endregion
	}
}
