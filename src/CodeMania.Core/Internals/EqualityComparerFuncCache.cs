using System;
using System.Linq.Expressions;

namespace CodeMania.Core.Internals
{
	public static class EqualityComparerFuncCache<T>
	{
		public static readonly Func<T, T, bool> EqualsFunc;
		public static readonly Func<T, int> GetHashCodeFunc;

		static EqualityComparerFuncCache()
		{
			EqualsFunc = ExpressionCompiler.Default.Compile((Expression<Func<T, T, bool>>) EqualsExpressions.CreateEqualsExpression(typeof(T)));
			GetHashCodeFunc = ExpressionCompiler.Default.Compile((Expression<Func<T, int>>) HashCodeExpressions.CreateGetHashCodeExpression(typeof(T)));
		}
	}
}