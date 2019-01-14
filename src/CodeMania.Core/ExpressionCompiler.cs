using System;

namespace CodeMania.Core
{
	public static class ExpressionCompiler
	{
		private static IExpressionCompiler _default =
#if NETSTANDARD2_0
			DefaultExpressionCompiler.Instance;
#else
			new DynamicMethodExpressionCompiler();
#endif

		public static IExpressionCompiler Default
		{
			get => _default;
			set => _default = value ?? throw new ArgumentNullException(nameof(value));
		}
	}
}