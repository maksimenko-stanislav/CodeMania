using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CodeMania.Core.Extensions;
using CodeMania.Core.Internals;
using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers
{
	/// <summary>
	/// Contains helper parts for <see cref="ObjectStructureEqualityComparer{T}"/>.
	/// </summary>
	public static class ObjectStructureEqualityComparer
	{
		public static IEqualityComparer Create([NotNull] Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));

			return (IEqualityComparer) Activator.CreateInstance(typeof(ObjectStructureEqualityComparer<>)
				.MakeGenericType(type));
		}

		/// <summary>
		/// Gets access to <seealso cref="IEqualityComparer"/> for appropriate types.
		/// </summary>
		internal static readonly ConcurrentDictionary<Type, IEqualityComparer> DynamicallyResolvedComparers =
			new ConcurrentDictionary<Type, IEqualityComparer>();
	}

	/// <summary>
	/// Provides generic structural <see cref="IEqualityComparer{T}"/> implementation for any type.
	/// <see cref="IEqualityComparer{T}.Equals(T,T)"/> and <see cref="IEqualityComparer{T}.GetHashCode(T)"/> inspect object structure
	/// by provided set of properties and/or fields with respect to collection and complex object properties.
	/// </summary>
	/// <typeparam name="T">Type to compare equality.</typeparam>
	[PublicAPI]
	public sealed class ObjectStructureEqualityComparer<T> : EqualityComparer<T>
	{
		#region Fields

		private static readonly Lazy<ObjectStructureEqualityComparer<T>> DefaultLazy =
			new Lazy<ObjectStructureEqualityComparer<T>>(() => new ObjectStructureEqualityComparer<T>(), true);

		private readonly Func<T, T, bool> primitivePropertyComparer;
		private readonly Func<T, int> primitivePropertyHashCode;

		private readonly List<(Func<T, object> Getter, string Name)> complexMembers;

		#endregion

		#region Properties

		public static ObjectStructureEqualityComparer<T> Instance => DefaultLazy.Value;

		#endregion

		#region .ctor

		public ObjectStructureEqualityComparer() : this(typeof(T).GetPublicInstanceProperties())
		{
		}

		private ObjectStructureEqualityComparer(IEnumerable<PropertyInfo> properties)
			: this((properties ?? throw new ArgumentNullException(nameof(properties))).Select(p => (p as MemberInfo, p.PropertyType)))
		{
		}

		private ObjectStructureEqualityComparer(IEnumerable<FieldInfo> fields)
			: this((fields ?? throw new ArgumentNullException(nameof(fields))).Select(f => (f as MemberInfo, f.FieldType)))
		{
		}

		[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
		private ObjectStructureEqualityComparer(IEnumerable<(MemberInfo member, Type memberType)> propertyOrFields)
		{
			var x = Expression.Parameter(typeof(T), "x");
			var y = Expression.Parameter(typeof(T), "y");

			// get properties which returns complex types or collections of complex type.s
			complexMembers = propertyOrFields
				.Where(p => !p.memberType.IsWellKnownPrimitiveOrCollectionOfThesePrimitives())
				.OrderBy(p => p.memberType.AssemblyQualifiedName)
				.ThenBy(p => p.member.Name)
				.Select(p =>
						(
							ExpressionCompiler.Default.Compile(
								Expression.Lambda<Func<T, object>>(
									Expression.Convert(
										Expression.MakeMemberAccess(x, p.member),
										typeof(object)
										),
									x)),
							p.member.Name
						)
					)
				.ToList();

			var primitiveMembers = propertyOrFields
				.Where(p => p.memberType.IsWellKnownPrimitiveOrCollectionOfThesePrimitives())
				.OrderBy(p => p.memberType.AssemblyQualifiedName)
				.ToList();

			Expression equalsExpression = null;
			var combineHashCodeMethod = typeof(HashHelper).GetMethod(nameof(HashHelper.CombineHashCodes));

			if (combineHashCodeMethod == null)
			{
				throw new InvalidOperationException(); // TODO: Pass message
			}

			var getHashCodeExpressions = new List<Expression>(2 + primitiveMembers.Count * 2); // provide estimated capacity
			// int hashCode
			var hashCode = Expression.Variable(typeof(int), "hashCode");
			// hashCode = HashHelper.HashSeed;
			var initHashCode = Expression.Assign(hashCode, Expression.Constant(HashHelper.HashSeed));

			getHashCodeExpressions.Add(initHashCode);

			foreach (var propertyOrField in primitiveMembers)
			{
				if (equalsExpression == null)
				{
					equalsExpression = EqualsExpressions<T>.CreateEqualsExpression(propertyOrField.member, x, y);
				}
				else
				{
					equalsExpression =
						Expression.AndAlso(
							equalsExpression,
							EqualsExpressions<T>.CreateEqualsExpression(propertyOrField.member, x, y));
				}

				// HashHelper.CombineHashCodes(hashCode * 397, GetHashCode(obj))
				Expression combineHashCodes = Expression.Call(
					null,
					combineHashCodeMethod,
					Expression.Multiply(hashCode, Expression.Constant(397)),
					HashCodeExpressions<T>.CreateHashCodeExpression(propertyOrField.member, x));

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

			primitivePropertyComparer = ExpressionCompiler.Default.Compile(
				Expression.Lambda<Func<T, T, bool>>(
					equalsExpression ?? Expression.Constant(true),
					x, y)
			);

			primitivePropertyHashCode = ExpressionCompiler.Default.Compile(
				Expression.Lambda<Func<T, int>>(
					Expression.Block(
						typeof(int),
						new[] { hashCode },
						getHashCodeExpressions),
					x)
			);
		}

		#endregion

		#region Methods

		public override bool Equals(T x, T y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (ReferenceEquals(x, null)) return false;
			if (ReferenceEquals(y, null)) return false;
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
				return EqualsInternal(x, y, context);
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

		public override int GetHashCode(T obj)
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
				return GetHashCodeInternal(obj, context);
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

		private bool EqualsInternal(T x, T y, EqualityComparerContext context)
		{
			if (!primitivePropertyComparer(x, y)) return false;

			int i = 0;
			for (; i < complexMembers.Count; i++)
			{
				var property = complexMembers[i];

				// now we know that both objects are "complex", so we can get or create appropriate and use it for comparision ObjectStructureEqualityComparer<T>.

				object xValue = property.Getter(x);
				object yValue = property.Getter(y);

				// OK, both property or field values are null, continuing comparision.
				if (ReferenceEquals(xValue, yValue)) continue;

				//if (ReferenceEquals(xValue, x) || ReferenceEquals(yValue, y)) continue;

				if (ReferenceEquals(xValue, null)) return false;
				if (ReferenceEquals(yValue, null)) return false;

				var valueType = xValue.GetType();

				if (valueType != yValue.GetType()) return false;

				if (!context.TryAdd(xValue) | !context.TryAdd(yValue)) continue;

				if (xValue is IEnumerable xCollection)
				{
					IEnumerable yCollection = (IEnumerable) yValue;

					if (!AreCollectionsEquals(xCollection, yCollection, context))
					{
						Debug.Print("Values of property or field {0}.{1} are not equals.", typeof(T), property.Name);
						return false;
					}

					continue;
				}

				// if xValue.GetType() == typeof(T) then yValue.GetType() == typeof(T)
				if (valueType == typeof(T))
				{
					if (!EqualsInternal((T) xValue, (T) yValue, context))
						return false;

					continue;
				}

				var comparer = GetOrCreateObjectStructureEqualityComparer(valueType);

				if (!comparer.Equals(xValue, yValue)) return false;
			}

			Debug.Assert(i == complexMembers.Count, "i == _propertyOrFields.Count");

			return true;
		}

		private int GetHashCodeInternal(T obj, EqualityComparerContext context)
		{
			var hashCode = primitivePropertyHashCode(obj);

			int i = 0;
			for (; i < complexMembers.Count; i++)
			{
				var property = complexMembers[i];

				object value = property.Getter(obj);

				if (value == null || !context.TryAdd(value))
				{
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, HashHelper.HashSeed);

					continue;
				}

				if (value is IEnumerable collection)
				{
					hashCode = HashHelper.CombineHashCodes(
						hashCode * 397,
						CalculateHashCodeForCollection(collection, context));

					continue;
				}

				var valueType = value.GetType();

				if (valueType == typeof(T))
				{
					hashCode = HashHelper.CombineHashCodes(
						hashCode * 397,
						GetHashCodeInternal((T) value, context));
					continue;
				}

				// now we know that both objects are "complex", so we can get or create appropriate and use it for comparision ObjectStructureEqualityComparer<T>.
				var comparer = GetOrCreateObjectStructureEqualityComparer(valueType);

				hashCode = HashHelper.CombineHashCodes(hashCode * 397, comparer.GetHashCode(property.Getter(obj)));
			}

			Debug.Assert(i == complexMembers.Count, "i == complexMembers.Count");

			return hashCode;
		}

		private bool AreCollectionsEquals(IEnumerable x, IEnumerable y, EqualityComparerContext context)
		{
			// TODO: avoid memory allocations on List<T> and arrays during virtual GetEnumerator call!
			// ReSharper disable PossibleMultipleEnumeration
			var xEnumerator = x.GetEnumerator();
			var yEnumerator = y.GetEnumerator();
			// ReSharper restore PossibleMultipleEnumeration

			try
			{
#if DEBUG
				// ReSharper disable PossibleMultipleEnumeration
				var xCount = x.OfType<object>().Count();
				var yCount = y.OfType<object>().Count();
				// ReSharper restore PossibleMultipleEnumeration

				var xCounter = 0;
				var yCounter = 0;
#endif

				while (true)
				{
					var canMoveX = xEnumerator.MoveNext();
					var canMoveY = yEnumerator.MoveNext();

					if (canMoveX != canMoveY)
						return false; // collections have different count of elements, consider them as non equals.

					if (!canMoveX)
					{
						break;
					}

#if DEBUG
					xCounter++;
					yCounter++;
#endif

					var xValue = xEnumerator.Current;
					var yValue = yEnumerator.Current;

					// OK, both values are null or references to same object, continuing comparision.
					if (ReferenceEquals(xValue, yValue)) continue;

					if (ReferenceEquals(xValue, null) || ReferenceEquals(yValue, null)) return false;

					var valueType = xValue.GetType();

					if (valueType != yValue.GetType()) return false;

					// check if we've already used this references
					if (!context.TryAdd(xValue) | !context.TryAdd(yValue)) continue;

					if (Equals(xValue, yValue)) continue;

					if (xValue is IEnumerable xValueAsCollection)
					{
						var yValueAsCollection = (IEnumerable) yValue;

						// NOTE: recursion
						if (!AreCollectionsEquals(xValueAsCollection, yValueAsCollection, context))
						{
							return false;
						}
					}

					// if xValue.GetType() == typeof(T) then yValue.GetType() == typeof(T)
					if (valueType == typeof(T))
					{
						if (!EqualsInternal((T) xValue, (T) yValue, context))
							return false;

						continue;
					}

					// now we know, that xValue and yValue are not collections, so consider them as "complex" type.
					if (!GetOrCreateObjectStructureEqualityComparer(valueType).Equals(xValue, yValue))
					{
						return false;
					}
				}

#if DEBUG
				Debug.Assert(xCounter == yCounter, "xCounter == yCounter");
				Debug.Assert(xCounter == xCount,   "xCounter == xCount");
				Debug.Assert(yCounter == yCount,   "yCounter == yCount");
#endif
			}
			finally
			{
				(xEnumerator as IDisposable)?.Dispose();
				(yEnumerator as IDisposable)?.Dispose();
			}

			return true;
		}

		private int CalculateHashCodeForCollection(IEnumerable collection, EqualityComparerContext context)
		{
			// ReSharper disable once PossibleMultipleEnumeration
			var enumerator = collection.GetEnumerator();
			var hashCode = HashHelper.HashSeed;

#if DEBUG
// ReSharper disable once PossibleMultipleEnumeration
			var count = collection.OfType<object>().Count();

			var counter = 0;
#endif
			try
			{
				while (enumerator.MoveNext())
				{
#if DEBUG
					counter++;
#endif

					var value = enumerator.Current;

					// check value for null or if we have already visited this reference we need to continue calculation to avoid StackOverFlowException.
					if (value == null || !context.TryAdd(value))
					{
						hashCode = HashHelper.CombineHashCodes(hashCode * 397, HashHelper.HashSeed);

						continue;
					}

					if (value is IEnumerable valueAsCollection)
					{
						// NOTE: recursion
						hashCode = HashHelper.CombineHashCodes(
							hashCode * 397,
							CalculateHashCodeForCollection(valueAsCollection, context));

						continue;
					}

					if (value.GetType() == typeof(T))
					{
						hashCode = HashHelper.CombineHashCodes(
							hashCode * 397,
							GetHashCodeInternal((T) value, context));

						continue;
					}

					// now we know, that value is not collections, so consider it as "complex" type.
					hashCode = HashHelper.CombineHashCodes(
						hashCode * 397,
						GetOrCreateObjectStructureEqualityComparer(value.GetType()).GetHashCode(value));
				}
			}
			finally
			{
				(enumerator as IDisposable)?.Dispose();
			}

#if DEBUG
			Debug.Assert(counter == count, "counter == count");
#endif

			return hashCode;
		}

		private IEqualityComparer GetOrCreateObjectStructureEqualityComparer(Type type)
		{
			if (type == typeof(T)) return this;

			if (ObjectStructureEqualityComparer.DynamicallyResolvedComparers.TryGetValue(type, out var comparer))
			{
				return comparer;
			}

			comparer = ObjectStructureEqualityComparer.Create(type);

			ObjectStructureEqualityComparer.DynamicallyResolvedComparers.TryAdd(type, comparer);

			return comparer;
		}

		#endregion
	}
}