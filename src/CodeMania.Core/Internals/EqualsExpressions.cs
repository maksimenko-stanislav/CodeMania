using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using CodeMania.Core.EqualityComparers;
using CodeMania.Core.Extensions;
using JetBrains.Annotations;

namespace CodeMania.Core.Internals
{
	public static class EqualsExpressions
	{
		public static LambdaExpression CreateEqualsExpression([NotNull] Type itemType, IEqualityComparisonConfiguration configuration = null)
		{
			if (itemType == null) throw new ArgumentNullException(nameof(itemType));

			configuration = configuration ?? EqualityComparisonConfiguration.Default;

			LambdaExpression elementTypeComparer;

			if (itemType.IsEnum)
			{
				elementTypeComparer = CreateForEnums(itemType);
			}
			else if (itemType == typeof(string))
			{
				elementTypeComparer = CreateForStrings(configuration.StringComparisonMode);
			}
			else if (itemType.IsBuiltInPrimitive() ||
			         itemType.IsAssignableFrom(typeof(IEquatable<>).MakeGenericType(itemType)))
			{
				elementTypeComparer = CreateForPrimitiveType(itemType);
			}
			else if (itemType.IsNullable(out Type nullableUnderlyingType))
			{
				elementTypeComparer = nullableUnderlyingType.IsEnum
					? CreateForNullableEnums(itemType)
					: CreateForDefaultEqualityComparer(itemType);
			}
			else if (itemType.IsCollection(out Type elementType) ||
			         itemType.IsReadOnlyDictionary(out _, out elementType))
			{
				elementTypeComparer = GetEqualsExpressionForCollection(itemType, elementType);
			}
			// TODO: what about value types (structs) without IEquatable support? I thing we need to use default equality comparer
			else
			{
				elementTypeComparer = CreateForObjectStructureEqualityComparer(itemType);
			}

			return elementTypeComparer;
		}

		private static LambdaExpression GetEqualsExpressionForCollection(Type collectionType, Type elementType)
		{
			return GetEqualsExpressionForEnumerable(collectionType, elementType);
		}

		private static LambdaExpression GetEqualsExpressionForEnumerable(Type collectionType, Type elementType)
		{
			Type comparerType = EqualityComparerTypeProvider.GetCustomEqualityComparerTypeFor(collectionType);

			var comparerExpression = Expression.MakeMemberAccess(null, comparerType.GetMember("Default")[0]);

			var xArr = Expression.Parameter(collectionType, "xArr");
			var yArr = Expression.Parameter(collectionType, "yArr");

			var equalsMethod =
				comparerType.GetMethod(nameof(IEqualityComparer.Equals), new[] {collectionType, collectionType})
				?? throw new InvalidOperationException($"Can't find {nameof(IEqualityComparer.Equals)} method.");

			return Expression.Lambda(
				typeof(Func<,,>).MakeGenericType(collectionType, collectionType, typeof(bool)),
				Expression.Call(comparerExpression, equalsMethod, xArr, yArr),
				// lambda arguments: (x, y) => <body>
				xArr, yArr
			);
		}

		private static LambdaExpression CreateForObjectStructureEqualityComparer(Type itemType)
		{
			var equalityComparerType = typeof(ObjectStructureEqualityComparer<>).MakeGenericType(itemType);

			var equalsMethod = equalityComparerType.GetMethod(nameof(Equals), new[] {itemType, itemType});

			if (equalsMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(Equals)} method.");
			}

			var x = Expression.Parameter(itemType, "x");
			var y = Expression.Parameter(itemType, "y");

			return Expression.Lambda(
				typeof(Func<,,>).MakeGenericType(itemType, itemType, typeof(bool)),
				Expression.Call(
					Expression.Property(null, equalityComparerType,
						nameof(ObjectStructureEqualityComparer<object>.Default)),
					equalsMethod,
					x, y),
				x, y);
		}

