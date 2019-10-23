using System;
using System.Collections.Generic;

namespace CodeMania.TestData.TestDataTypes
{
	[Serializable]
	public class TestEntity
	{
		public int Integer { get; set; }
		public int[] Ints { get; set; }
		public int? NullableInt { get; set; }
		public int[] NullableInts { get; set; }
		public byte Byte { get; set; }
		public byte[] Bytes { get; set; }
		public byte? NullableByte { get; set; }
		public byte?[] NullableBytes { get; set; }
		public bool Bool { get; set; }
		public bool[] Bools { get; set; }
		public bool? NullableBool { get; set; }
		public bool?[] NullableBools { get; set; }
		public float Float { get; set; }
		public float[] Floats { get; set; }
		public float? NullableFloat { get; set; }
		public float?[] NullableFloats { get; set; }
		public double Double { get; set; }
		public double[] Doubles { get; set; }
		public double? NullableDouble { get; set; }
		public double?[] NullableDoubles { get; set; }
		public decimal Decimal { get; set; }
		public decimal[] Decimals { get; set; }
		public decimal? NullableDecimal { get; set; }
		public decimal?[] NullableDecimals { get; set; }
		public Guid Guid { get; set; }
		public Guid[] Guids { get; set; }
		public Guid? NullableGuid { get; set; }
		public Guid?[] NullableGuids { get; set; }
		public DateTime DateTime { get; set; }
		public DateTime[] DateTimes { get; set; }
		public DateTime? NullabelDateTime { get; set; }
		public DateTime?[] NullabelDateTimes { get; set; }
		public TimeSpan TimeSpan { get; set; }
		public TimeSpan[] TimeSpans { get; set; }
		public TimeSpan? NullableTimeSpan { get; set; }
		public TimeSpan?[] NullableTimeSpans { get; set; }
		public DateTimeOffset DateTimeOffset { get; set; }
		public DateTimeOffset[] DateTimeOffsets { get; set; }
		public DateTimeOffset? NullableDateTimeOffset { get; set; }
		public DateTimeOffset?[] NullableDateTimeOffsets { get; set; }
		public string String { get; set; }
		public string[] Strings { get; set; }
		public ByteEnum ByteEnum { get; set; }
		public ByteEnum[] ByteEnums { get; set; }
		public ByteEnum? NullableByteEnum { get; set; }
		public ByteEnum[] NullableByteEnums { get; set; }
		public Int16Enum    Int16Enum { get; set; }
		public Int16Enum[] Int16Enums { get; set; }
		public Int16Enum?   NullableInt16Enum { get; set; }
		public Int16Enum?[]  NullableInt16Enums { get; set; }
		public Int32Enum Int32Enum { get; set; }
		public Int32Enum[] Int32Enums { get; set; }
		public Int32Enum? NullableInt32Enum { get; set; }
		public Int32Enum?[] NullableInt32Enums { get; set; }
		public Int64Enum Int64Enum { get; set; }
		public Int64Enum[] Int64Enums { get; set; }
		public Int64Enum? NullableInt64Enum { get; set; }
		public Int64Enum?[] NullableInt64Enums { get; set; }
		public UserDefinedStruct UserDefinedStruct { get; set; }
		public UserDefinedStruct[] UserDefinedStructs { get; set; }
		public UserDefinedStruct? NullableUserDefinedStruct { get; set; }
		public UserDefinedStruct?[] NullableUserDefinedStructs { get; set; }
		public TestEntity Parent { get; set; }
		public List<TestEntity> Children { get; set; }
		public OtherEntity OtherEntity { get; set; }
		public List<OtherEntity> OtherEntities { get; set; }
		//public Dictionary<string, int> StringIntDictionary { get; set; }
	}
}