using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CodeMania.Core.Internals;
using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers
{
	[PublicAPI]
	public sealed unsafe class ArrayOfStructuresMemoryEqualityComparer<T> : EqualityComparer<T[]>
		where T : struct
	{
		private readonly int _structSize;

		public ArrayOfStructuresMemoryEqualityComparer()
		{
			int? size = default(int?);
			switch (Type.GetTypeCode(typeof(T)))
			{
				case TypeCode.Boolean:
					size = sizeof(bool);
					break;
				case TypeCode.Char:
					size = sizeof(char);
					break;
				case TypeCode.SByte:
					size = sizeof(sbyte);
					break;
				case TypeCode.Byte:
					size = sizeof(byte);
					break;
				case TypeCode.Int16:
					size = sizeof(short);
					break;
				case TypeCode.UInt16:
					size = sizeof(ushort);
					break;
				case TypeCode.Int32:
					size = sizeof(int);
					break;
				case TypeCode.UInt32:
					size = sizeof(uint);
					break;
				case TypeCode.Int64:
					size = sizeof(long);
					break;
				case TypeCode.UInt64:
					size = sizeof(ulong);
					break;
				case TypeCode.Single:
					size = sizeof(float);
					break;
				case TypeCode.Double:
					size = sizeof(double);
					break;
				case TypeCode.Decimal:
					size = sizeof(decimal);
					break;
				case TypeCode.DateTime:
					size = sizeof(DateTime);
					break;
			}

			if (!size.HasValue)
			{
				if (typeof(T) == typeof(TimeSpan))
				{
					size = sizeof(TimeSpan);
				}
				else if (typeof(T) == typeof(DateTimeOffset))
				{
					size = sizeof(DateTimeOffset);
				}
				else if (typeof(T) == typeof(Guid))
				{
					size = sizeof(Guid);
				}
				else
				{
					size = Marshal.SizeOf<T>();
				}
			}

			_structSize = size.Value;
		}

		public override bool Equals(T[] x, T[] y)
		{
			if (ReferenceEquals(x, y)) return true;

			if (x != null && y != null && x.Length == y.Length)
			{
				if (x.Length == 0 && y.Length == 0) return true;

				var gcHandleX = GCHandle.Alloc(x, GCHandleType.Pinned);
				var gcHandleY = GCHandle.Alloc(y, GCHandleType.Pinned);

				try
				{
					return 0 == UnsafeNativeMethods.memcmp(
						       gcHandleX.AddrOfPinnedObject().ToPointer(),
						       gcHandleY.AddrOfPinnedObject().ToPointer(),
						       x.LongLength);
				}
				finally
				{
					gcHandleX.Free();
					gcHandleY.Free();
				}
			}

			return false;
		}

		public override int GetHashCode(T[] obj)
		{
			if (obj == null) throw new ArgumentNullException(nameof(obj));
			if (obj.Length == 0) return 0;

			var gcHandle = GCHandle.Alloc(obj, GCHandleType.Pinned);
			try
			{
				void* startAddr = gcHandle.AddrOfPinnedObject().ToPointer(); // another way Marshal.UnsafeAddrOfPinnedArrayElement(obj, 0);

				int hashCode = HashHelper.HashSeed;

				int i = 0;

				// reinterpret as int*
				int* intAddr = (int*) startAddr;

				var arrayByteSize = obj.Length * _structSize;
				if (arrayByteSize >= sizeof(int))
				{
					for (; i < arrayByteSize; i += sizeof(int))
					{
						hashCode = HashHelper.CombineHashCodes(hashCode * 397, *intAddr++);
					}
				}

				// reinterpret as byte*
				byte* ptrToTheRest = (byte*) intAddr;

				// process the rest of array1
				for (; i < arrayByteSize; i++)
				{
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, *ptrToTheRest++);
				}

				return hashCode;
			}
			finally
			{
				gcHandle.Free();
			}
		}
	}
}