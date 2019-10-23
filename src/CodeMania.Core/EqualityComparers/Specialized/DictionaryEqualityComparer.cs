using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.Specialized
{
	public sealed class DictionaryEqualityComparer<TKey, TValue, TDictionary> : EqualityComparer<TDictionary>
		where TDictionary : IReadOnlyDictionary<TKey, TValue>
	{
		// ReSharper disable MultipleSpaces
		private static readonly Func<TKey, TKey, bool>     KeyEqualsFunc;
		private static readonly Func<TValue, TValue, bool> ValueEqualsFunc;
		private static readonly Func<TKey, int>            KeyGetHashCodeFunc;
		private static readonly Func<TValue, int>          ValueGetHashCodeFunc;

		private readonly Func<TKey, TKey, bool>     keyEqualsFunc;
		private readonly Func<TValue, TValue, bool> valueEqualsFunc;
		private readonly Func<TKey, int>            keyGetHashCodeFunc;
		private readonly Func<TValue, int>          valueGetHashCodeFunc;
		// ReSharper restore MultipleSpaces

		private static readonly Func<TValue, bool> IsValueNullFunc;

		private static readonly Lazy<DictionaryEqualityComparer<TKey, TValue, TDictionary>> LazyInstance =
			new Lazy<DictionaryEqualityComparer<TKey, TValue, TDictionary>>(() =>
				new DictionaryEqualityComparer<TKey, TValue, TDictionary>());

		public static DictionaryEqualityComparer<TKey, TValue, TDictionary> Instance => LazyInstance.Value;

		static DictionaryEqualityComparer()
		{
			var expressionCompiler = ExpressionCompiler.Default;
			// ReSharper disable MultipleSpaces
			KeyEqualsFunc        = expressionCompiler.Compile((Expression<Func<TKey, TKey, bool>>) EqualsExpressions.CreateEqualsExpression(typeof(TKey)       /*,typeof(TKey).IsArray*/));
			KeyGetHashCodeFunc   = expressionCompiler.Compile((Expression<Func<TKey, int>>) HashCodeExpressions.CreateGetHashCodeExpression(typeof(TKey)       /*,typeof(TKey).IsArray*/));
			ValueEqualsFunc      = expressionCompiler.Compile((Expression<Func<TValue, TValue, bool>>) EqualsExpressions.CreateEqualsExpression(typeof(TValue) /*,typeof(TKey).IsArray*/));
			ValueGetHashCodeFunc = expressionCompiler.Compile((Expression<Func<TValue, int>>) HashCodeExpressions.CreateGetHashCodeExpression(typeof(TValue)   /*,typeof(TKey).IsArray*/));
			IsValueNullFunc      = expressionCompiler.Compile(NullableHelper.GetIsNullExpression<TValue>());
			// ReSharper restore MultipleSpaces
		}

		public DictionaryEqualityComparer()
		{
			keyEqualsFunc		 = KeyEqualsFunc;
			valueEqualsFunc		 = ValueEqualsFunc;
			keyGetHashCodeFunc	 = KeyGetHashCodeFunc;
			valueGetHashCodeFunc = ValueGetHashCodeFunc;
		}

		public DictionaryEqualityComparer(IEqualityComparer<TKey> keyEqualityComparer, IEqualityComparer<TValue> valueEqualityComparer)
		{
			// ReSharper disable MultipleSpaces
			keyEqualsFunc        = keyEqualityComparer.Equals;
			valueEqualsFunc      = valueEqualityComparer.Equals;
			keyGetHashCodeFunc   = keyEqualityComparer.GetHashCode;
			valueGetHashCodeFunc = valueEqualityComparer.GetHashCode;
			// ReSharper restore MultipleSpaces
		}

		public override bool Equals(TDictionary x, TDictionary y)
		{
			if (x == null) return y == null;
			if (ReferenceEquals(x, y)) return true;

			if (y != null)
			{
				if (x.Count != y.Count) return false;

				foreach (var xPair in x)
				{
					if (!y.TryGetValue(xPair.Key, out var yValue) || !valueEqualsFunc(xPair.Value, yValue))
					{
						return false;
					}
				}

				return true;
			}

			return false;
		}

		public override int GetHashCode(TDictionary obj)
		{
			if (obj == null) return 0;

			unchecked
			{
				int hashCode = HashHelper.HashSeed;

				foreach (var pair in obj)
				{
					hashCode = HashHelper.CombineHashCodes(
						hashCode * 397,
						HashHelper.CombineHashCodes(
							keyGetHashCodeFunc(pair.Key),
							IsValueNullFunc(pair.Value) ? ~hashCode : valueGetHashCodeFunc(pair.Value)));
				}

				return hashCode;
			}
		}
	}
}