using System;
using CodeMania.Core.Maths;
using NUnit.Framework;

namespace CodeMania.UnitTests.Numbers
{
	[TestFixture]
	public class NumberTests
	{
		[TestCase(0, 0)]
		[TestCase(1, 2)]
		[TestCase(2, 3)]
		[TestCase(-1, -1)]
		[TestCase(int.MaxValue, 1)]
		[TestCase(int.MinValue, -1)]
		public static void TestAdd_Int_Int(int x, int y)
		{
			Numeric<int> numericX = x;
			Numeric<int> numericY = y;

			int actual = numericX + numericY;

			Assert.AreEqual(x + y, actual);
		}

		[TestCase(false, false)]
		[TestCase(false, true )]
		[TestCase(true,  false)]
		[TestCase(true,  true )]
		public static void TestAnd_Boolean_Boolean(bool x, bool y)
		{
			Numeric<bool> vX = x;
			Numeric<bool> vY = y;

			bool actual = vX & vY;

			Assert.AreEqual(x & y, actual);
		}

		[TestCase(false, false)]
		[TestCase(false, true)]
		[TestCase(true, false)]
		[TestCase(true, true)]
		public static void TestAndAlso_Boolean_Boolean(bool x, bool y)
		{
			Numeric<bool> vX = x;
			Numeric<bool> vY = y;
		
			bool actual = vX && vY;
		
			Assert.AreEqual(x && y, actual);
		}

		[TestCase(false, false)]
		[TestCase(false, true)]
		[TestCase(true, false)]
		[TestCase(true, true)]
		public static void TestOr_Boolean_Boolean(bool x, bool y)
		{
			Numeric<bool> vX = x;
			Numeric<bool> vY = y;

			bool actual = vX | vY;

			Assert.AreEqual(x | y, actual);
		}

		[TestCase(false, false)]
		[TestCase(false, true)]
		[TestCase(true, false)]
		[TestCase(true, true)]
		public static void TestOrElse_Boolean_Boolean(bool x, bool y)
		{
			Numeric<bool> vX = x;
			Numeric<bool> vY = y;

			bool actual = vX || vY;

			Assert.AreEqual(x || y, actual);
		}


		[TestCase(false, false)]
		[TestCase(false, true)]
		[TestCase(true, false)]
		[TestCase(true, true)]
		public static void TestXor_Boolean_Boolean(bool x, bool y)
		{
			Numeric<bool> vX = x;
			Numeric<bool> vY = y;

			bool actual = vX ^ vY;

			Assert.AreEqual(x ^ y, actual);
		}

		[Test]
		public static void TestUnsupportedOperators_Boolean()
		{
			object ignored = null;

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.Add));
			Assert.Throws<NotSupportedException>(() =>
			{
				ignored = new Numeric<bool>(true) + new Numeric<bool>(false);
			});

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.Subtract));
			Assert.Throws<NotSupportedException>(() =>
			{
				ignored = new Numeric<bool>(true) - new Numeric<bool>(false);
			});

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.Multiply));
			Assert.Throws<NotSupportedException>(() =>
			{
				ignored = new Numeric<bool>(true) * new Numeric<bool>(false);
			});

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.Divide));
			Assert.Throws<NotSupportedException>(() =>
			{
				ignored = new Numeric<bool>(true) / new Numeric<bool>(false);
			});

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.Modulo));
			Assert.Throws<NotSupportedException>(() =>
			{
				ignored = new Numeric<bool>(true) % new Numeric<bool>(false);
			});

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.BitwiseShiftLeft));
			Assert.Throws<NotSupportedException>(() =>
			{
				ignored = new Numeric<bool>(true) << 1;
			});

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.BitwiseShiftRight));
			Assert.Throws<NotSupportedException>(() =>
			{
				ignored = new Numeric<bool>(true) >> 1;
			});

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.GreaterThan));
			Assert.Throws<NotSupportedException>(() =>
			{
				ignored = new Numeric<bool>(true) > new Numeric<bool>(false);
			});

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.GreaterThanOrEqual));
			Assert.Throws<NotSupportedException>(() =>
			{
				ignored = new Numeric<bool>(true) >= new Numeric<bool>(false);
			});

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.LessThan));
			Assert.Throws<NotSupportedException>(() =>
			{
				ignored = new Numeric<bool>(true) < new Numeric<bool>(false);
			});

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.LessThanOrEqual));
			Assert.Throws<NotSupportedException>(() =>
			{
				ignored = new Numeric<bool>(true) <= new Numeric<bool>(false);
			});

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.Increment));
			Assert.Throws<NotSupportedException>(() =>
			{
				var d = new Numeric<bool>(true);
				ignored = d++;
			});

			Assert.IsFalse(Operations<bool>.IsOperatorSupported(Operation.Decrement));
			Assert.Throws<NotSupportedException>(() =>
			{
				var d = new Numeric<bool>(true);
				ignored = d--;
			});

			Assert.IsNull(ignored);
		}
	}
}