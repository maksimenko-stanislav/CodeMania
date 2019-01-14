using System;
using System.Collections.Generic;
using System.Linq;
using CodeMania.Core.EqualityComparers;
using CodeMania.Core.EqualityComparers.BlittableTypeArrayEqualityComparers;
using NUnit.Framework;

// ReSharper disable RedundantExplicitArrayCreation

namespace CodeMania.UnitTests.EqualityComparers
{
	[TestFixture]
	public class ArrayOfStructuresMemoryEqualityComparerTest
	{
		[TestCaseSource(nameof(ByteTestCaseDatas))]
		public void Equals_ByteArray_ReturnsValidResult_ByteArrayMemoryEqualityComparer(byte[] x, byte[] y, bool expected)
		{
			// arrange
			var comparer = new ByteArrayMemoryEqualityComparer();

			// act
			var actual = comparer.Equals(x, y);

			// assert
			Assert.AreEqual(expected, actual);
		}

		[TestCaseSource(nameof(ByteTestCaseDatas))]
		public void Equals_ByteArray_ReturnsValidArray(byte[] x, byte[] y, bool expected)
		{
			// arrange
			var comparer = new ArrayOfStructuresMemoryEqualityComparer<byte>();

			// act
			var actual = comparer.Equals(x, y);

			// assert
			Assert.AreEqual(expected, actual);
		}

		[TestCaseSource(nameof(ByteTestCaseDatas))]
		public void GetHashCode_ByteArray_ReturnsValidArray(byte[] x, byte[] y, bool expected)
		{
			// arrange
			var comparer = new ArrayOfStructuresMemoryEqualityComparer<byte>();

			// act
			var hashX = comparer.GetHashCode(x);
			var hashY = comparer.GetHashCode(y);

			// assert
			Assert.AreEqual(expected, hashX == hashY);
		}

		[TestCaseSource(nameof(IntTestCaseDatas))]
		public void GetHashCode_IntArray_ReturnsValidArray(int[] x, int[] y, bool expected)
		{
			// arrange
			var comparer = new ArrayOfStructuresMemoryEqualityComparer<int>();

			// act
			var hashX = comparer.GetHashCode(x);
			var hashY = comparer.GetHashCode(y);

			// assert
			Assert.AreEqual(expected, hashX == hashY);
		}

		[TestCaseSource(nameof(IntTestCaseDatas))]
		public void Equals_IntArray_ReturnsValidArray(int[] x, int[] y, bool expected)
		{
			// arrange
			var comparer = new ArrayOfStructuresMemoryEqualityComparer<int>();

			// act
			var actual = comparer.Equals(x, y);

			// assert
			Assert.AreEqual(expected, actual);
		}

		[TestCaseSource(nameof(DoubleTestCaseDatas))]
		public void GetHashCode_DoubleArray_ReturnsValidArray(double[] x, double[] y, bool expected)
		{
			// arrange
			var comparer = new ArrayOfStructuresMemoryEqualityComparer<double>();

			// act
			var hashX = comparer.GetHashCode(x);
			var hashY = comparer.GetHashCode(y);

			// assert
			Assert.AreEqual(expected, hashX == hashY);
		}

		[TestCaseSource(nameof(DoubleTestCaseDatas))]
		public void Equals_DoubleArray_ReturnsValidArray(double[] x, double[] y, bool expected)
		{
			// arrange
			var comparer = new ArrayOfStructuresMemoryEqualityComparer<double>();

			// act
			var actual = comparer.Equals(x, y);

			// assert
			Assert.AreEqual(expected, actual);
		}

		public static IEnumerable<TestCaseData> ByteTestCaseDatas
		{
			get
			{
				var largeArray = new byte[1024 * 1024];
				new Random(Guid.NewGuid().GetHashCode()).NextBytes(largeArray);

				yield return new TestCaseData(
					new byte[] { },
					new byte[] { },
					true);
				yield return new TestCaseData(
					new byte[] { 1 },
					new byte[] { 1 },
					true);
				yield return new TestCaseData(
					new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					true);
				yield return new TestCaseData(
					new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					new byte[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					false);
				yield return new TestCaseData(
					new byte[] { 255, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					false);
				yield return new TestCaseData(
					new byte[] {  },
					new byte[] { 1 },
					false);
				yield return new TestCaseData(
					largeArray,
					largeArray.Select(x => x).ToArray(), //clone
					true);
			}
		}

		public static IEnumerable<TestCaseData> IntTestCaseDatas
		{
			get
			{
				yield return new TestCaseData(
					new int[] { },
					new int[] { },
					true);
				yield return new TestCaseData(
					new int[] { 1 },
					new int[] { 1 },
					true);
				yield return new TestCaseData(
					new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					true);
				yield return new TestCaseData(
					new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					new int[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					false);
				yield return new TestCaseData(
					new int[] { 255, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					false);
				yield return new TestCaseData(
					new int[] { },
					new int[] { 1 },
					false);
			}
		}

		public static IEnumerable<TestCaseData> DoubleTestCaseDatas
		{
			get
			{
				yield return new TestCaseData(
					new double[] { },
					new double[] { },
					true);
				yield return new TestCaseData(
					new double[] { 1 },
					new double[] { 1 },
					true);
				yield return new TestCaseData(
					new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					true);
				yield return new TestCaseData(
					new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					new double[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					false);
				yield return new TestCaseData(
					new double[] { 255, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
					false);
				yield return new TestCaseData(
					new double[] { },
					new double[] { 1 },
					false);
			}
		}
	}
}