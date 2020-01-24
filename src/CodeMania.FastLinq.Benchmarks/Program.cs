using System;
using BenchmarkDotNet.Running;

namespace CodeMania.FastLinq.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<WhereNoClosureBenchmark>();
            Console.ReadLine();
        }
    }
}
