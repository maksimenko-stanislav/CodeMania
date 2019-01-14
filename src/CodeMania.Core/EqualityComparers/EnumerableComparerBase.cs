using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers
{
	public abstract class EnumerableComparerBase<T, TCollection> : EqualityComparer<TCollection>
		where TCollection : IEnumerable<T>
	{
		protected static readonly Func<T, T, bool> AreEqualsFunc;
		protected static readonly Func<T, int> GetHashCodeFunc;
		protected static readonly Func<T, bool> IsNullFunc;
		private static readonly Func<TCollection, int> GetHashCodeImplementation;

		static EnumerableComparerBase()
		{
			var expressionCompiler = ExpressionCompiler.Default;

			AreEqualsFunc = expressionCompiler.Compile((Expression<Func<T, T, bool>>) EqualsExpressions.CreateEqualsExpression(typeof(T) /*, typeof(T).IsArray)*/));
			GetHashCodeFunc = expressionCompiler.Compile((Expression<Func<T, int>>) HashCodeExpressions.CreateGetHashCodeExpression(typeof(T) /*, typeof(T).IsArray)*/));
			IsNullFunc = expressionCompiler.Compile(NullableHelper.GetIsNullExpression<T>());

			GetHashCodeImplementation = NullableHelper.CanBeNull(typeof(T))
				? GetHashCodeForCollectionOfNullables
				: new Func<TCollection, int>(GetHashCodeForCollectionOfNonNullables);
		}

		public override int GetHashCode(TCollection obj) => GetHashCodeImplementation(obj);

		protected static int GetHashCodeForCollectionOfNonNullables(TCollection obj)
		{
			if (obj == null) return 0;

			unchecked
			{
				int hashCode = HashHelper.HashSeed;

				foreach (var element in obj)
				{
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, GetHashCodeFunc(element));
				}

				return hashCode;
			}
		}

		protected static int GetHashCodeForCollectionOfNullables(TCollection obj)
		{
			if (obj == null) return 0;

			unchecked
			{
				int hashCode = HashHelper.HashSeed;

				foreach (var element in obj)
				{
					hashCode = IsNullFunc(element)
						? ~HashHelper.CombineHashCodes(hashCode * 397, HashHelper.HashSeed)
						: HashHelper.CombineHashCodes(hashCode * 397, GetHashCodeFunc(element));
				}

				return hashCode;
			}
		}
	}
}