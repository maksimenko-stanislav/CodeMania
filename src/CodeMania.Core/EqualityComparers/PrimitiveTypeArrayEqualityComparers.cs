using CodeMania.Core.EqualityComparers.BlittableTypeArrayEqualityComparers;

namespace CodeMania.Core.EqualityComparers
{
	/// <summary>
	/// [Internal use only] Provides pre-created instances of equality comparer for array of primitive types.
	/// </summary>
	public static class PrimitiveTypeArrayEqualityComparers
	{
		// ReSharper disable MultipleSpaces
		public static readonly BooleanArrayMemoryEqualityComparer  BooleanArrayMemoryEqualityComparer  = new BooleanArrayMemoryEqualityComparer();
		public static readonly ByteArrayMemoryEqualityComparer     ByteArrayMemoryEqualityComparer     = new ByteArrayMemoryEqualityComparer();
		public static readonly DateTimeArrayMemoryEqualityComparer DateTimeArrayMemoryEqualityComparer = new DateTimeArrayMemoryEqualityComparer();
		public static readonly GuidArrayMemoryEqualityComparer     GuidArrayMemoryEqualityComparer     = new GuidArrayMemoryEqualityComparer();
		public static readonly Int16ArrayMemoryEqualityComparer    Int16ArrayMemoryEqualityComparer    = new Int16ArrayMemoryEqualityComparer();
		public static readonly Int32ArrayMemoryEqualityComparer    Int32ArrayMemoryEqualityComparer    = new Int32ArrayMemoryEqualityComparer();
		public static readonly Int64ArrayMemoryEqualityComparer    Int64ArrayMemoryEqualityComparer    = new Int64ArrayMemoryEqualityComparer();
		public static readonly SByteArrayMemoryEqualityComparer    SByteArrayMemoryEqualityComparer    = new SByteArrayMemoryEqualityComparer();
		public static readonly TimeSpanArrayMemoryEqualityComparer TimeSpanArrayMemoryEqualityComparer = new TimeSpanArrayMemoryEqualityComparer();
		public static readonly UInt16ArrayMemoryEqualityComparer   UInt16ArrayMemoryEqualityComparer   = new UInt16ArrayMemoryEqualityComparer();
		public static readonly UInt32ArrayMemoryEqualityComparer   UInt32ArrayMemoryEqualityComparer   = new UInt32ArrayMemoryEqualityComparer();
		public static readonly UInt64ArrayMemoryEqualityComparer   UInt64ArrayMemoryEqualityComparer   = new UInt64ArrayMemoryEqualityComparer();
		public static readonly CharArrayMemoryEqualityComparer     CharArrayMemoryEqualityComparer     = new CharArrayMemoryEqualityComparer();
		// ReSharper restore MultipleSpaces
	}
}