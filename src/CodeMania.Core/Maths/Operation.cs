namespace CodeMania.Core.Maths
{
	public enum Operation
	{
		// unused
		Unknown,

		// binary
		Add,
		Subtract,
		Multiply,
		Divide,
		Modulo,
		Equals,
		NotEquals,
		GreaterThan,
		GreaterThanOrEqual,
		LessThan,
		LessThanOrEqual,
		And,
		Or,
		Xor,
		AndAlso,
		OrElse,
		BitwiseAnd,
		BitwiseOr,
		BitwiseXor,
		BitwiseShiftLeft,
		BitwiseShiftRight,

		// unary
		UnaryPlus,
		UnaryMinus,
		Negate,
		BitwiseComplement,
		Increment,
		Decrement,

		// true/false
		True,
		False
	}
}