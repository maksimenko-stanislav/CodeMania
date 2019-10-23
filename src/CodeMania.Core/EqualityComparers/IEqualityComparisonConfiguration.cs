using System;

namespace CodeMania.Core.EqualityComparers
{
	public interface IEqualityComparisonConfiguration
	{
		IExpressionCompiler ExpressionCompiler { get; }
		bool PreferBuiltInEquatableImplementation { get; }
		StringComparison StringComparisonMode { get; }
		CollectionComparisonMode CollectionComparisonMode { get; }
	}
}