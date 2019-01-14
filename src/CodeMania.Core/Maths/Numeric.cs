using System;

namespace CodeMania.Core.Maths
{
	public struct Numeric<T> : IEquatable<Numeric<T>>, IFormattable
	{
		#region Fields

		private readonly T value;

		#endregion

		#region .ctor

		public Numeric(T value)
		{
			this.value = value;
		}

		#endregion

		#region Cast operators

		public static implicit operator Numeric<T>(T value) => new Numeric<T>(value);

		public static implicit operator T(Numeric<T> value) => value.value;

		public static explicit operator double(Numeric<T> value) => UnsafeDynamicCast<T, double>.Cast(value.value);

		public static explicit operator Numeric<T>(double value) => UnsafeDynamicCast<double, T>.Cast(value);

		#endregion

		#region +, -, *, /, %

		public static Numeric<T> operator +(Numeric<T> x, Numeric<T> y) => Operations<T>.Add(x, y);

		public static Numeric<T> operator -(Numeric<T> x, Numeric<T> y) => Operations<T>.Subtract(x, y);

		public static Numeric<T> operator *(Numeric<T> x, Numeric<T> y) => Operations<T>.Multiply(x, y);

		public static Numeric<T> operator /(Numeric<T> x, Numeric<T> y) => Operations<T>.Divide(x, y);

		public static Numeric<T> operator %(Numeric<T> x, Numeric<T> y) => Operations<T>.Modulo(x, y);

		#endregion

		#region ==, !==, >, >=, <, <=

		public static bool operator ==(Numeric<T> x, Numeric<T> y) => Operations<T>.Equals(x.value, y.value);

		public static bool operator !=(Numeric<T> x, Numeric<T> y) => Operations<T>.NotEquals(x.value, y.value);

		public static bool operator >(Numeric<T> x, Numeric<T> y) => Operations<T>.GreaterThan(x.value, y.value);

		public static bool operator >=(Numeric<T> x, Numeric<T> y) => Operations<T>.GreaterThanOrEqual(x.value, y.value);

		public static bool operator <(Numeric<T> x, Numeric<T> y) => Operations<T>.LessThan(x.value, y.value);

		public static bool operator <=(Numeric<T> x, Numeric<T> y) => Operations<T>.LessThanOrEqual(x.value, y.value);

		#endregion

		#region Bitwise &, |, ^, <<, >>

		public static Numeric<T> operator &(Numeric<T> x, Numeric<T> y) => Operations<T>.BitwiseAnd(x, y);

		public static Numeric<T> operator |(Numeric<T> x, Numeric<T> y) => Operations<T>.BitwiseOr(x, y);

		public static Numeric<T> operator ^(Numeric<T> x, Numeric<T> y) => Operations<T>.BitwiseXor(x, y);

		public static Numeric<T> operator <<(Numeric<T> x, int shift) => Operations<T>.BitwiseShiftLeft(x, shift);

		public static Numeric<T> operator >>(Numeric<T> x, int shift) => Operations<T>.BitwiseShiftRight(x, shift);

		#endregion

		#region Logical &, |, ^

		public static bool operator &(Numeric<T> x, bool y) => Operations<T>.LogicalAnd(x.value, y);

		public static bool operator |(Numeric<T> x, bool y) => Operations<T>.LogicalOr(x.value, y);

		public static bool operator ^(Numeric<T> x, bool y) => Operations<T>.LogicalXor(x.value, y);

		#endregion

		#region Unary +, -, !, ~, ++, --

		public static Numeric<T> operator + (Numeric<T> x) => Operations<T>.UnaryPlus(x.value);

		public static Numeric<T> operator - (Numeric<T> x) => Operations<T>.UnaryMinus(x.value);

		public static Numeric<T> operator ! (Numeric<T> x) => Operations<T>.Negate(x.value);

		public static Numeric<T> operator ~ (Numeric<T> x) => Operations<T>.BitwiseComplement(x.value);

		public static Numeric<T> operator ++(Numeric<T> x) => Operations<T>.Increment(x.value);

		public static Numeric<T> operator --(Numeric<T> x) => Operations<T>.Decrement(x.value);

		#endregion

		#region True/False operators

		public static bool operator true (Numeric<T> numeric) => Operations<T>.IsTrue(numeric.value);

		public static bool operator false(Numeric<T> numeric) => Operations<T>.IsFalse(numeric.value);

		#endregion

		#region Methods

		public bool Equals(Numeric<T> other) => Operations<T>.Equals(value, other.value);

		public override bool Equals(object obj) => obj is Numeric<T> numeric && Equals(numeric);

		public override int GetHashCode() => Operations<T>.GetHashCode(value);

		public override string ToString() => ToString(null, null);

		public string ToString(string format) => ToString(format, null);

		public string ToString(string format, IFormatProvider formatProvider) =>
			Operations<T>.ToString(value, format, formatProvider);

		#endregion
	}
}