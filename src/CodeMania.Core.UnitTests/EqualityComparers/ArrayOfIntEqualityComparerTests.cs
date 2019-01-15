using System.Collections.Generic;
using CodeMania.Core.EqualityComparers;
using CodeMania.Core.EqualityComparers.BlittableTypeArrayEqualityComparers;
using NUnit.Framework;

namespace CodeMania.UnitTests.EqualityComparers
{
	[TestFixture]
	public class ArrayOfIntEqualityComparerTests : EqualityComparerTestsBase<int[]>
	{
		public ArrayOfIntEqualityComparerTests() : base(new Int32ArrayMemoryEqualityComparer())
		{
		}

		protected override IEnumerable<TestCase> GetTestCases()
		{
			//yield return Create(null, null, true);
			//yield return Create(new int[] { }, null, false);
			//yield return Create(null, new int[] { }, false);
			//yield return Create(new int[] { }, new int[] { }, true);
			//yield return Create(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 }, true);
			yield return Create(new int[] { 0, 1, 2, 3, 4, 5, 6 ,7, 8, 9, -1 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, -1 }, true);
			//yield return Create(new int[] { 0, 2, 3 }, new int[] { 1, 2, 3 }, false);
			//yield return Create(new int[] { 1, 2, 3 }, new int[] { 1, 2, 0 }, false);
		}
	}
}