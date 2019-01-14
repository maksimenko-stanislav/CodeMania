//using CodeMania.Core.Serialization;
//using NUnit.Framework;

//// ReSharper disable StringLiteralTypo

//namespace CodeMania.UnitTests.Serialization
//{
//	[TestFixture]
//	public class QueryStringReaderTests
//	{
//		[TestCase('&', "")]
//		[TestCase('&', "?")]
//		[TestCase('=', "?id=1", "id", "1")]
//		[TestCase('=', "?id=1#details.html", "id", "1")]
//		[TestCase('=', "id=1", "id", "1")]
//		[TestCase('=', "id=1#index.html", "id", "1")]
//		[TestCase('&', "?id=1&id=2", "id=1", "id=2")]
//		[TestCase('&', "?id=1&id=2#", "id=1", "id=2")]
//		[TestCase('&', "id=1&id=2", "id=1", "id=2")]
//		[TestCase('&', "id=1&id=2#hoe.html", "id=1", "id=2")]
//		[TestCase('=', "?value=0.001", "value", "0.001")]
//		[TestCase('=', "value=0.001", "value", "0.001")]
//		[TestCase('=', "str=Hello%20World!", "str", "Hello%20World!")]
//		[TestCase('=', "str=Hello+World!", "str", "Hello+World!")]
//		public void ReadWhile_ReturnsExpectedSections(char separator, string source, params string[] sections)
//		{
//			var reader = new QueryStringReader(source);

//			if (sections != null)
//			{
//				for (int j = 0; j < sections.Length; j++)
//				{
//					Assert.AreEqual(sections[j], reader.ReadWhile(separator));
//				}
//			}
//		}

//		[TestCase("", "", "")]
//		[TestCase("?", "", "")]
//		[TestCase("#", "", "")]
//		[TestCase("https://127.0.01:12345/ui#details.html", "", "")]
//		[TestCase("https://127.0.01:12345/ui?id=123456#details.html", "id", "123456")]
//		public void ReadPropertyPairs_ReturnsExpectedValues(string source, string propertyName, string propertyValue)
//		{
//			var reader = new QueryStringReader(source);

//			Assert.AreEqual(propertyName, reader.ReadPropertyName());
//			Assert.AreEqual(propertyValue, reader.ReadValue());
//		}
//	}
//}