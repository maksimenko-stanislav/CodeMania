using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace CodeMania.Core.Benchmarks.Benchmarks
{
	[MemoryDiagnoser]
	[SimpleJob(baseline: true), RPlotExporter, RankColumn]
	public class MethodCallBenchmark
	{
		private readonly MethodInfo methodToCall;
		private readonly Delegate delegateToCall;
		private readonly Func<string, int, string> typedDelegateToCall;

		public MethodCallBenchmark()
		{
			methodToCall = GetType().GetMethod("MethodToCall");
			delegateToCall = Delegate.CreateDelegate(typeof(Func<string, int, string>), this, methodToCall);
			typedDelegateToCall = (Func<string, int, string>) delegateToCall;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string MethodToCall(string arg1, int arg2) => arg1 + arg2.ToString();

		[Benchmark(Baseline = true)]
		public string DirectCall() => MethodToCall("id", 1);

		[Benchmark]
		public string MethodInfoInvoke() => (string) methodToCall.Invoke(this, new object[] {"id", 1});

		[Benchmark]
		public string DelegateDynamicInvoke() => (string) delegateToCall.DynamicInvoke("id", 1);

		[Benchmark]
		public string DelegateTypedInvoke() => typedDelegateToCall("id", 1);
	}
}