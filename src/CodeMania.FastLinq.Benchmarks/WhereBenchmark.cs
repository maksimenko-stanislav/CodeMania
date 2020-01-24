using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace CodeMania.FastLinq.Benchmarks
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31, baseline: true)]
    [SimpleJob(RuntimeMoniker.NetCoreApp22)]
    [SimpleJob(RuntimeMoniker.Net461)]
    [MemoryDiagnoser]
    public class WhereNoClosureBenchmark
    {
        private readonly Dictionary<int, List<int>> Lists = new Dictionary<int, List<int>>
        {
            [0] = new List<int>(),
            [10] = Enumerable.Range(0, 10).ToList(),
            [100] = Enumerable.Range(0, 100).ToList(),
            [1000] = Enumerable.Range(0, 1000).ToList(),
        };

        public IEnumerable<object> Sizes
        {
            get
            {
                yield return 0;
                yield return 10;
                yield return 100;
                yield return 1000;
            }
        }

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Sizes))]
        public int RegularLinq(int size)
        {
            int sum = 0;
            foreach (var value in Lists[size].Where(x => x < 5))
            {
                sum += value;
            }

            return sum;
        }

        [Benchmark]
        [ArgumentsSource(nameof(Sizes))]
        public int FastLinq(int size)
        {
            int sum = 0;

            foreach (var value in Lists[size].FastWhere(x => x < 5))
            {
                sum += value;
            }

            return sum;
        }
    }
}