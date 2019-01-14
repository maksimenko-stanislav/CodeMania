using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CodeMania.Core.EqualityComparers;
using Common.TestData.TestDataTypes;
using NUnit.Framework;

namespace CodeMania.UnitTests.EqualityComparers
{
	[TestFixture]
	public class TestEntityEqualityComparerTests : EqualityComparerTestsBase<TestEntity>
	{
		public TestEntityEqualityComparerTests() : base(new ObjectStructureEqualityComparer<TestEntity>())
		{
		}

		protected override IEnumerable<TestCase> GetTestCases()
		{
			return AutoTestCaseSources.Concat(ManualTestCaseSources);
		}

		public static IEnumerable<TestCase> AutoTestCaseSources
		{
			get
			{
				Fixture fixture = new Fixture();
				var recursionBehavior = fixture.Behaviors.FirstOrDefault(x => x is ThrowingRecursionBehavior);
				if (recursionBehavior != null) fixture.Behaviors.Remove(recursionBehavior);
				fixture.Behaviors.Add(new OmitOnRecursionBehavior());

				var random = new Random(Guid.NewGuid().GetHashCode());
				var built = fixture.Build<TestEntity>()
					.With(x => x.String, Guid.NewGuid().ToString())
					.With(x => x.Strings, Enumerable.Range(0, 64).Select(y => Guid.NewGuid().ToString()).ToArray())
					.With(x => x.Guid, Guid.NewGuid())
					.With(x => x.NullableGuid, random.Next() % 2 == 0 ? Guid.NewGuid() : default(Guid?))
					.With(x => x.Guids, Enumerable.Range(0, 64).Select(y => Guid.NewGuid()).ToArray())
					.With(x => x.NullableGuids, Enumerable.Range(0, 64).Select(y => (y ^ random.Next()) % 2 == 0 ? Guid.NewGuid() : default(Guid?)).ToArray())
					.With(x => x.Guids, Enumerable.Range(0, 64).Select(y => Guid.NewGuid()).ToArray());

				for (int i = 0; i < 100; i++)
				{
					yield return Create(built.Create(), built.Create(), false);
				}
			}
		}

		public static IEnumerable<TestCase> ManualTestCaseSources
		{
			get
			{
				yield return Create(
					() => new TestEntity(),
					t1 => new TestEntity(),
					(t1, t2) => true
				);
				yield return Create(
					() => new TestEntity(),
					t1 => t1,
					(t1, t2) => true
				);
				yield return Create(
					() => new TestEntity { NullableGuid = Guid.Empty },
					t1 => new TestEntity(),
					(t1, t2) => false
				);
				yield return Create(
					() => new TestEntity(),
					t1 => new TestEntity { NullableGuid = Guid.Empty },
					(t1, t2) => false
				);
				yield return Create(
					() => new TestEntity { Integer = int.MinValue },
					t1 => new TestEntity { Integer = int.MinValue },
					(t1, t2) => true
				);
				yield return Create(
					() => new TestEntity { Integer = 0 },
					t1 => new TestEntity { Integer = int.MinValue },
					(t1, t2) => false
				);
				yield return Create(
					() => new TestEntity(),
					t1 => new TestEntity(),
					(t1, t2) =>
					{
						// cyclic reference
						t1.Parent = t1;
						t2.Parent = t2;

						return true;
					});
				yield return Create(
					() => new TestEntity(),
					t1 => new TestEntity(),
					(t1, t2) =>
					{
						// cyclic references
						t1.Children = new List<TestEntity> { t1 };
						t2.Children = new List<TestEntity> { t2 };

						return true;
					});
				yield return Create(
					() => new TestEntity
					{
						Guids = new[] { Guid.Empty },
						DateTime = DateTime.MaxValue,
						Decimal = 0.01m
					},
					t1 => new TestEntity
					{
						Guids = new[] { Guid.Empty },
						DateTime = DateTime.MaxValue,
						Decimal = 0.01m
					},
					(t1, t2) => true
				);
				//yield return Create(
				//	() => new TestEntity
				//	{
				//		StringIntDictionary = new Dictionary<string, int>
				//		{
				//			["1"] = 1,
				//			["2"] = 2,
				//			["3"] = 3
				//		}
				//	},
				//	t1 => new TestEntity
				//	{
				//		StringIntDictionary = new Dictionary<string, int>
				//		{
				//			["1"] = 1,
				//			["2"] = 2,
				//			["3"] = 3
				//		}
				//	},
				//	(t1, t2) => true
				//);
				//yield return Create(
				//	() => new TestEntity
				//	{

				//	},
				//	t1 => new TestEntity
				//	{
				//		StringIntDictionary = new Dictionary<string, int>
				//		{
				//			["1"] = 1,
				//			["2"] = 2,
				//			["3"] = 3
				//		}
				//	},
				//	(t1, t2) => false
				//);
				//yield return Create(
				//	() => new TestEntity
				//	{
				//		StringIntDictionary = new Dictionary<string, int>
				//		{
				//			["1"] = 1,
				//			["2"] = 2,
				//			["3"] = 3
				//		}
				//	},
				//	t1 => new TestEntity
				//	{

				//	},
				//	(t1, t2) => false
				//);
				//yield return Create(
				//	() => new TestEntity
				//	{
				//		StringIntDictionary = new Dictionary<string, int>
				//		{
				//			["1"] = 1,
				//			["2"] = 2,
				//			["3"] = 3
				//		}
				//	},
				//	t1 => new TestEntity
				//	{
				//		StringIntDictionary = new Dictionary<string, int>
				//		{
				//			["1"] = 1,
				//			["2"] = 2
				//		}
				//	},
				//	(t1, t2) => false
				//);
				//yield return Create(
				//	() => new TestEntity
				//	{
				//		StringIntDictionary = new Dictionary<string, int>
				//		{
				//			["1"] = 1,
				//			["2"] = 2,
				//			["3"] = 3
				//		}
				//	},
				//	t1 => new TestEntity
				//	{
				//		StringIntDictionary = new Dictionary<string, int>
				//		{
				//			["1"] = 1,
				//			["2"] = 2,
				//			["333"] = 3
				//		}
				//	},
				//	(t1, t2) => false
				//);
				//yield return Create(
				//	() => new TestEntity
				//	{
				//		StringIntDictionary = new Dictionary<string, int>
				//		{
				//			["1"] = 1,
				//			["2"] = 2,
				//			["3"] = 3
				//		}
				//	},
				//	t1 => new TestEntity
				//	{
				//		StringIntDictionary = new Dictionary<string, int>
				//		{
				//			["1"] = 1,
				//			["2"] = 2,
				//			["3"] = 333
				//		}
				//	},
				//	(t1, t2) => false
				//);
			}
		}
	}
}