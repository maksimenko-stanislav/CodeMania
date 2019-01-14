using System.Collections.Generic;
using CodeMania.Core.EqualityComparers.BlittableTypeArrayEqualityComparers;
using NUnit.Framework;

namespace CodeMania.UnitTests.EqualityComparers
{
	[TestFixture]
	public class IntArrayMemoryEqualityComparerTestBase : ArrayMemoryEqualityComparerTestBase<int>
	{
		public IntArrayMemoryEqualityComparerTestBase() : base(new Int32ArrayMemoryEqualityComparer())
		{
		}

		//[Test]
		//public void CustomTest()
		//{
		//	var x = new int[] { 1, 2, 3 };
		//	var y = new int[] { 1, 2, 0 };

		//	Assert.IsFalse(base.EqualityComparer.Equals(x, y));
		//}

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
		}
	}
}