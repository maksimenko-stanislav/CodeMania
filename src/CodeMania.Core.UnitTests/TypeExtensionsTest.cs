using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CodeMania.Core.Extensions;
using NUnit.Framework;

// ReSharper disable ConvertNullableToShortForm
// ReSharper disable InvokeAsExtensionMethod

namespace CodeMania.UnitTests
{
	public enum TestEnum
	{
		Value0,
		Value1,
		Value2,
	}

	public class DtoSample
	{
		public List<TestEnum> TestEnums { get; set; }
	}

	public static class ByteArrayMemoryEqualityComparerTestCaseSources
	{
		private static readonly byte[] SampleBytes = Enumerable.Range(0, new Random(Guid.NewGuid().GetHashCode()).Next(1, 1024)).Select(x => (byte) x).ToArray();

		public static IEnumerable TestCases
		{
			get
			{
				yield return new TestCaseData(default(byte[]), default(byte[]), true);
				yield return new TestCaseData(new byte[0], new byte[0], true);
				yield return new TestCaseData(new byte[1], new byte[1], true);
				yield return new TestCaseData(new byte[2], new byte[2], true);
				yield return new TestCaseData(new byte[short.MaxValue], new byte[short.MaxValue], true);
				yield return new TestCaseData(new byte[] {1, 2, 3}, new byte[] {1, 2, 3}, true);
				yield return new TestCaseData(SampleBytes, SampleBytes, true);

				yield return new TestCaseData(new byte[short.MaxValue], default(byte[]), false);
				yield return new TestCaseData(default(byte[]), new byte[short.MaxValue], false);
				yield return new TestCaseData(new byte[0], new byte[1], false);
				yield return new TestCaseData(new byte[1], new byte[0], false);
				yield return new TestCaseData(new byte[1], new byte[0], false);
				yield return new TestCaseData(new byte[short.MaxValue], new byte[1], false);
				yield return new TestCaseData(new byte[1], new byte[short.MaxValue], false);
				yield return new TestCaseData(new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 2 }, false);
			}
		}
	}

	[TestFixture]
	public class TypeExtensionsTest
	{
		private sealed class IntList : List<int>
		{
		}

		[Test]
		[TestCase(typeof(List<int>),	      typeof(IEnumerable<>), true)]
		[TestCase(typeof(List<int>),	      typeof(ICollection<>), true)]
		[TestCase(typeof(List<int>),	      typeof(IList<>),       true)]
		[TestCase(typeof(List<int>),	      typeof(List<>),        true)]
		[TestCase(typeof(List<int>),	      typeof(ISet<>),        false)]
		[TestCase(typeof(List<int>),          typeof(Collection<>),  false)]
		[TestCase(typeof(List<string>),       typeof(IEnumerable<>), true)]
		[TestCase(typeof(List<string>),       typeof(ICollection<>), true)]
		[TestCase(typeof(List<string>),       typeof(IList<>),       true)]
		[TestCase(typeof(List<string>),       typeof(List<>),        true)]
		[TestCase(typeof(List<string>),       typeof(ISet<>),        false)]
		[TestCase(typeof(List<string>),       typeof(Collection<>),  false)]
		[TestCase(typeof(List<>),		      typeof(IEnumerable<>), true)]
		[TestCase(typeof(List<>),		      typeof(ICollection<>), true)]
		[TestCase(typeof(List<>),		      typeof(IList<>),       true)]
		[TestCase(typeof(List<>),		      typeof(List<>),        false)]
		[TestCase(typeof(List<>),		      typeof(ISet<>),        false)]
		[TestCase(typeof(List<>),             typeof(Collection<>),  false)]
		[TestCase(typeof(Nullable<>),         typeof(Nullable<>),    false)]
		[TestCase(typeof(Nullable<int>),      typeof(Nullable<>),    true)]
		[TestCase(typeof(Nullable<FileMode>), typeof(Nullable<>),    true)]
		[TestCase(typeof(IntList),			  typeof(IEnumerable<>), true)]
		[TestCase(typeof(IntList),			  typeof(ICollection<>), true)]
		[TestCase(typeof(IntList),			  typeof(IList<>),       true)]
		[TestCase(typeof(IntList),			  typeof(List<>),        true)]
		[TestCase(typeof(IntList),			  typeof(ISet<>),        false)]
		[TestCase(typeof(IntList),			  typeof(Collection<>),  false)]
		public void IsGenericAssignable_ReturnsTrueForGenericClass(Type testType, Type openGenericType, bool isAssignable)
		{
			Assert.AreEqual(isAssignable, testType.IsGenericAssignable(openGenericType));
		}

		[Test]
		public void IsGenericAssignable_ThrowArgumentNullExceptionOnNullArguments()
		{
			Assert.Throws<ArgumentNullException>(() => TypeExtensions.IsGenericAssignable(null, typeof(IEnumerable<>)));
			Assert.Throws<ArgumentNullException>(() => TypeExtensions.IsGenericAssignable(typeof(IList<int>), null));
		}

		[Test]
		public void IsGenericAssignable_ThrowArgumentExceptionOnNonOpenGenericType()
		{
			Assert.Throws<ArgumentException>(() => TypeExtensions.IsGenericAssignable(typeof(IList<int>), typeof(IEnumerable<int>)));
		}
	}
}
