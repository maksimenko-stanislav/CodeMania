using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CodeMania.UnitTests.EqualityComparers
{
	[TestFixture]
	public abstract class EqualityComparerTestsBase<T>
	{
		public class TestCase : TestCaseData
		{
			public TestCase(T first, T second, bool areEquals) : base(first, second, areEquals)
			{
				Returns(areEquals);
			}

			public T First => (T) Arguments[0];
			public T Second => (T) Arguments[1];
			public bool AreEquals => (bool) Arguments[2];
		}

		protected readonly IEqualityComparer<T> EqualityComparer;

		private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		};

		protected abstract IEnumerable<TestCase> GetTestCases();

		protected EqualityComparerTestsBase(IEqualityComparer<T> equalityComparer)
		{
			this.EqualityComparer = equalityComparer;
		}

		[Test]
		public void Equals_ReturnsExpectedResult()
		{
			foreach (var testCase in GetTestCases())
			{
				Equals_ReturnsExpectedResult(testCase.First, testCase.Second, testCase.AreEquals);
			}
		}

		public void Equals_ReturnsExpectedResult(T x, T y, bool expected)
		{
			// act
			var actual = EqualityComparer.Equals(x, y);

			// assert
			Assert.AreEqual(expected, actual, "Unexpected result:\r\nx:\r\n{0}\r\n\r\ny:\r\n{1}",
				JsonConvert.SerializeObject(x, Formatting.Indented, jsonSerializerSettings),
				JsonConvert.SerializeObject(y, Formatting.Indented, jsonSerializerSettings));
		}

		[Test]
		public void Equals_IsTransitive()
		{
			foreach (var testCase in GetTestCases())
			{
				Equals_IsTransitive(testCase.First, testCase.Second, testCase.AreEquals);
			}
		}

		public void Equals_IsTransitive(T x, T y, bool expected)
		{
			// act
			var actual = EqualityComparer.Equals(x, y);

			// assert
			Assert.AreEqual(EqualityComparer.Equals(y, x), actual, "Unexpected result:\r\nx:\r\n{0}\r\n\r\ny:\r\n{1}",
				JsonConvert.SerializeObject(x, Formatting.Indented, jsonSerializerSettings),
				JsonConvert.SerializeObject(y, Formatting.Indented, jsonSerializerSettings));
		}

		[Test]
		public void GetHashCode_ReturnsValidValues()
		{
			foreach (var testCase in GetTestCases())
			{
				GetHashCode_ReturnsValidValues(testCase.First, testCase.Second, testCase.AreEquals);
			}
		}

		public void GetHashCode_ReturnsValidValues(T x, T y, bool expected)
		{
			// act
			var hashX = EqualityComparer.GetHashCode(x);
			var hashY = EqualityComparer.GetHashCode(y);

			// assert
			Assert.AreEqual(expected, hashX == hashY, "Unexpected result:\r\nx:\r\n{0}\r\n\r\ny:\r\n{1}",
				JsonConvert.SerializeObject(x, Formatting.Indented, jsonSerializerSettings),
				JsonConvert.SerializeObject(y, Formatting.Indented, jsonSerializerSettings));
		}

		[Test]
		public void GetHashCode_MultipleCallsProducesSameHashCode()
		{
			foreach (var testCase in GetTestCases())
			{
				GetHashCode_MultipleCallsProducesSameHashCode(testCase.First, testCase.AreEquals);
			}
		}

		public void GetHashCode_MultipleCallsProducesSameHashCode(T x, bool expected)
		{
			if (x == null) return;

			// arrange
			const int iterationCount = 1000;

			var hashCodes = new List<int>(iterationCount);

			// act
			for (int i = 0; i < iterationCount; i++)
			{
				hashCodes.Add(EqualityComparer.GetHashCode(x));
			}

			// assert
			Assert.IsTrue(hashCodes.Distinct().Count() == 1);
		}

		protected static TestCase Create(Func<T> getFirst, Func<T, T> getSecond, Func<T, T, bool> getAreEquals)
		{
			T t1 = getFirst();
			T t2 = getSecond(t1);

			bool t3 = getAreEquals(t1, t2);

			return new TestCase(t1, t2, t3);
		}

		protected static TestCase Create(T first, T second, bool areEquals)
		{
			return new TestCase(first, second, areEquals);
		}
	}
}