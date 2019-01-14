using System;
using CodeMania.Core.Serialization;
using Common.TestData.TestDataTypes;
using NUnit.Framework;

// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

namespace CodeMania.UnitTests.Serialization
{
	[TestFixture]
	public class QueryStringSerializerTest
	{
		public class SomeEntity
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
			public ByteEnum?[] NullableByteEnums { get; set; }
			//public Int16Enum Int16Enum { get; set; }
			//public Int16Enum[] Int16Enums { get; set; }
			//public Int16Enum? NullableInt16Enum { get; set; }
			//public Int16Enum?[] NullableInt16Enums { get; set; }
			//public Int32Enum Int32Enum { get; set; }
			//public Int32Enum[] Int32Enums { get; set; }
			//public Int32Enum? NullableInt32Enum { get; set; }
			//public Int32Enum?[] NullableInt32Enums { get; set; }
			//public Int64Enum Int64Enum { get; set; }
			//public Int64Enum[] Int64Enums { get; set; }
			//public Int64Enum? NullableInt64Enum { get; set; }
			//public Int64Enum?[] NullableInt64Enums { get; set; }
		}

		[Test]
		public void Test()
		{
			// arrange
			var serializer = QueryStringSerializerBuilder.Create<SomeEntity>()
				//.Ignore(x => x.DateTime)
				//.WithName(x => x.Bools, "booleans")
				//.ConvertPropertyValueWith(x => x.Decimal, (entity, value) => value.ToString("C0", NumberFormatInfo.InvariantInfo))
				//.ConvertPropertyValueWith(x => x.ByteEnums, EnumCollectionToStringsConverter<ByteEnum>.Default)
				//.ConvertPropertyValueWith(x => x.NullableByteEnums, (entity, value) => value.Select(x => x.ToString()))
				//.WithConverter(EnumCollectionToStringsConverter<ByteEnum>.Default)
				//.ConvertPropertyValueWith(x => x.ByteEnum, new EnumMemberToStringConverter<ByteEnum>())
				//.WithName(x => x.DateTime, property => property.GetCustomAttribute<DataMemberAttribute>()?.Name ?? property.Name)
				.Build();

			// act
			var result = serializer.Serialize(new SomeEntity { ByteEnum = ByteEnum.Value4, Bool = true, Ints = new[] { 1, 2, 3 } });

			// assert
			Assert.AreEqual("Bool=True&ByteEnum=Value4&Ints=1&Ints=2&Ints=3", result);
		}
	}
}