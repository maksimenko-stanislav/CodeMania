using System;
using System.Collections.Generic;
using CodeMania.Core.Serialization;
using NUnit.Framework;

namespace CodeMania.UnitTests.Serialization
{
	[TestFixture]
	public class QueryStringTokenizerTests
	{
		public sealed class TestCase : TestCaseData
		{
			public TestCase(string arg0, Dictionary<string, IList<string>> expectedResult) : base(arg0, expectedResult)
			{
			}
		}

		public static IEnumerable<TestCase> TestCases
		{
			get
			{
				yield return new TestCase(
					"",
					new Dictionary<string, IList<string>>(StringComparer.Ordinal));
				yield return new TestCase(
					"id=1&",
					new Dictionary<string, IList<string>>(StringComparer.Ordinal)
					{
						{ "id", new[] { "1" } }
					});
				yield return new TestCase(
					"id=1&name=First Last",
					new Dictionary<string, IList<string>>(StringComparer.Ordinal)
					{
						{ "id", new[] { "1" } },
						{ "name", new[] { "First Last" } }
					});
				yield return new TestCase(
					"ids=1&name=First Last&ids=2",
					new Dictionary<string, IList<string>>(StringComparer.Ordinal)
					{
						{ "ids", new[] { "1", "2" } },
						{ "name", new[] { "First Last" } }
					});
				yield return new TestCase(
					"?ids=1&name=First Last&ids=2",
					new Dictionary<string, IList<string>>(StringComparer.Ordinal)
					{
						{ "ids", new[] { "1", "2" } },
						{ "name", new[] { "First Last" } }
					});
				yield return new TestCase(
					"?ids=1&name=First Last&ids=2#p1.html",
					new Dictionary<string, IList<string>>(StringComparer.Ordinal)
					{
						{ "ids", new[] { "1", "2" } },
						{ "name", new[] { "First Last" } }
					});
				yield return new TestCase(
					"https://host:12345/api/service?ids=1&name=First+Last&ids=2#p1.html",
					new Dictionary<string, IList<string>>(StringComparer.Ordinal)
					{
						{ "ids", new[] { "1", "2" } },
						{ "name", new[] { "First+Last" } }
					});
				yield return new TestCase(
					"https://host:12345/api/service?wsdl",
					new Dictionary<string, IList<string>>(StringComparer.Ordinal)
					{
						{ "wsdl", new[] { "" } }
					});
				yield return new TestCase(
					"https://host:12345/api/service?=wsdl",
					new Dictionary<string, IList<string>>(StringComparer.Ordinal)
					{
						{ "", new[] { "wsdl" } }
					});
				yield return new TestCase(
					"https://host:12345/api/service?&k1=v1&&k2=v2",
					new Dictionary<string, IList<string>>(StringComparer.Ordinal)
					{
						{ "k1", new[] { "v1" } },
						{ "k2", new[] { "v2" } },
					});
				yield return new TestCase(
					"&id=1&&==&&==&&&=#",
					new Dictionary<string, IList<string>>(StringComparer.Ordinal)
					{
						{ "id", new[] { "1" } }
					});
				yield return new TestCase(
					"&id1=1&&==&&==&&&=&&======&id2=2.01&isTest=true&#",
					new Dictionary<string, IList<string>>(StringComparer.Ordinal)
					{
						{ "id1", new[] { "1" } },
						{ "id2", new[] { "2.01" } },
						{ "isTest", new[] { "true" } },
					});
			}
		}

		[Test]
		public void Constructor_PassNullString_ThrowsArgumentNullException()
		{
			Assert.That(() => new QueryStringTokenizer(null), Throws.ArgumentNullException);
		}

		[TestCaseSource(nameof(TestCases))]
		public void Read_PassQueryString_ReturnsExpectedKeysAndValues(string queryString, Dictionary<string, IList<string>> expectedResult)
		{
			var actualTokens = new Dictionary<string, IList<string>>(StringComparer.Ordinal);

			var tokenizer = new QueryStringTokenizer(queryString);

			ReadOnlyMemory<char> key, value;
			while (tokenizer.Read(out key, out value))
			{
				IList<string> values;
				if (!actualTokens.TryGetValue(key.ToString(), out values))
				{
					values = new List<string>();
					actualTokens.Add(key.ToString(), values);
				}

				values.Add(value.ToString());
			}

			AssertThatDictionariesContainsSameContent(expectedResult, actualTokens);
		}

		private static void AssertThatDictionariesContainsSameContent(Dictionary<string, IList<string>> expected, Dictionary<string, IList<string>> actual)
		{
			foreach (var pair in expected)
			{
				Assert.IsTrue(actual.ContainsKey(pair.Key), "actual.ContainsKey(pair.Key)");

				var actualValues = actual[pair.Key];

				foreach (var actualValue in actualValues)
				{
					Assert.IsTrue(pair.Value.Contains(actualValue), "pair.Value.Contains(actualValue)");
				}
			}
		}
	}
}