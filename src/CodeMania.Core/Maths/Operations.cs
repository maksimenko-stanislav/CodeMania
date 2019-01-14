using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using CodeMania.Core.Utils;

// ReSharper disable MultipleSpaces

namespace CodeMania.Core.Maths
{
	public static class Operations<T1, T2, TResult>
	{
		public static readonly Func<T1, T2, TResult> AddFunc;
		public static readonly Func<T1, T2, TResult> SubtractFunc;
		public static readonly Func<T1, T2, TResult> MultiplyFunc;
		public static readonly Func<T1, T2, TResult> DivideFunc;

		static Operations()
		{
			var x = Expression.Parameter(typeof(T1), "x");
			var y = Expression.Parameter(typeof(T2), "y");

			AddFunc      = ExpressionUtil.CreateBinaryMethod<T1, T2, TResult>(x, y, Expression.Add,      GetUnsupportedOperationMethod("+"));
			SubtractFunc = ExpressionUtil.CreateBinaryMethod<T1, T2, TResult>(x, y, Expression.Subtract, GetUnsupportedOperationMethod("-"));
			MultiplyFunc = ExpressionUtil.CreateBinaryMethod<T1, T2, TResult>(x, y, Expression.Multiply, GetUnsupportedOperationMethod("*"));
			DivideFunc   = ExpressionUtil.CreateBinaryMethod<T1, T2, TResult>(x, y, Expression.Divide,   GetUnsupportedOperationMethod("/"));
		}

		private static string GetUnsupportedOperationMethod(string operation)
			=> $"Operator '{operation}' is not supported for types {typeof(T1)} and {typeof(T2)}.";
	}

	public static class Operations<T>
	{
		#region Fields

		// ReSharper disable once StaticMemberInGenericType
		private static readonly Dictionary<Operation, bool> SupportedOperators =
			new Dictionary<Operation, bool>(EqualityComparer<Operation>.Default);

		private static readonly Func<T, T, T>       AddFunc;
		private static readonly Func<T, T, T>       SubtractFunc;
		private static readonly Func<T, T, T>       MultiplyFunc;
		private static readonly Func<T, T, T>       DivideFunc;
		private static readonly Func<T, T, T>       ModuloFunc;
		private static readonly Func<T, T, T>       BitwiseAndFunc;
		private static readonly Func<T, T, T>       BitwiseOrFunc;
		private static readonly Func<T, T, T>       BitwiseXorFunc;
		private static readonly Func<T, int, T>     BitwiseShiftLeftFunc;
		private static readonly Func<T, int, T>     BitwiseShiftRightFunc;
		private static readonly Func<T, T, bool>    EqualsFunc;
		private static readonly Func<T, T, bool>    NotEqualsFunc;
		private static readonly Func<T, T, bool>    GreaterThanFunc;
		private static readonly Func<T, T, bool>    GreaterThanOrEqualFunc;
		private static readonly Func<T, T, bool>    LessThanFunc;
		private static readonly Func<T, T, bool>    LessThanOrEqualFunc;
		private static readonly Func<T, bool, bool> LogicalAndFunc;
		private static readonly Func<T, bool, bool> LogicalOrFunc;
		private static readonly Func<T, bool, bool> LogicalXorFunc;
		private static readonly Func<T, bool, bool> LogicalAndAlsoFunc;
		private static readonly Func<T, bool, bool> LogicalOrElseFunc;
		private static readonly Func<T, T>          UnaryPlusFunc;
		private static readonly Func<T, T>          UnaryMinusFunc;
		private static readonly Func<T, T>          NegateFunc;
		private static readonly Func<T, T>          BitwiseComplementFunc;
		private static readonly Func<T, T>          IncrementFunc;
		private static readonly Func<T, T>          DecrementFunc;
		private static readonly Func<T, bool>       TrueFunc;
		private static readonly Func<T, bool>       FalseFunc;

		private static readonly Func<T, int> GetHashCodeFunc;
		private static readonly Func<T, string, IFormatProvider, string> ToStringFunc;

		#endregion

		#region .cctor

