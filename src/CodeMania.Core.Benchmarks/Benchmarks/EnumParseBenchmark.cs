using System;
using BenchmarkDotNet.Attributes;
using CodeMania.TestData.TestDataTypes;

namespace CodeMania.Core.Benchmarks.Benchmarks
{
	[MemoryDiagnoser]
	[SimpleJob(baseline: true), RPlotExporter, RankColumn]
	public class EnumParseBenchmark
	{
		public string fileAccess = NonFlags.Four.ToString();
		public readonly EnumMap<NonFlags> enumMap = EnumMap<NonFlags>.Instance;

		[Benchmark(Baseline = true)]
		public NonFlags EnumParse() => (NonFlags) Enum.Parse(typeof(NonFlags), fileAccess);

		[Benchmark(Baseline = false)]
		public NonFlags EnumMapParse() => enumMap.Parse(fileAccess);
	}
}