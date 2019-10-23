using System;
using BenchmarkDotNet.Attributes;
using CodeMania.TestData.TestDataTypes;

namespace CodeMania.Core.Benchmarks.Benchmarks
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