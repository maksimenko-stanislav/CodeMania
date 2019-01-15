using System;
using BenchmarkDotNet.Running;
using CodeMania.Core.Benchmarks.Benchmarks;

namespace CodeMania.Core.Benchmarks
{
	class Program
	{
		static void Main(string[] args)
		{
			BenchmarkRunner.Run<EqualityComparer_EqualsTests>();
			//BenchmarkRunner.Run<EqualityComparer_GetHashCode>();

			Console.ReadLine();
		}
	}
}
