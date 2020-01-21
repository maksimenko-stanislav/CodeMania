using System;
using System.Collections.Generic;
using System.Linq;
using CodeMania.Core.EqualityComparers.Specialized;
using NUnit.Framework;

namespace CodeMania.UnitTests.EqualityComparers
{
	[TestFixture]
	public class IntArrayMemoryEqualityComparerTestBase : ArrayMemoryEqualityComparerTestBase<int>
	{
		public IntArrayMemoryEqualityComparerTestBase() : base(UnmanagedTypeArrayEqualityComparer<int>.Default)
		{
		}

		protected override IEnumerable<TestCase> GetTestCases()
		{
			yield return new TestCase(new int[] { 1 }, new int[] { 1 }, true);
			yield return new TestCase(new int[] { 1 }, new int[] { 0 }, false);
			yield return new TestCase(new int[] { 0 }, new int[] { 1 }, false);
			yield return new TestCase(new int[] { 1, 2, 3 }, new int[] { 1, 2, 0 }, false);
			yield return new TestCase(new int[] { }, new int[] { 1 }, false);
			yield return new TestCase(new int[] { }, new int[] { }, true);
			yield return new TestCase(new int[1024], new int[1024], true);
			yield return new TestCase(new int[1024], new int[1025], false);

			var random = new Random(Guid.NewGuid().GetHashCode());
			var ints = Enumerable.Range(0, 10000).Select(x => random.Next()).ToArray();
			var ints2 = ints.ToList().ToArray();
			yield return new TestCase(ints, ints2, true);

			ints2[ints2.Length - 1] = unchecked(~(ints2[ints2.Length - 1] + 1));
			yield return new TestCase(ints, ints2, false);
		}
	}
}