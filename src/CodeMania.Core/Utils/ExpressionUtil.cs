using System;
using System.Linq.Expressions;

namespace CodeMania.Core.Utils
{
	internal static class ExpressionUtil
	{
		public static Func<T1, T2, TResult> CreateBinaryMethod<T1, T2, TResult>(ParameterExpression left, ParameterExpression right,
			Func<ParameterExpression, ParameterExpression, BinaryExpression> getBody, string errorMessage = null)
		{
			try
			{
				return ExpressionCompiler.Default.Compile(
					Expression.Lambda<Func<T1, T2, TResult>>(getBody(left, right), left, right));
			}
			catch (Exception)
			{
				return (x, y) => throw new NotSupportedException(errorMessage ?? string.Empty);
			}
		}

		public static Func<T1, TResult> CreateUnaryMethod<T1, TResult>(ParameterExpression parameter,
			Func<ParameterExpression, UnaryExpression> getBody, string errorMessage = null)
		{
			try
			{
				return ExpressionCompiler.Default.Compile(
					Expression.Lambda<Func<T1, TResult>>(getBody(parameter), parameter));
			}
			catch (Exception)
			{
				return x => throw new NotSupportedException(errorMessage ?? string.Empty);
			}
		}
	}
}