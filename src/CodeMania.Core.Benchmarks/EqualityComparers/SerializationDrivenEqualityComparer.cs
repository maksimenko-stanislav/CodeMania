using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CodeMania.Core.EqualityComparers;
using CodeMania.Core.Internals;

namespace CodeMania.Core.Benchmarks.EqualityComparers
{
	public sealed unsafe class SerializationDrivenEqualityComparer<T> : EqualityComparer<T>
	{
		private readonly BinaryFormatter formatter = new BinaryFormatter();

		public override bool Equals(T x, T y)
		{

			if (ReferenceEquals(x, y)) return true;
			if (ReferenceEquals(x, null)) return false;
			if (ReferenceEquals(y, null)) return false;
			if (x.GetType() != y.GetType()) return false;

			//return JsonConvert.SerializeObject(x) == JsonConvert.SerializeObject(y);

			using (var xStream = new MemoryStream())
			using (var yStream = new MemoryStream())
			{
				formatter.Serialize(xStream, x);
				formatter.Serialize(yStream, y);

				if (xStream.Length != yStream.Length) return false;

				fixed (void* xPtr = &xStream.GetBuffer()[0])
				fixed (void* yPtr = &yStream.GetBuffer()[0])
                {
                    return UnsafeNativeMethods.Memcmp(new IntPtr(xPtr), new IntPtr(yPtr), xStream.Length) == 0;
                }
			}
		}

		public override int GetHashCode(T obj)
		{
			if (obj == null) return 0;

			//return JsonConvert.SerializeObject(obj).GetHashCode();

			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, obj);

				return PrimitiveTypeArrayEqualityComparers.ByteArrayMemoryEqualityComparer.GetHashCode(stream.ToArray());
			}
		}
	}
}