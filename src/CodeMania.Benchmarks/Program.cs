using System;
using BenchmarkDotNet.Running;
using CodeMania.Benchmarks.Benchmarks;

namespace CodeMania.Benchmarks
{
	class Program
	{
		static void Main(string[] args)
		{
			//BenchmarkRunner.Run<EqualityComparer_EqualsTests>();
			BenchmarkRunner.Run<EqualityComparer_GetHashCode>();
			
			Console.ReadLine();
		}
	}
}