		static Operations()
		{
			var x     = Expression.Parameter(typeof(T),    "x");
			var y     = Expression.Parameter(typeof(T),    "y");
			var shift = Expression.Parameter(typeof(int),  "shift");
			var boolY = Expression.Parameter(typeof(bool), "boolY");

			var paramsX      = new[] { x };
			var paramsXY     = new[] { x, y };
			var paramsXShift = new[] { x, shift };
			var paramsXBoolY = new[] { x, boolY };

			// TODO: Refactor to use ExpressionUtil instead GetCompiledFunc

			#region +, -, *, /, %

			GetCompiledFunc(out AddFunc,      Operation.Add,      paramsXY, () => Expression.Add     (x, y));
			GetCompiledFunc(out SubtractFunc, Operation.Subtract, paramsXY, () => Expression.Subtract(x, y));
			GetCompiledFunc(out MultiplyFunc, Operation.Multiply, paramsXY, () => Expression.Multiply(x, y));
			GetCompiledFunc(out DivideFunc,   Operation.Divide,   paramsXY, () => Expression.Divide  (x, y));
			GetCompiledFunc(out ModuloFunc,   Operation.Modulo,   paramsXY, () => Expression.Modulo  (x, y));

			#endregion

			#region ==, !==, >, >=, <, <=

			GetCompiledFunc(out EqualsFunc,             Operation.Equals,             paramsXY, () => Expression.Equal             (x, y));
			GetCompiledFunc(out NotEqualsFunc,          Operation.NotEquals,          paramsXY, () => Expression.NotEqual          (x, y));
			GetCompiledFunc(out GreaterThanFunc,        Operation.GreaterThan,        paramsXY, () => Expression.GreaterThan       (x, y));
			GetCompiledFunc(out GreaterThanOrEqualFunc, Operation.GreaterThanOrEqual, paramsXY, () => Expression.GreaterThanOrEqual(x, y));
			GetCompiledFunc(out LessThanFunc,           Operation.LessThan,           paramsXY, () => Expression.LessThan          (x, y));
			GetCompiledFunc(out LessThanOrEqualFunc,    Operation.LessThanOrEqual,    paramsXY, () => Expression.LessThanOrEqual   (x, y));

			#endregion

			#region Bitwise &, |, ^, <<, >>

			GetCompiledFunc(out BitwiseAndFunc, Operation.BitwiseAnd, paramsXY, () => Expression.And        (x, y));
			GetCompiledFunc(out BitwiseOrFunc,  Operation.BitwiseOr,  paramsXY, () => Expression.Or         (x, y));
			GetCompiledFunc(out BitwiseXorFunc, Operation.BitwiseXor, paramsXY, () => Expression.ExclusiveOr(x, y));

			GetCompiledFunc(out BitwiseShiftLeftFunc,  Operation.BitwiseShiftLeft,  paramsXShift, () => Expression.LeftShift (x, shift));
			GetCompiledFunc(out BitwiseShiftRightFunc, Operation.BitwiseShiftRight, paramsXShift, () => Expression.RightShift(x, shift));

			#endregion

			#region Logical &, |, ^, &&, ||

			GetCompiledFunc(out LogicalAndFunc,     Operation.And,     paramsXBoolY, () => Expression.And        (x, boolY));
			GetCompiledFunc(out LogicalOrFunc,      Operation.Or,      paramsXBoolY, () => Expression.Or         (x, boolY));
			GetCompiledFunc(out LogicalXorFunc,     Operation.Xor,     paramsXBoolY, () => Expression.ExclusiveOr(x, boolY));
			GetCompiledFunc(out LogicalAndAlsoFunc, Operation.AndAlso, paramsXBoolY, () => Expression.AndAlso    (x, boolY));
			GetCompiledFunc(out LogicalOrElseFunc,  Operation.OrElse,  paramsXBoolY, () => Expression.OrElse     (x, boolY));

			#endregion

			#region Unary +, -, !, ~, ++, --

			GetCompiledFunc(out UnaryPlusFunc,         Operation.UnaryPlus,         paramsX, () => Expression.UnaryPlus     (x));
			GetCompiledFunc(out UnaryMinusFunc,        Operation.UnaryMinus,        paramsX, () => Expression.Negate        (x));
			GetCompiledFunc(out NegateFunc,            Operation.Negate,            paramsX, () => Expression.Negate        (x));
			GetCompiledFunc(out BitwiseComplementFunc, Operation.BitwiseComplement, paramsX, () => Expression.OnesComplement(x));
			GetCompiledFunc(out IncrementFunc,         Operation.Increment,         paramsX, () => Expression.Increment     (x));
			GetCompiledFunc(out DecrementFunc,         Operation.Decrement,         paramsX, () => Expression.Decrement     (x));

			#endregion

			#region True/False operators

			GetCompiledFunc(out TrueFunc,  Operation.True,  paramsX, () => Expression.IsTrue (x));
			GetCompiledFunc(out FalseFunc, Operation.False, paramsX, () => Expression.IsFalse(x));

			#endregion

			#region GetHashCode, ToString

			var getHashCodeMethod = typeof(T).GetMethod(nameof(GetHashCode),
				BindingFlags.Public | BindingFlags.InvokeMethod,
				null, new Type[0], new ParameterModifier[0]);

			if (typeof(T).IsValueType && getHashCodeMethod != null)
			{
				GetHashCodeFunc = ExpressionCompiler.Default.Compile(
					Expression.Lambda<Func<T, int>>(
						Expression.Call(x, getHashCodeMethod),
						x
				));
			}
			else
			{
				GetHashCodeFunc = obj => EqualityComparer<T>.Default.GetHashCode(obj);
			}

			var toStringMethod = typeof(T).GetMethod(nameof(ToString),
				BindingFlags.Public | BindingFlags.Instance,
				null, new[] {typeof(string), typeof(IFormatProvider)}, new ParameterModifier[0]);

			if (toStringMethod != null)
			{
				var format = Expression.Parameter(typeof(string), "format");
				var formatProvider = Expression.Parameter(typeof(IFormatProvider), "formatProvider");

				ToStringFunc = ExpressionCompiler.Default.Compile(
					Expression.Lambda<Func<T, string, IFormatProvider, string>>(
						Expression.Call(x, toStringMethod, format, formatProvider),
						x, format, formatProvider
				));
			}
			else
			{
				ToStringFunc = (obj, format, formatProvider) => obj?.ToString();
			}

			#endregion
		}

