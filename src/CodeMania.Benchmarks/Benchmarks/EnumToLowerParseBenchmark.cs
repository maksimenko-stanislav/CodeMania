using System;
using BenchmarkDotNet.Attributes;
using CodeMania.Core;
using Common.TestData.TestDataTypes;

namespace CodeMania.Benchmarks.Benchmarks
{
	[MemoryDiagnoser]
	[CoreJob(true), RPlotExporter, RankColumn]
	public class EnumToLowerParseBenchmark
	{
		public string value = NonFlags.Eight.ToString().ToLower();
		private readonly EnumMap<NonFlags> enumMap = EnumMap<NonFlags>.Instance;

		[Benchmark(Baseline = true)]
		public NonFlags EnumParse() => (NonFlags) Enum.Parse(typeof(NonFlags), value, true);

		[Benchmark(Baseline = false)]
		public NonFlags EnumMapParse() => enumMap.Parse(value, true);
	}
}