		/// <summary>
		/// Creates an expression using <see cref="EqualityComparer{T}.Default"/> equivalent to
		/// <para>
		/// <code>
		/// (x, y) => EqualityComparer&lt;SomeType&gt;.Default.Equals(x, y)
		/// </code>
		/// </para>
		/// </summary>
		/// <param name="itemType">Type object to create comparer.</param>
		private static LambdaExpression CreateForDefaultEqualityComparer(Type itemType)
		{
			var equalityComparerType = typeof(EqualityComparer<>).MakeGenericType(itemType);

			var equalsMethod = equalityComparerType.GetMethod(nameof(Equals), new[] {itemType, itemType});

			if (equalsMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(Equals)} method.");
			}

			var x = Expression.Parameter(itemType, "x");
			var y = Expression.Parameter(itemType, "y");

			return Expression.Lambda(
				typeof(Func<,,>).MakeGenericType(itemType, itemType, typeof(bool)),
				Expression.Call(
					Expression.Property(null, equalityComparerType, nameof(EqualityComparer<object>.Default)),
					equalsMethod,
					x, y),
				x, y);
		}

		private static LambdaExpression CreateForPrimitiveType(Type itemType)
		{
			var x = Expression.Parameter(itemType, "x");
			var y = Expression.Parameter(itemType, "y");

			try
			{
				return Expression.Lambda(
					typeof(Func<,,>).MakeGenericType(itemType, itemType, typeof(bool)),
					Expression.Equal(x, y),
					x, y);
			}
			catch (Exception)
			{
				// equality operator is not supported. try to use types Equals method
				return CreateForBuiltInEqualsMethod(itemType);
			}
		}

		/// <summary>
		/// Creates an expression using built-in equals method equivalent to
		/// <para>
		/// <code>
		/// (x, y) => x.Equals(y)
		/// </code>
		/// </para>
		/// </summary>
		/// <param name="itemType">Type object to create comparer.</param>
		private static LambdaExpression CreateForBuiltInEqualsMethod(Type itemType)
		{
			var equalsMethod = itemType.GetMethod(nameof(Equals), new[] {itemType});

			if (equalsMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(Equals)} method.");
			}

			var x = Expression.Parameter(itemType, "x");
			var y = Expression.Parameter(itemType, "y");

			return Expression.Lambda(
				typeof(Func<,,>).MakeGenericType(itemType, itemType, typeof(bool)),
				Expression.Call(
					x,
					equalsMethod,
					y),
				x, y);
		}

		/// <summary>
		/// Creates an expression equivalent to
		/// <para>
		/// <code>
		/// (x, y) => string.Equals(x, y, comparision)
		/// </code>
		/// </para>
		/// </summary>
		/// <param name="comparison">Type of string comparision.</param>
		private static LambdaExpression CreateForStrings(StringComparison comparison)
		{
			var equalsMethod = typeof(string).GetMethod(nameof(Equals),
				BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null,
				new[] {typeof(string), typeof(string), typeof(StringComparison)}, new ParameterModifier[0]);

			if (equalsMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(Equals)} method.");
			}

			var x = Expression.Parameter(typeof(string), "x");
			var y = Expression.Parameter(typeof(string), "y");

			return Expression.Lambda<Func<string, string, bool>>(
				Expression.Call(
					null, // we use static method string.Equals(string, string, StringComparison)
					equalsMethod,
					x,
					y,
					Expression.Constant(comparison)),
				x, y);
		}

		/// <summary>
		/// Creates an equals expression for enums using built-in Equals method for enum underlying type equivalent to
		/// <para>
		/// <code>
		/// (SomeEnum x, SomeEnum y) => ((SomeEnumUnderlyingType) x).Equals((SomeEnumUnderlyingType) y)
		/// </code>
		/// </para>
		/// </summary>
		/// <param name="enumType">Type object to create comparer.</param>
		private static LambdaExpression CreateForEnums(Type enumType)
		{
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("It is required to pass enum type.", nameof(enumType));
			}

			var underlyingType = Enum.GetUnderlyingType(enumType);

			Debug.Assert(underlyingType != null, "underlyingType != null");

			var equalsMethod = underlyingType.GetMethod(nameof(Equals), new[] {underlyingType});

			if (equalsMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(Equals)} method.");
			}

			var x = Expression.Parameter(enumType, "x");
			var y = Expression.Parameter(enumType, "y");

			return Expression.Lambda(
				typeof(Func<,,>).MakeGenericType(enumType, enumType, typeof(bool)),
				Expression.Call(
					Expression.Convert(x, underlyingType),
					equalsMethod,
					Expression.Convert(y, underlyingType)),
				x, y);
		}

		/// <summary>
		/// Creates an <see cref="LambdaExpression"/> using built-in Equals method equivalent to
		/// <para>
		/// <code>
		/// (T x, T y) => ((enumUnderlyingType)x).CustomFieldOrProperty.Equals((enumUnderlyingType) y.CustomFieldOrProperty)
		/// </code>
		/// </para> 
		/// </summary>
		/// <param name="nullableEnumType"></param>
		/// <returns></returns>
		private static LambdaExpression CreateForNullableEnums(Type nullableEnumType)
		{
			var nullableUnderlyingType = Nullable.GetUnderlyingType(nullableEnumType);

			if (nullableUnderlyingType == null || !nullableUnderlyingType.IsEnum)
			{
				throw new ArgumentException("It is required to pass nullable enum type.", nameof(nullableEnumType));
			}

			var enumUnderlyingType = Enum.GetUnderlyingType(nullableUnderlyingType);

			Debug.Assert(enumUnderlyingType != null, "underlyingType != null");

			var equalsMethod = enumUnderlyingType.GetMethod(nameof(Equals), new[] {enumUnderlyingType});

			if (equalsMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(Equals)} method.");
			}

			var x = Expression.Parameter(nullableEnumType, "x");
			var y = Expression.Parameter(nullableEnumType, "y");

			var hasValueMethodName = nameof(Nullable<int>.HasValue);
			var valueMethodName = nameof(Nullable<int>.Value);

			var falseConstant = Expression.Constant(false);

			// (TEnum? x, TEnum? y) => (!x.HasValue && !y.HasValue) || ((EnumUnderlyingType) x).Value.Equals((EnumUnderlyingType) y.Value)
			return Expression.Lambda(
				typeof(Func<,,>).MakeGenericType(nullableEnumType, nullableEnumType, typeof(bool)),
				Expression.OrElse(
					// left:  (TEnum? x, TEnum? y) => !x.HasValue && !y.HasValue
					Expression.AndAlso(
						Expression.Equal(
							falseConstant,
							Expression.PropertyOrField(x, hasValueMethodName)),
						Expression.Equal(
							falseConstant,
							Expression.PropertyOrField(y, hasValueMethodName))
					),
					// right: (TEnum? x, TEnum? y) => ((EnumUnderlyingType) x).Value.Equals((EnumUnderlyingType) y.Value)
					Expression.Call(
						Expression.Convert(
							Expression.PropertyOrField(x, valueMethodName),
							enumUnderlyingType),
						equalsMethod,
						Expression.Convert(
							Expression.PropertyOrField(y, valueMethodName),
							enumUnderlyingType))
				),
				x, y);
		}
	}

	public static class EqualsExpressions<T>
	{
		public static Expression<Func<T, T, bool>> CreateEqualsExpression([NotNull] MemberInfo memberInfo,
			IEqualityComparisonConfiguration configuration = null)
		{
			if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

			var x = Expression.Parameter(typeof(T), "x");
			var y = Expression.Parameter(typeof(T), "y");
			var equalityComparerContext = Expression.Parameter(typeof(EqualityComparerContext), "context");

			Expression body;
			switch (memberInfo)
			{
				case PropertyInfo pi when pi.CanRead:
					body = CreateEqualsExpression(pi, x, y, equalityComparerContext, configuration);
					break;
				case FieldInfo fi when (fi.Attributes & FieldAttributes.Static) != FieldAttributes.Static:
					body = CreateEqualsExpression(fi, x, y, equalityComparerContext, configuration);
					break;
				default:
					throw new ArgumentException(
						$"Unsupported member type. Available types are {nameof(FieldInfo)} and {nameof(PropertyInfo)} with read support.",
						nameof(memberInfo));
			}

			return Expression.Lambda<Func<T, T, bool>>(body, x, y, equalityComparerContext);
		}

		public static Expression CreateEqualsExpression(
			[NotNull] FieldInfo fieldInfo,
			[NotNull] ParameterExpression x,
			[NotNull] ParameterExpression y,
			[NotNull] ParameterExpression equalityComparerContext,
			IEqualityComparisonConfiguration configuration = null) =>
				CreateEqualsExpression(fieldInfo, fieldInfo.FieldType, x, y, equalityComparerContext, configuration);

		public static Expression CreateEqualsExpression(
			[NotNull] PropertyInfo propertyInfo,
			[NotNull] ParameterExpression x,
			[NotNull] ParameterExpression y,
			[NotNull] ParameterExpression equalityComparerContext,
			IEqualityComparisonConfiguration configuration = null) =>
				CreateEqualsExpression(propertyInfo, propertyInfo.PropertyType, x, y, equalityComparerContext, configuration);

		public static Expression CreateEqualsExpression(
			[NotNull] MemberInfo memberInfo,
			[NotNull] ParameterExpression x,
			[NotNull] ParameterExpression y,
			[NotNull] ParameterExpression equalityComparerContext,
			IEqualityComparisonConfiguration configuration = null)
		{
			if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));
			if (x == null) throw new ArgumentNullException(nameof(x));
			if (y == null) throw new ArgumentNullException(nameof(y));

			if (memberInfo is FieldInfo fieldInfo)
			{
				return CreateEqualsExpression(fieldInfo, fieldInfo.FieldType, x, y, equalityComparerContext, configuration);
			}

			if (memberInfo is PropertyInfo propertyInfo)
			{
				return CreateEqualsExpression(propertyInfo, propertyInfo.PropertyType, x, y, equalityComparerContext, configuration);
			}

			throw new ArgumentException($"Expected {nameof(FieldInfo)} or {nameof(PropertyInfo)}.", nameof(memberInfo));
		}

		private static Expression CreateEqualsExpression(
			[NotNull] MemberInfo memberInfo,
			[NotNull] Type memberType,
			[NotNull] ParameterExpression x,
			[NotNull] ParameterExpression y,
			[NotNull] ParameterExpression equalityComparerContext,
			IEqualityComparisonConfiguration configuration)
		{
			if (memberInfo == null)
				throw new ArgumentNullException(nameof(memberInfo));
			if (memberType == null)
				throw new ArgumentNullException(nameof(memberType));
			if (x == null)
				throw new ArgumentNullException(nameof(x));
			if (y == null)
				throw new ArgumentNullException(nameof(y));
			if (!typeof(T).IsAssignableFrom(x.Type))
				throw new ArgumentException($"Expected that parameter returns type assignable from {typeof(T)}.", nameof(x));
			if (!typeof(T).IsAssignableFrom(x.Type))
				throw new ArgumentException($"Expected that parameter returns type assignable from {typeof(T)}.", nameof(y));

			configuration = configuration ?? EqualityComparisonConfiguration.Default;

			Expression equalsFunc;

			if (memberType == typeof(string))
			{
				equalsFunc = CreateForStrings(memberInfo, configuration.StringComparisonMode, x, y);
			}
			else if (memberType.IsEnum)
			{
				equalsFunc = CreateForEnums(memberInfo, memberType, x, y);
			}
			else if (memberType.IsBuiltInPrimitive() || memberType.IsGenericAssignable(typeof(IEquatable<>)))
			{
				equalsFunc = CreateForPrimitiveType(memberInfo, memberType, x, y);
			}
			else if (memberType.IsNullable(out var underlyingType))
			{
				equalsFunc = underlyingType.IsEnum
					? CreateForNullableEnums(memberInfo, memberType, x, y) as Expression
					: CreateForDefaultEqualityComparer(memberInfo, memberType, x, y);
			}
			else if (memberType.IsCollection(out Type elementType) ||
			         memberType.IsReadOnlyDictionary(out _, out elementType))
			{
				equalsFunc = GetEqualsExpressionForEnumerable(memberInfo, memberType, x, y, equalityComparerContext);
			}
			else
			{
				equalsFunc = CreateForObjectStructureEqualityComparer(memberInfo, memberType, x, y, equalityComparerContext);
			}

			return equalsFunc;
		}

		private static MethodCallExpression GetEqualsExpressionForEnumerable(
			MemberInfo memberInfo,
			Type collectionType, ParameterExpression x, ParameterExpression y, ParameterExpression equalityComparerContext)
		{
			Type comparerType = EqualityComparerTypeProvider.GetCustomEqualityComparerTypeFor(collectionType);

			// we need to have high performance static instance property with name 'Default' in each comparer type.
			// We must not spend time on creating instance of collection comparer.
			// Ensure that all collection comparers follow this rule.
			var comparerExpression = Expression.MakeMemberAccess(null, comparerType.GetMember("Default")[0]);

			return comparerType.IsGenericAssignable(typeof(IReferenceTypeEqualityComparer<>))
				? Expression.Call(
					comparerExpression,
					comparerType.GetMethod(nameof(IEqualityComparer.Equals), new[] { collectionType, collectionType, typeof(EqualityComparerContext) }),
					Expression.MakeMemberAccess(x, memberInfo),
					Expression.MakeMemberAccess(y, memberInfo),
					equalityComparerContext)
				: Expression.Call(
					comparerExpression,
					comparerType.GetMethod(nameof(IEqualityComparer.Equals), new[] { collectionType, collectionType }),
					Expression.MakeMemberAccess(x, memberInfo),
					Expression.MakeMemberAccess(y, memberInfo));
		}

		private static MethodCallExpression CreateForDefaultEqualityComparer(
			MemberInfo memberInfo, Type memberType, ParameterExpression x, ParameterExpression y)
		{
			var equalityComparerType = typeof(EqualityComparer<>).MakeGenericType(memberType);

			var equalsMethod = equalityComparerType.GetMethod(nameof(Equals), new[] {memberType, memberType});

			if (equalsMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(Equals)} method.");
			}

			return Expression.Call(
				Expression.Property(null, equalityComparerType, nameof(EqualityComparer<T>.Default)),
				equalsMethod,
				Expression.MakeMemberAccess(x, memberInfo),
				Expression.MakeMemberAccess(y, memberInfo));
		}

		private static MethodCallExpression CreateForObjectStructureEqualityComparer(
			MemberInfo memberInfo, Type memberType, ParameterExpression x, ParameterExpression y, ParameterExpression equalityComparerContext)
		{
			var equalityComparerType = typeof(ObjectStructureEqualityComparer<>).MakeGenericType(memberType);

			var equalsMethod = equalityComparerType.GetMethod(nameof(Equals), new[] {memberType, memberType, typeof(EqualityComparerContext)});

			if (equalsMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(Equals)} method.");
			}

			return Expression.Call(
				Expression.Property(null, equalityComparerType, nameof(ObjectStructureEqualityComparer<object>.Default)),
				equalsMethod,
				Expression.MakeMemberAccess(x, memberInfo),
				Expression.MakeMemberAccess(y, memberInfo),
				equalityComparerContext);
		}

		private static Expression CreateForPrimitiveType(MemberInfo memberInfo,
			Type memberType, ParameterExpression x, ParameterExpression y)
		{
			try
			{
				return Expression.Equal(
					Expression.MakeMemberAccess(x, memberInfo),
					Expression.MakeMemberAccess(y, memberInfo));
			}
			catch (Exception)
			{
				return CreateForBuiltInEqualsMethod(memberInfo, memberType, x, y);
			}
		}

		private static MethodCallExpression CreateForBuiltInEqualsMethod(MemberInfo memberInfo,
			Type memberType, ParameterExpression x, ParameterExpression y)
		{
			var equalsMethod = memberType.GetMethod(nameof(Equals), new[] {memberType});

			if (equalsMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(Equals)} method.");
			}

			return Expression.Call(
				Expression.MakeMemberAccess(x, memberInfo),
				equalsMethod,
				Expression.MakeMemberAccess(y, memberInfo));
		}

		private static Expression CreateForStrings(MemberInfo memberInfo,
			StringComparison comparison, ParameterExpression x, ParameterExpression y)
		{
			var equalsMethod = typeof(string).GetMethod(nameof(Equals),
				BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null,
				new[] {typeof(string), typeof(string), typeof(StringComparison)}, new ParameterModifier[0]);

			if (equalsMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(Equals)} method.");
			}

			return Expression.Call(
				null, // we use static method string.Equals(string, string, StringComparison)
				equalsMethod,
				Expression.PropertyOrField(x, memberInfo.Name),
				Expression.PropertyOrField(y, memberInfo.Name),
				Expression.Constant(comparison));
		}

		private static MethodCallExpression CreateForEnums(MemberInfo memberInfo, Type memberType,
			ParameterExpression x, ParameterExpression y)
		{
			if (!memberType.IsEnum)
			{
				throw new ArgumentException("It is required to pass enum type.", nameof(memberType));
			}

			var underlyingType = Enum.GetUnderlyingType(memberType);

			Debug.Assert(underlyingType != null, "underlyingType != null");

			var equalsMethod = underlyingType.GetMethod(nameof(Equals), new[] {underlyingType});

			if (equalsMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(Equals)} method.");
			}

			return Expression.Call(
				Expression.Convert(Expression.PropertyOrField(x, memberInfo.Name), underlyingType),
				equalsMethod,
				Expression.Convert(Expression.PropertyOrField(y, memberInfo.Name), underlyingType));
		}

		private static BinaryExpression CreateForNullableEnums(MemberInfo memberInfo,
			Type memberType, ParameterExpression x, ParameterExpression y)
		{
			var nullableUnderlyingType = Nullable.GetUnderlyingType(memberType);

			if (nullableUnderlyingType == null || !nullableUnderlyingType.IsEnum)
			{
				throw new ArgumentException("It is required to pass nullable enum type.", nameof(memberType));
			}

			var enumUnderlyingType = Enum.GetUnderlyingType(nullableUnderlyingType);

			Debug.Assert(enumUnderlyingType != null, "underlyingType != null");

			var equalsMethod = enumUnderlyingType.GetMethod(nameof(Equals), new[] {enumUnderlyingType});

			if (equalsMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(Equals)} method.");
			}

			var hasValueMethodName = nameof(Nullable<int>.HasValue);
			var valueMethodName = nameof(Nullable<int>.Value);

			var falseConstant = Expression.Constant(false);

			// (TEnum? x, TEnum? y) => (!x.CustomNullableEnumValue.HasValue && !y.CustomNullableEnumValue.HasValue) ||
			// ((EnumUnderlyingType) x.CustomNullableEnumValue).Value.Equals((EnumUnderlyingType) y.CustomNullableEnumValue.Value)
			return Expression.OrElse(
				// left:  (TEnum? x, TEnum? y) => !x.CustomNullableEnumValue.HasValue && !y.CustomNullableEnumValue.HasValue
				Expression.AndAlso(
					Expression.Equal(
						falseConstant,
						Expression.PropertyOrField(Expression.PropertyOrField(x, memberInfo.Name),
							hasValueMethodName)),
					Expression.Equal(
						falseConstant,
						Expression.PropertyOrField(Expression.PropertyOrField(y, memberInfo.Name),
							hasValueMethodName))
				),
				// right: (TEnum? x, TEnum? y) => ((EnumUnderlyingType) x.CustomNullableEnumValue).Value.Equals((EnumUnderlyingType) y.CustomNullableEnumValue.Value)
				Expression.Call(
					Expression.Convert(
						Expression.PropertyOrField(
							Expression.PropertyOrField(x, memberInfo.Name),
							valueMethodName),
						enumUnderlyingType),
					equalsMethod,
					Expression.Convert(
						Expression.PropertyOrField(
							Expression.PropertyOrField(y, memberInfo.Name),
							valueMethodName),
						enumUnderlyingType)));
		}
	}
}