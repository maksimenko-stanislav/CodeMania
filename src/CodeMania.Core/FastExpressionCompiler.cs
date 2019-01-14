//using System;
//using System.Linq.Expressions;
//using FastExpressionCompiler;

//namespace CodeMania.Core
//{
//	public sealed class FastExpressionCompiler : IExpressionCompiler
//	{
//		private static readonly DefaultExpressionCompiler FallbackCompiler = new DefaultExpressionCompiler();

//		public TDelegate Compile<TDelegate>(Expression<TDelegate> expression)
//			where TDelegate : class
//		{
//			try
//			{
//				var result = expression.CompileFast(true);

//				if (result == null) throw new InvalidOperationException();

//				return result;
//			}
//			catch (Exception)
//			{
//				return FallbackCompiler.Compile(expression);
//			}
//		}

//		public Delegate Compile(Type delegateType, LambdaExpression expression)
//		{
//			try
//			{
//				var result = expression.CompileFast(true);

//				if (result == null) throw new InvalidOperationException();

//				return result;
//			}
//			catch (Exception)
//			{
//				return FallbackCompiler.Compile(delegateType, expression);
//			}
//		}
//	}
//}