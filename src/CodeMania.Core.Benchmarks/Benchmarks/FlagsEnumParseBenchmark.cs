using System;
using BenchmarkDotNet.Attributes;
using CodeMania.TestData.TestDataTypes;

namespace CodeMania.Core.Benchmarks.Benchmarks
{
	[MemoryDiagnoser]
	[SimpleJob(baseline: true), RPlotExporter, RankColumn]
	public class FlagsEnumParseBenchmark
	{
		private string fileAccess = (Flags.Eight | Flags.Four | Flags.Two).ToString();
		private readonly EnumMap<Flags> enumMap = EnumMap<Flags>.Instance;

		[Benchmark(Baseline = true)]
		public Flags EnumParse() => (Flags)Enum.Parse(typeof(Flags), fileAccess);

		[Benchmark(Baseline = false)]
		public Flags EnumMapParse() => enumMap.Parse(fileAccess);
	}
}