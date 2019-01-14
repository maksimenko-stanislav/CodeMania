using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace CodeMania.Core
{
	public class DefaultExpressionCompiler : IExpressionCompiler
	{
		public static readonly DefaultExpressionCompiler Instance = new DefaultExpressionCompiler();

		public TDelegate Compile<TDelegate>([NotNull] Expression<TDelegate> expression)
			where TDelegate : class
		{
			if (expression == null) throw new ArgumentNullException(nameof(expression));

			return expression.Compile();
		}

		public Delegate Compile([NotNull] Type delegateType, [NotNull] LambdaExpression expression)
		{
			if (delegateType == null) throw new ArgumentNullException(nameof(delegateType));
			if (expression == null) throw new ArgumentNullException(nameof(expression));

			if (!typeof(Delegate).IsAssignableFrom(delegateType))
				throw new ArgumentException("Type is not delegate.", nameof(delegateType));

			return expression.Compile();
		}
	}
}