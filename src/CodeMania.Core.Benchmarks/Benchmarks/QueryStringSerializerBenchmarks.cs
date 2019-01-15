using System;
using System.Linq;
using System.Runtime.Serialization;
using BenchmarkDotNet.Attributes;
using CodeMania.Core.Serialization;

namespace CodeMania.Core.Benchmarks.Benchmarks
{
	[DataContract]
	public class TestEntity1
	{
		[DataMember]
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
		//public ByteEnum ByteEnum { get; set; }
		//public ByteEnum[] ByteEnums { get; set; }
		//public ByteEnum? NullableByteEnum { get; set; }
		//public ByteEnum[] NullableByteEnums { get; set; }
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

	[MemoryDiagnoser]
	[CoreJob(true), RPlotExporter, RankColumn]
	public class QueryStringSerializerBenchmarks
	{
		private readonly QueryStringSerializer<TestEntity1> queryStringSerializer = QueryStringSerializerBuilder.Create<TestEntity1>().Build();

		private readonly ReflectionBasedQueryStringSerializer<TestEntity1> reflectionBasesQueryStringSerializer =
			new ReflectionBasedQueryStringSerializer<TestEntity1>();

		private TestEntity1 entityToSerialize = new TestEntity1
		{
			Bool = true,
			Bools = new bool[] {true, false, true},
			DateTime = DateTime.UtcNow,
			DateTimes = Enumerable.Range(0, 10).Select(x => DateTime.UtcNow).ToArray(),
			Bytes = Enumerable.Range(0, 255).Select(x => (byte) x).ToArray(),
			Ints = Enumerable.Range(0, 255).Select(x => x).ToArray(),
			Doubles = Enumerable.Range(0, 255).Select((x, i) => x + i * 0.1).ToArray(),
			Double = 0.0000001,
			String = "1234567890qwertyuiopasdfghjkl+-~d"
		};

		[Benchmark(Baseline = true)]
		public string Optimized() => queryStringSerializer.Serialize(entityToSerialize);

		[Benchmark(Baseline = false)]
		public string ReflectionBased() => reflectionBasesQueryStringSerializer.Serialize(entityToSerialize);
	}
}