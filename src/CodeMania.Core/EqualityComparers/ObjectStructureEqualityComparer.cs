using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CodeMania.Core.Extensions;
using CodeMania.Core.Internals;
using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers
{
	/// <summary>
	/// Provides generic structural <see cref="IEqualityComparer{T}"/> implementation for any type.
	/// <see cref="IEqualityComparer{T}.Equals(T,T)"/> and <see cref="IEqualityComparer{T}.GetHashCode(T)"/> inspect object structure
	/// by provided set of properties and/or fields with respect to collection and complex object properties.
	/// </summary>
	/// <typeparam name="T">Type to compare equality.</typeparam>
	[PublicAPI]
	public sealed class ObjectStructureEqualityComparer<T> : IReferenceTypeEqualityComparer<T>, IEqualityComparer<T>, IEqualityComparer
		where T : class
	{
		#region Fields

		private static readonly Lazy<ObjectStructureEqualityComparer<T>> DefaultLazy =
			new Lazy<ObjectStructureEqualityComparer<T>>(() => new ObjectStructureEqualityComparer<T>(), true);

		private readonly Func<T, T, EqualityComparerContext, bool> areEqualsFunc;
		private readonly Func<T, EqualityComparerContext, int> getHashCodeFunc;

		private static readonly int DefaultHashCode = typeof(T).GetHashCode();

		#endregion

		#region Properties

		public static ObjectStructureEqualityComparer<T> Default => DefaultLazy.Value;

		#endregion

		#region .ctor

		public ObjectStructureEqualityComparer() : this(typeof(T).GetPublicInstanceProperties(), EqualityComparisonConfiguration.Default)
		{
		}

		public ObjectStructureEqualityComparer(IEqualityComparisonConfiguration configuration) : this(typeof(T).GetPublicInstanceProperties(), configuration)
		{
		}

		public ObjectStructureEqualityComparer([NotNull] IEnumerable<PropertyInfo> properties, [CanBeNull] IEqualityComparisonConfiguration configuration = null)
			: this((properties ?? throw new ArgumentNullException(nameof(properties))).Select(p => (p as MemberInfo, p.PropertyType)), configuration)
		{
		}

		public ObjectStructureEqualityComparer([NotNull] IEnumerable<FieldInfo> fields, [CanBeNull] IEqualityComparisonConfiguration configuration = null)
			: this((fields ?? throw new ArgumentNullException(nameof(fields))).Select(f => (f as MemberInfo, f.FieldType)), configuration)
		{
		}

		private ObjectStructureEqualityComparer(IEnumerable<(MemberInfo member, Type memberType)> propertyOrFields, IEqualityComparisonConfiguration configuration)
		{
			configuration = configuration ?? EqualityComparisonConfiguration.Default;

			var x = Expression.Parameter(typeof(T), "x");
			var y = Expression.Parameter(typeof(T), "y");
			var contextParam = Expression.Parameter(typeof(EqualityComparerContext), "context");

			var memberList = propertyOrFields.ToList();

			Expression equalsExpression = null;
			var combineHashCodeMethod = typeof(HashHelper).GetMethod(nameof(HashHelper.CombineHashCodes));

			if (combineHashCodeMethod == null)
			{
				throw new InvalidOperationException(); // TODO: Pass message
			}

			var getHashCodeExpressions = new List<Expression>(2 + memberList.Count * 2); // provide estimated capacity
			// int hashCode
			var hashCode = Expression.Variable(typeof(int), "hashCode");
			// hashCode = HashHelper.HashSeed;
			var initHashCode = Expression.Assign(hashCode, Expression.Constant(HashHelper.HashSeed));

			getHashCodeExpressions.Add(initHashCode);

			var hashPrime397 = Expression.Constant(397);

			foreach (var propertyOrField in memberList)
			{
				if (equalsExpression == null)
				{
					equalsExpression = EqualsExpressions<T>.CreateEqualsExpression(propertyOrField.member, x, y, contextParam, configuration);
				}
				else
				{
					equalsExpression =
						Expression.AndAlso(
							equalsExpression,
							EqualsExpressions<T>.CreateEqualsExpression(propertyOrField.member, x, y, contextParam, configuration));
				}

				// HashHelper.CombineHashCodes(hashCode * 397, GetHashCode(obj))
				Expression combineHashCodes = Expression.Call(
					null,
					combineHashCodeMethod,
					Expression.Multiply(hashCode, hashPrime397),
					HashCodeExpressions<T>.CreateHashCodeExpression(propertyOrField.member, x, contextParam));

				getHashCodeExpressions.Add(Expression.Assign(hashCode, combineHashCodes));

				if (NullableHelper.CanBeNull(propertyOrField.memberType))
				{
					getHashCodeExpressions.Add(
						Expression.IfThen(
							// if (x.CustomProperty == default(CustomPropertyType))
							Expression.Equal(
								Expression.PropertyOrField(x, propertyOrField.member.Name),
								Expression.Default(propertyOrField.memberType)),
							//	hashCode = ~hashCode;
							Expression.Assign(hashCode, Expression.Negate(hashCode))));
				}
			}

			getHashCodeExpressions.Add(hashCode); // return hashCode

			areEqualsFunc = configuration.ExpressionCompiler.Compile(
				Expression.Lambda<Func<T, T, EqualityComparerContext, bool>>(
					equalsExpression ?? Expression.Constant(true),
					x, y, contextParam)
			);

			getHashCodeFunc = configuration.ExpressionCompiler.Compile(
				Expression.Lambda<Func<T, EqualityComparerContext, int>>(
					Expression.Block(
						typeof(int),
						new[] { hashCode },
						getHashCodeExpressions),
					x, contextParam)
			);
		}

		#endregion

		#region Methods

		public bool Equals(T x, T y)
		{
			if (x != null)
			{
				if (y != null)
				{
					if (ReferenceEquals(x, y)) return true;
					if (x.GetType() != y.GetType()) return false;

					EqualityComparerContext context = EqualityComparerContext.Current;
					bool isAcquired = false;

					if (!context.IsAcquired)
					{
						context.IsAcquired = isAcquired = true;
						context.Free();
					}

					try
					{
						return Equals(x, y, context);
					}
					finally
					{
						if (isAcquired)
						{
							context.IsAcquired = false;
							context.Free();
						}
					}
				}

				return false;
			}

			return y == null;
		}

		public int GetHashCode(T obj)
		{
			if (obj == null) return 0;

			EqualityComparerContext context = EqualityComparerContext.Current;
			bool isAcquired = false;

			if (!context.IsAcquired)
			{
				context.IsAcquired = isAcquired = true;
				context.Free();
			}

			try
			{
				return GetHashCode(obj, context);
			}
			finally
			{
				if (isAcquired)
				{
					context.IsAcquired = false;
					context.Free();
				}
			}
		}


		#endregion

		bool IEqualityComparer.Equals(object x, object y) => Equals((T) x, (T) y);

		int IEqualityComparer.GetHashCode(object obj) => GetHashCode((T) obj);

		public bool Equals(T x, T y, EqualityComparerContext context)
		{
			if (x != null)
			{
				if (y != null)
				{
					if (ReferenceEquals(x, y)) return true;
					if (x.GetType() != y.GetType()) return false;

					var xAdded = context.TryAdd(x);
					var yAdded = context.TryAdd(y);

					if (!xAdded || !yAdded) return true;

					return areEqualsFunc(x, y, context);
				}

				return false;
			}

			return y == null;
		}

		public int GetHashCode(T obj, EqualityComparerContext context)
		{
			if (obj == null) return 0;

			if (!context.TryAdd(obj)) return HashHelper.CombineHashCodes(DefaultHashCode, HashHelper.HashSeed);

			return getHashCodeFunc(obj, context);
		}
	}
}