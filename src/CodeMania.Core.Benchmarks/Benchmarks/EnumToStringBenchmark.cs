﻿using BenchmarkDotNet.Attributes;
using Common.TestData.TestDataTypes;

namespace CodeMania.Core.Benchmarks.Benchmarks
{
	[MemoryDiagnoser]
	[CoreJob(true), RPlotExporter, RankColumn]
	public class EnumToStringBenchmark
	{
		private NonFlags value = NonFlags.Two;
		private readonly EnumMap<NonFlags> enumMap = EnumMap<NonFlags>.Instance;

		[Benchmark(Baseline = true)]
		public string EnumToString() => value.ToString();

		[Benchmark(Baseline = false)]
		public string EnumMapToString() => enumMap.ToString(value);
	}
}