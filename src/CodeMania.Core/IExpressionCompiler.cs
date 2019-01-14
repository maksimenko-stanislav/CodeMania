using System;
using System.Linq.Expressions;

namespace CodeMania.Core
{
	public interface IExpressionCompiler
	{
		TDelegate Compile<TDelegate>(Expression<TDelegate> expression)
			where TDelegate : class;

		Delegate Compile(Type delegateType, LambdaExpression expression);
	}
}