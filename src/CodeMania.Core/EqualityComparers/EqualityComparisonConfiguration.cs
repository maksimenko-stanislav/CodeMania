using System;

namespace CodeMania.Core.EqualityComparers
{
	public class EqualityComparisonConfiguration : IEqualityComparisonConfiguration
	{
		private IExpressionCompiler expressionCompiler;

		public static IEqualityComparisonConfiguration Default { get; } = new EqualityComparisonConfiguration
		{
			PreferBuiltInEquatableImplementation = false,
			StringComparisonMode = StringComparison.Ordinal,
			CollectionComparisonMode = CollectionComparisonMode.Default,
			ExpressionCompiler = Core.ExpressionCompiler.Default
		};

		public IExpressionCompiler ExpressionCompiler
		{
			get => expressionCompiler ?? Core.ExpressionCompiler.Default;
			set => expressionCompiler = value;
		}

		public bool PreferBuiltInEquatableImplementation { get; set; }
		public StringComparison StringComparisonMode { get; set; }
		public CollectionComparisonMode CollectionComparisonMode { get; set; }
	}
}