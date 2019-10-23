using System.Collections.Generic;
using CodeMania.Core.EqualityComparers.Common.ReferenceType;
using NUnit.Framework;

namespace CodeMania.UnitTests.EqualityComparers
{
	[TestFixture]
	public class ListOfIntArrayEqualityComparerTests : EqualityComparerTestsBase<List<int[]>>
	{
		public ListOfIntArrayEqualityComparerTests() : base(ReferenceTypeListEqualityComparer<int[]>.Default)
		{
		}

		protected override IEnumerable<TestCase> GetTestCases()
		{
			yield return Create(null, null, true);
			yield return Create(new List<int[]> { new int[0] }, null, false);
			yield return Create(null, new List<int[]> { new int[0] }, false);
			yield return Create(new List<int[]> { new int[0] }, new List<int[]> { new int[0] }, true);
			yield return Create(new List<int[]> { new int[] { 1, 2, 3 } }, new List<int[]> { new int[] { 1, 2, 3 } }, true);
			yield return Create(new List<int[]> { new int[] { 0, 2, 3 } }, new List<int[]> { new int[] { 1, 2, 3 } }, false);
			yield return Create(new List<int[]> { new int[] { 1, 2, 3 } }, new List<int[]> { new int[] { 1, 2, 0 } }, false);
			yield return Create(
				new List<int[]>
				{
					new int[] { 1, 2, 3 },
					new int[] { 1, 2, 3, 4 }
				},
				new List<int[]>
				{
					new int[] { 1, 2, 3 },
					new int[] { 1, 2, 3, 4 },
				},
				true);
			yield return Create(
				new List<int[]>
				{
					new int[] { 1, 2, 3 },
				},
				new List<int[]>
				{
					new int[] { 1, 2, 3 },
					new int[] { 1, 2, 3, 4 },
				},
				false);
			yield return Create(
				new List<int[]>
				{
					new int[] { 1, 2, 3 },
					new int[] { 1, 2, 3, 4 }
				},
				new List<int[]>
				{
					new int[] { 1, 2, 3, 4 },
				},
				false);
			yield return Create(
				new List<int[]>
				{
					new int[] { 1, 2, 3 },
					null
				},
				new List<int[]>
				{
					new int[] { 1, 2, 3 },
					new int[] { 1, 2, 3, 4 },
				},
				false);
			yield return Create(
				new List<int[]>
				{
					new int[] { 1, 2, 3 },
					new int[] { 1, 2, 3, 4 }
				},
				new List<int[]>
				{
					new int[] { 1, 2, 3 },
					null,
				},
				false);
		}
	}
}