using System.Collections.Generic;
using CodeMania.Core.EqualityComparers;
using NUnit.Framework;

namespace CodeMania.UnitTests.EqualityComparers
{
	[TestFixture]
	public class ArrayOfIntArrayEqualityComparerTests : EqualityComparerTestsBase<int[][]>
	{
		public ArrayOfIntArrayEqualityComparerTests() : base(ArrayEqualityComparer<int[]>.Instance)
		{
		}

		protected override IEnumerable<TestCase> GetTestCases()
		{
			yield return Create(null, null, true);
			yield return Create(new int[][] { new int[0] }, null, false);
			yield return Create(null, new int[][] { new int[0] }, false);
			yield return Create(new int[][] {new int[0]}, new int[][] {new int[0]}, true);
			yield return Create(new int[][] {new int[] {1, 2, 3}}, new int[][] {new int[] {1, 2, 3}}, true);
			yield return Create(new int[][] {new int[] {0, 2, 3}}, new int[][] {new int[] {1, 2, 3}}, false);
			yield return Create(new int[][] {new int[] {1, 2, 3}}, new int[][] {new int[] {1, 2, 0}}, false);
			yield return Create(
				new int[][]
				{
					new int[] { 1, 2, 3 },
					new int[] { 1, 2, 3, 4 }
				},
				new int[][]
				{
					new int[] { 1, 2, 3 },
					new int[] { 1, 2, 3, 4 },
				},
				true);
			yield return Create(
				new int[][]
				{
					new int[] { 1, 2, 3 },
				},
				new int[][]
				{
					new int[] { 1, 2, 3 },
					new int[] { 1, 2, 3, 4 },
				},
				false);
			yield return Create(
				new int[][]
				{
					new int[] { 1, 2, 3 },
					new int[] { 1, 2, 3, 4 }
				},
				new int[][]
				{
					new int[] { 1, 2, 3, 4 },
				},
				false);
			yield return Create(
				new int[][]
				{
					new int[] { 1, 2, 3 },
					null
				},
				new int[][]
				{
					new int[] { 1, 2, 3 },
					new int[] { 1, 2, 3, 4 },
				},
				false);
			yield return Create(
				new int[][]
				{
					new int[] { 1, 2, 3 },
					new int[] { 1, 2, 3, 4 }
				},
				new int[][]
				{
					new int[] { 1, 2, 3 },
					null,
				},
				false);
		}
	}
}