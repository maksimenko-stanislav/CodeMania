using System;
using System.Linq;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using CodeMania.Core.Benchmarks.EqualityComparers;
using CodeMania.Core.Benchmarks.Utils;
using CodeMania.Core.EqualityComparers;
using CodeMania.TestData.TestDataTypes;

// ReSharper disable InconsistentNaming

namespace CodeMania.Core.Benchmarks.Benchmarks
{
    public abstract class EqualityComparerBenchmarkBase
    {
        protected readonly TestEntityEqualityComparer _manualComparer = new TestEntityEqualityComparer();

        protected readonly ReflectionBasedObjectStructureEqualityComparer<TestEntity> _reflectionComparer =
            new ReflectionBasedObjectStructureEqualityComparer<TestEntity>();

        protected readonly ObjectStructureEqualityComparer<TestEntity> _compiledExpressionComparer =
            new ObjectStructureEqualityComparer<TestEntity>();

        protected readonly SerializationDrivenEqualityComparer<TestEntity> _SerializationDrivenEqualityComparer =
            new SerializationDrivenEqualityComparer<TestEntity>();

        protected readonly TestEntity _x;
        protected readonly TestEntity _y;

        protected EqualityComparerBenchmarkBase()
        {
            _x = CreateRandom();
            _y = DeepCopy.Create(_x);
        }

        private static TestEntity CreateRandom()
        {
            Fixture fixture = new Fixture();
            var recursionBehavior = fixture.Behaviors.FirstOrDefault(x => x is ThrowingRecursionBehavior);
            if (recursionBehavior != null) fixture.Behaviors.Remove(recursionBehavior);
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var random = new Random(Guid.NewGuid().GetHashCode());
            var built = fixture.Build<TestEntity>()
                .With(x => x.String, Guid.NewGuid().ToString())
                .With(x => x.Strings, Enumerable.Range(0, 128).Select(y => Guid.NewGuid().ToString()).ToArray())
                .With(x => x.Guid, Guid.Empty)
                .With(x => x.NullableGuid, random.Next() % 2 == 0 ? Guid.NewGuid() : default(Guid?))
                .With(x => x.Guids, Enumerable.Range(0, 128).Select(y => Guid.NewGuid()).ToArray())
                .With(x => x.NullableGuids,
                    Enumerable.Range(0, 128).Select(y => (y ^ random.Next()) % 2 == 0 ? Guid.NewGuid() : default(Guid?))
                        .ToArray())
                .With(x => x.DateTimeOffsets, Enumerable.Range(0, 128).Select(y => DateTimeOffset.Now).ToArray())
                .With(x => x.Decimals, Enumerable.Range(0, 128).Select(y => (decimal) y).ToArray())
                .With(x => x.NullableDoubles,
                    Enumerable.Range(0, 128).Select(y => y % 2 == 0 ? new Random(y).NextDouble() : default(double?))
                        .ToArray())
                .With(x => x.Floats, Enumerable.Range(0, 128).Select(y => (float) new Random(y).NextDouble()).ToArray())
                .With(x => x.NullableInt64Enums,
                    Enumerable.Range(0, 128).Select(y => y % 2 == 0 ? (Int64Enum?) (y % 4) : default(Int64Enum?))
                        .ToArray())
                .With(x => x.Int16Enums, Enumerable.Range(0, 128).Select(y => (Int16Enum) (y % 4)).ToArray())
                .With(x => x.TimeSpans, Enumerable.Range(0, 128).Select(y => TimeSpan.FromMilliseconds(y)).ToArray());

            return built.Create();
        }
    }

    [MemoryDiagnoser]
    [CoreJob(true), RPlotExporter, RankColumn]
    public class EqualityComparer_EqualsTests : EqualityComparerBenchmarkBase
    {
        [Benchmark(Baseline = true)]
        public bool ManualComparer() => _manualComparer.Equals(_x, _y);

        [Benchmark]
        public bool ReflectionComparer() => _reflectionComparer.Equals(_x, _y);

        [Benchmark]
        public bool SerializationDrivenEqualityComparer() => _SerializationDrivenEqualityComparer.Equals(_x, _y);

        [Benchmark]
        public bool ObjectStructureComparer() => _compiledExpressionComparer.Equals(_x, _y);
    }

    [MemoryDiagnoser]
    [CoreJob(true), RPlotExporter, RankColumn]
    public class EqualityComparer_GetHashCode : EqualityComparerBenchmarkBase
    {
        [Benchmark(Baseline = true)]
        public int ManualComparer() => _manualComparer.GetHashCode(_x);

        [Benchmark]
        public int ReflectionComparer() => _reflectionComparer.GetHashCode(_x);

        [Benchmark]
        public int SerializationDrivenEqualityComparer() => _SerializationDrivenEqualityComparer.GetHashCode(_x);

        [Benchmark]
        public int ObjectStructureComparer() => _compiledExpressionComparer.GetHashCode(_x);
    }
}