		#endregion

		#region Methods

		public static bool IsOperatorSupported(Operation op) =>
			SupportedOperators.ContainsKey(op) && SupportedOperators[op];

		private static void GetCompiledFunc<TDelegate>(out TDelegate result, Operation op,
			ParameterExpression[] parameters, Func<Expression> getBody)
			where TDelegate : class
		{
			try
			{
				Expression<TDelegate> expression = Expression.Lambda<TDelegate>(getBody(), parameters);

				result = ExpressionCompiler.Default.Compile(expression);

				SupportedOperators[op] = true;

				return;
			}
			catch (Exception)
			{
				// can't compile expression for type T.
				// ignored
			}

			// (args) => { throw new NotSupportedException(errorMessage); }
			var errorExpression = Expression.Lambda<TDelegate>(
				Expression.Block(
					parameters,
					Expression.Throw(
						Expression.New(
							// ReSharper disable once AssignNullToNotNullAttribute
							typeof(NotSupportedException).GetConstructor(new[] { typeof(string) }),
							Expression.Constant(GetUnsupportedOperatorErrorMessage(op.ToString()))
						)
					),
					// unreachable function result statement, because an exception is thrown above.
					// return default(T);
					Expression.Default(typeof(TDelegate).GetGenericArguments().Last())),
				parameters);

            result = ExpressionCompiler.Default.Compile(errorExpression);
        }

		private static string GetUnsupportedOperatorErrorMessage(string op) =>
			$"Type {typeof(T).FullName} doesn't contain operator '{op}'.";

		#region +, -, *, /, %

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Add(T x, T y) => AddFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Subtract(T x, T y) => SubtractFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Multiply(T x, T y) => MultiplyFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Divide(T x, T y) => DivideFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Modulo(T x, T y) => ModuloFunc(x, y);

		#endregion

		#region ==, !==, >, >=, <, <=

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Equals(T x, T y) => EqualsFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool NotEquals(T x, T y) => NotEqualsFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThan(T x, T y) => GreaterThanFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanOrEqual(T x, T y) => GreaterThanOrEqualFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThan(T x, T y) => LessThanFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanOrEqual(T x, T y) => LessThanOrEqualFunc(x, y);

		#endregion

		#region Bitwise &, |, ^, <<, >>

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T BitwiseAnd(T x, T y) => BitwiseAndFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T BitwiseOr(T x, T y) => BitwiseOrFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T BitwiseXor(T x, T y) => BitwiseXorFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T BitwiseShiftLeft(T x, int shift) => BitwiseShiftLeftFunc(x, shift);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T BitwiseShiftRight(T x, int shift) => BitwiseShiftRightFunc(x, shift);

		#endregion

		#region Logical &, |, ^, &&, ||

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LogicalAnd(T x, bool y) => LogicalAndFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LogicalOr(T x, bool y) => LogicalOrFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LogicalXor(T x, bool y) => LogicalXorFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LogicalAndAlso(T x, bool y) => LogicalAndAlsoFunc(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LogicalOrElse(T x, bool y) => LogicalOrElseFunc(x, y);

		#endregion

		#region Unary +, -, !, ~, ++, --

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T UnaryPlus(T obj) => UnaryPlusFunc(obj);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T UnaryMinus(T obj) => UnaryMinusFunc(obj);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Negate(T obj) => NegateFunc(obj);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T BitwiseComplement(T obj) => BitwiseComplementFunc(obj);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Increment(T obj) => IncrementFunc(obj);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Decrement(T obj) => DecrementFunc(obj);

		#endregion

		#region True/False operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsTrue(T obj) => TrueFunc(obj);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsFalse(T obj) => FalseFunc(obj);

		#endregion

		#region GetHashCode, ToString

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetHashCode(T obj) => GetHashCodeFunc(obj);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string ToString(T obj, string format, IFormatProvider formatProvider) =>
			ToStringFunc(obj, format, formatProvider);

		#endregion

		#endregion
	}
}