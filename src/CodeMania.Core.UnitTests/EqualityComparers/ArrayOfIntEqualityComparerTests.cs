using System.Collections.Generic;
using CodeMania.Core.EqualityComparers;
using NUnit.Framework;

namespace CodeMania.UnitTests.EqualityComparers
{
	[TestFixture]
	public class ArrayOfIntEqualityComparerTests : EqualityComparerTestsBase<int[]>
	{
		public ArrayOfIntEqualityComparerTests() : base(ArrayEqualityComparer<int>.Instance)
		{
		}

		protected override IEnumerable<TestCase> GetTestCases()
		{
			yield return Create(null, null, true);
			yield return Create(new int[] { }, null, false);
			yield return Create(null, new int[] { }, false);
			yield return Create(new int[] { }, new int[] { }, true);
			yield return Create(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 }, true);
			yield return Create(new int[] { 0, 2, 3 }, new int[] { 1, 2, 3 }, false);
			yield return Create(new int[] { 1, 2, 3 }, new int[] { 1, 2, 0 }, false);
		}
	}
}