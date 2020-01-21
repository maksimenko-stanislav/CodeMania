using System;
using System.Linq;
using System.Linq.Expressions;
using CodeMania.Core.Internals;
using NUnit.Framework;

// ReSharper disable MultipleSpaces

namespace CodeMania.UnitTests.ExpressionsTests
{
	[TestFixture]
	public class EqualsExpressionsTests
	{
		[TestCase(typeof(int),    typeof(BinaryExpression),     typeof(Func<int, int, bool>))]
		[TestCase(typeof(int?),   typeof(MethodCallExpression), typeof(Func<int?, int?, bool>))]
		[TestCase(typeof(string), typeof(MethodCallExpression), typeof(Func<string, string, bool>))]
		[TestCase(typeof(int[]),  typeof(MethodCallExpression), typeof(Func<int[], int[], bool>))]
		public void CreateEqualsExpressions_PassItemType_ProducesValidExpressionFor(
			Type itemType, Type expectedBodyType, Type expectedDelegateType)
		{
			LambdaExpression expression = EqualsExpressions.CreateEqualsExpression(itemType);

			Assert.AreEqual(2, expression.Parameters.Count);
			Assert.IsTrue(expression.Parameters.All(p => p.Type == itemType));
			Assert.AreEqual(typeof(bool), expression.ReturnType);
			Assert.IsTrue(expectedBodyType.IsInstanceOfType(expression.Body));
			Assert.AreEqual(expectedDelegateType, expression.Compile().GetType());
		}
	}
}