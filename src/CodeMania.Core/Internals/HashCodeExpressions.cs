using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CodeMania.Core.EqualityComparers;
using CodeMania.Core.Extensions;
using JetBrains.Annotations;

namespace CodeMania.Core.Internals
{
	public static class HashCodeExpressions
	{
		public static LambdaExpression CreateGetHashCodeExpression([NotNull] Type itemType)
		{
			if (itemType == null) throw new ArgumentNullException(nameof(itemType));
			LambdaExpression elementTypeComparer;

			if (itemType.IsEnum)
			{
				elementTypeComparer = CreateForEnums(itemType);
			}
			else if (itemType == typeof(string))
			{
				elementTypeComparer = CreateForStrings();
			}
			else if (itemType.IsBuiltInPrimitive()
			         || itemType.IsAssignableFrom(typeof(IEquatable<>).MakeGenericType(itemType)))
			{
				elementTypeComparer = CreateForBuiltInGetHashCodeMethod(itemType);
			}
			else if (itemType.IsNullable(out var nullableUnderlyingType))
			{
				elementTypeComparer = nullableUnderlyingType.IsEnum
					? CreateForNullableEnums(itemType)
					: CreateForDefaultEqualityComparer(itemType);
			}
			else if (itemType.IsCollection(out Type elementType) ||
			         itemType.IsReadOnlyDictionary(out _, out elementType))
			{
				elementTypeComparer = GetHashCodeExpressionForCollection(itemType, elementType);
			}
			// TODO: what about value types (structs) without IEquatable support? I thing we need to use default equality comparer
			else
			{
				elementTypeComparer = CreateForObjectStructureEqualityComparer(itemType);
			}

			return elementTypeComparer;
		}

		private static LambdaExpression GetHashCodeExpressionForCollection(Type collectionType, Type elementType)
		{
			FieldInfo comparerField;
			if (!elementType.IsArray && collectionType == elementType.MakeArrayType() &&
			    (comparerField = typeof(PrimitiveTypeArrayEqualityComparers).GetFields()
				    .FirstOrDefault(x => x.Name == elementType.Name + "ArrayMemoryEqualityComparer")) != null)
			{
				return GetHashCodeExpressionForArrayOfPrimitiveTypes(collectionType, elementType, comparerField);
			}

			return GetHashCodeExpressionForEnumerable(collectionType, elementType);
		}

		private static LambdaExpression GetHashCodeExpressionForArrayOfPrimitiveTypes([NotNull] Type collectionType,
			[NotNull] Type elementType, [NotNull] FieldInfo comparerField)
		{
			if (comparerField == null) throw new ArgumentNullException(nameof(comparerField));

			var arrayType = elementType.MakeArrayType();

			var xArr = Expression.Parameter(arrayType, "xArr");

			var comparerValue = comparerField.GetValue(null) ??
			                    throw new InvalidOperationException(
				                    $"Can't get value for comparer of type '{comparerField.FieldType}'.");
			var comparerType = comparerValue.GetType();

			var getHashCodeMethod = comparerType.GetMethod(nameof(IEqualityComparer.GetHashCode), new[] {arrayType})
			                        ?? throw new InvalidOperationException(
				                        $"Can't find {nameof(IEqualityComparer.GetHashCode)} method.");

			var comparerExpression = Expression.Field(null, comparerField);

			return Expression.Lambda(
				typeof(Func<,>).MakeGenericType(collectionType, typeof(int)),
				Expression.Call(comparerExpression, getHashCodeMethod, xArr),
				xArr
			);
		}

		private static LambdaExpression GetHashCodeExpressionForEnumerable(
			Type collectionType, Type elementType)
		{
			Type comparerType;

			if (elementType.MakeArrayType() == collectionType)
			{
				comparerType = typeof(ArrayEqualityComparer<>).MakeGenericType(elementType);
			}
			else if (typeof(List<>).MakeGenericType(elementType) == collectionType)
			{
				comparerType = typeof(ListEqualityComparer<>).MakeGenericType(elementType);
			}
			else if (collectionType.IsReadOnlyDictionary(out _, out var keyValuePairType))
			{
				comparerType = typeof(DictionaryEqualityComparer<,,>).MakeGenericType(
					keyValuePairType.GetGenericArguments()[0],
					keyValuePairType.GetGenericArguments()[1],
					collectionType);
			}
			else
			{
				comparerType = typeof(GenericCollectionEqualityComparer<>).MakeGenericType(elementType);
			}

			// we need to have high performance static instance property with name 'Instance' in each comparer type.
			// We must not spend time on creating instance of collection comparer.
			// Ensure that all collection comparers follow this rule.
			var comparerExpression = Expression.Property(null, comparerType, "Instance");

			var xArr = Expression.Parameter(collectionType, "xArr");

			var getHashCodeMethod =
				comparerType.GetMethod(nameof(IEqualityComparer.GetHashCode), new[] {collectionType})
				?? throw new InvalidOperationException($"Can't find {nameof(IEqualityComparer.GetHashCode)} method.");

			return Expression.Lambda(
				typeof(Func<,>).MakeGenericType(collectionType, typeof(int)),
				Expression.Call(comparerExpression, getHashCodeMethod, xArr),
				// lambda arguments: (x, y) => <body>
				xArr
			);
		}

		// (SomeType x) => EqualityComparer<SomeType>.Default.GetHashCode(x)
		private static LambdaExpression CreateForObjectStructureEqualityComparer([NotNull] Type type)
		{
			var x = Expression.Parameter(type, "x");

			var body = CreateFoObjectStructureEqualityComparerExpressionBody(type, x);

			return Expression.Lambda(
				typeof(Func<,>).MakeGenericType(type, typeof(int)),
				body,
				x);
		}

		private static MethodCallExpression CreateFoObjectStructureEqualityComparerExpressionBody([NotNull] Type type,
			ParameterExpression x)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));

			var equalityComparerType = typeof(ObjectStructureEqualityComparer<>).MakeGenericType(type);
			var getHashCodeMethod = equalityComparerType.GetMethod(nameof(GetHashCode), new[] {type});

			if (getHashCodeMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(GetHashCode)} method.");
			}

			return Expression.Call(
				Expression.Property(null, equalityComparerType,
					nameof(ObjectStructureEqualityComparer<object>.Instance)),
				getHashCodeMethod,
				x);
		}

		// (SomeType x) => EqualityComparer<SomeType>.Default.GetHashCode(x)
		private static LambdaExpression CreateForDefaultEqualityComparer([NotNull] Type type)
		{
			var x = Expression.Parameter(type, "x");

			var body = CreateForDefaultEqualityComparerExpressionBody(type, x);

			return Expression.Lambda(
				typeof(Func<,>).MakeGenericType(type, typeof(int)),
				body,
				x);
		}

		private static MethodCallExpression CreateForDefaultEqualityComparerExpressionBody([NotNull] Type type,
			ParameterExpression x)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));

			var equalityComparerType = typeof(EqualityComparer<>).MakeGenericType(type);
			var getHashCodeMethod = equalityComparerType.GetMethod(nameof(GetHashCode), new[] {type});

			if (getHashCodeMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(GetHashCode)} method.");
			}

			return Expression.Call(
				Expression.Property(null, equalityComparerType, nameof(EqualityComparer<object>.Default)),
				getHashCodeMethod,
				x);
		}

		// (SomeType x) => x.GetHashCode()
		private static LambdaExpression CreateForBuiltInGetHashCodeMethod([NotNull] Type type)
		{
			var x = Expression.Parameter(type, "x");

			var body = CreateForBuiltInGetHashCodeMethodExpressionBody(type, x);

			return Expression.Lambda(
				typeof(Func<,>).MakeGenericType(type, typeof(int)),
				body,
				x);
		}

		private static Expression CreateForBuiltInGetHashCodeMethodExpressionBody([NotNull] Type type,
			ParameterExpression x)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));

			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Byte:
				case TypeCode.Char:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.SByte:
				case TypeCode.UInt16:
					return x;
			}

			var getHashCodeMethod = type.GetMethod(nameof(GetHashCode), new Type[0]);

			if (getHashCodeMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(GetHashCode)} method.");
			}

			return Expression.Call(x, getHashCodeMethod);
		}

		// (string x) => x != null ? x.GetHashCode() : 0
		private static LambdaExpression CreateForStrings()
		{
			var x = Expression.Parameter(typeof(string), "x");

			var body = CreateForStringsExpressionBody(x);

			return Expression.Lambda(
				typeof(Func<,>).MakeGenericType(typeof(string), typeof(int)),
				body,
				x);
		}

		private static ConditionalExpression CreateForStringsExpressionBody(ParameterExpression x)
		{
			var getHashCodeMethod = typeof(string).GetMethod(nameof(GetHashCode), new Type[0]);

			if (getHashCodeMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(getHashCodeMethod)} method.");
			}

			return Expression.Condition(
				// test
				Expression.NotEqual(x, Expression.Constant(null, typeof(string))),
				// true part
				Expression.Call(x, getHashCodeMethod),
				// false part
				Expression.Constant(0, typeof(int)));
		}

		// (EnumType x) => ((EnumUnderlyingType) x).GetHashCode()
		private static LambdaExpression CreateForEnums([NotNull] Type enumType)
		{
			var x = Expression.Parameter(enumType, "x");

			var body = CreateForEnumsExpressionBody(enumType, x);

			return Expression.Lambda(
				typeof(Func<,>).MakeGenericType(enumType, typeof(int)),
				body,
				x);
		}

		private static MethodCallExpression CreateForEnumsExpressionBody([NotNull] Type enumType, ParameterExpression x)
		{
			if (enumType == null) throw new ArgumentNullException(nameof(enumType));
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("It is required to pass enum type.", nameof(enumType));
			}

			var underlyingType = Enum.GetUnderlyingType(enumType);

			Debug.Assert(underlyingType != null, "underlyingType != null");

			var getHashMethod = underlyingType.GetMethod(nameof(GetHashCode), new Type[0]);

			if (getHashMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(GetHashCode)} method.");
			}

			return Expression.Call(Expression.Convert(x, underlyingType), getHashMethod);
		}

		// (EnumType? x) => x.HasValue ? x.Value.GetHashCode() : specialNullHashCode;
		private static LambdaExpression CreateForNullableEnums([NotNull] Type nullableEnumType)
		{
			var x = Expression.Parameter(nullableEnumType, "x");

			var body = CreateForNullableEnumsExpressionBody(nullableEnumType, x);

			return Expression.Lambda(
				typeof(Func<,>).MakeGenericType(nullableEnumType, typeof(int)),
				body,
				x);
		}

		private static ConditionalExpression CreateForNullableEnumsExpressionBody([NotNull] Type nullableEnumType,
			ParameterExpression x)
		{
			if (nullableEnumType == null) throw new ArgumentNullException(nameof(nullableEnumType));
			var nullableUnderlyingType = Nullable.GetUnderlyingType(nullableEnumType);

			if (nullableUnderlyingType == null || !nullableUnderlyingType.IsEnum)
			{
				throw new ArgumentException("It is required to pass nullable enum type.", nameof(nullableEnumType));
			}

			var enumUnderlyingType = Enum.GetUnderlyingType(nullableUnderlyingType);

			Debug.Assert(enumUnderlyingType != null, "underlyingType != null");

			var getHashCodeMethod = enumUnderlyingType.GetMethod(nameof(GetHashCode), new Type[0]);

			if (getHashCodeMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(getHashCodeMethod)} method.");
			}

			var hasValueMethodName = nameof(Nullable<int>.HasValue);
			var valueMethodName = nameof(Nullable<int>.Value);

			var specialNullHashCode = nullableUnderlyingType.AssemblyQualifiedName?.GetHashCode() ??
			                          nullableUnderlyingType.GetHashCode();

			return Expression.Condition(
				// test
				Expression.Equal(
					Expression.Constant(true),
					Expression.PropertyOrField(x, hasValueMethodName)),
				// true part
				Expression.Call(
					Expression.Convert(
						Expression.PropertyOrField(x, valueMethodName),
						enumUnderlyingType),
					getHashCodeMethod),
				// false part
				Expression.Constant(specialNullHashCode, typeof(int)));
		}
	}

	public static class HashCodeExpressions<T>
	{
		public static Expression<Func<T, int>> CreateHashCodeExpression(MemberInfo memberInfo)
		{
			if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

			var x = Expression.Parameter(typeof(T), "x");

			return Expression.Lambda<Func<T, int>>(CreateHashCodeExpression(memberInfo, x), x);
		}

		public static Expression CreateHashCodeExpression([NotNull] MemberInfo memberInfo, [NotNull] ParameterExpression x)
		{
			if (memberInfo == null)
				throw new ArgumentNullException(nameof(memberInfo));
			if (x == null)
				throw new ArgumentNullException(nameof(x));
			if (!typeof(T).IsAssignableFrom(x.Type))
				throw new ArgumentException($"Expected that parameter returns type assignable from {typeof(T)}.", nameof(x));

			Type memberType;
			if (memberInfo is PropertyInfo propertyInfo)
			{
				memberType = propertyInfo.PropertyType;
			}
			else if (memberInfo is FieldInfo fieldInfo)
			{
				memberType = fieldInfo.FieldType;
			}
			else
			{
				throw new ArgumentException("Expected PropertyInfo of FieldInfo instance.", nameof(memberInfo));
			}

			Expression getHashCodeFunc;

			if (memberType == typeof(string))
			{
				getHashCodeFunc = CreateForStrings(memberInfo, memberType, x);
			}
			else if (memberType.IsEnum)
			{
				getHashCodeFunc = CreateForEnums(memberInfo, memberType, x);
			}
			else if (memberType.IsBuiltInPrimitive() || memberType.IsGenericAssignable(typeof(IEquatable<>)))
			{
				getHashCodeFunc = CreateForBuiltInGetHashCodeMethod(memberInfo, memberType, x);
			}
			else if (memberType.IsNullable(out var underlyingType))
			{
				getHashCodeFunc = underlyingType.IsEnum
					? CreateForNullableEnums(memberInfo, memberType, x) as Expression
					: CreateForDefaultEqualityComparer(memberInfo, memberType, x);
			}
			else if (memberType.IsCollection(out Type elementType) ||
			         memberType.IsReadOnlyDictionary(out _, out elementType))
			{
				getHashCodeFunc = GetHashCodeExpressionForCollection(memberInfo, memberType, elementType, x);
			}
			// TODO: what about value types (structs) without IEquatable support? I thing we need to use default equality comparer
			else
			{
				getHashCodeFunc = CreateForObjectStructureEqualityComparer(memberInfo, memberType, x);
			}

			return getHashCodeFunc;
		}

		private static Expression GetHashCodeExpressionForCollection(MemberInfo memberInfo,
			Type collectionType, Type elementType, ParameterExpression x)
		{
			FieldInfo comparerField;
			if (!elementType.IsArray && collectionType == elementType.MakeArrayType() &&
			    (comparerField = typeof(PrimitiveTypeArrayEqualityComparers).GetFields().FirstOrDefault(field => field.Name == elementType.Name + "ArrayMemoryEqualityComparer")) != null)
			{
				return GetHashCodeExpressionForArrayOfPrimitiveTypes(memberInfo, elementType, comparerField, x);
			}

			return GetHashCodeExpressionForEnumerable(memberInfo, collectionType, elementType, x);
		}

		private static Expression GetHashCodeExpressionForArrayOfPrimitiveTypes(MemberInfo memberInfo,
			Type elementType, [NotNull] FieldInfo comparerField, ParameterExpression x)
		{
			if (comparerField == null) throw new ArgumentNullException(nameof(comparerField));

			var arrayType = elementType.MakeArrayType();

			var comparerValue = comparerField.GetValue(null) ??
			                    throw new InvalidOperationException(
				                    $"Can't get value for comparer of type '{comparerField.FieldType}'.");
			var comparerType = comparerValue.GetType();

			var getHashCodeMethod = comparerType.GetMethod(nameof(IEqualityComparer.GetHashCode), new[] { arrayType })
			                        ?? throw new InvalidOperationException(
				                        $"Can't find {nameof(IEqualityComparer.GetHashCode)} method.");

			var comparerExpression = Expression.Field(null, comparerField);

			return Expression.Call(
				comparerExpression,
				getHashCodeMethod,
				Expression.Convert(
					Expression.PropertyOrField(x, memberInfo.Name),
					arrayType));
		}

		private static MethodCallExpression GetHashCodeExpressionForEnumerable(MemberInfo memberInfo,
			Type collectionType, Type elementType, ParameterExpression x)
		{
			Type comparerType;

			if (elementType.MakeArrayType() == collectionType)
			{
				comparerType = typeof(ArrayEqualityComparer<>).MakeGenericType(elementType);
			}
			else if (typeof(List<>).MakeGenericType(elementType) == collectionType)
			{
				comparerType = typeof(ListEqualityComparer<>).MakeGenericType(elementType);
			}
			else if (collectionType.IsReadOnlyDictionary(out _, out var keyValuePairType))
			{
				comparerType = typeof(DictionaryEqualityComparer<,,>).MakeGenericType(
					keyValuePairType.GetGenericArguments()[0],
					keyValuePairType.GetGenericArguments()[1],
					collectionType);
			}
			else
			{
				comparerType = typeof(GenericCollectionEqualityComparer<>).MakeGenericType(elementType);
			}

			// we need to have high performance static instance property with name 'Instance' in each comparer type.
			// We must not spend time on creating instance of collection comparer.
			// Ensure that all collection comparers follow this rule.
			var comparerExpression = Expression.Property(null, comparerType, "Instance");

			var getHashCodeMethod =
				comparerType.GetMethod(nameof(IEqualityComparer.GetHashCode), new[] { collectionType })
				?? throw new InvalidOperationException($"Can't find {nameof(IEqualityComparer.GetHashCode)} method.");

			return Expression.Call(
				comparerExpression,
				getHashCodeMethod,
				Expression.Convert(Expression.PropertyOrField(x, memberInfo.Name), collectionType));
		}

		private static MethodCallExpression CreateForDefaultEqualityComparer(MemberInfo memberInfo,
			Type memberType, ParameterExpression x)
		{
			var equalityComparerType = typeof(EqualityComparer<>).MakeGenericType(memberType);

			var getHashCodeMethod = equalityComparerType.GetMethod(nameof(GetHashCode), new[] {memberType});

			if (getHashCodeMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(GetHashCode)} method.");
			}

			return Expression.Call(
				Expression.Property(null, equalityComparerType, nameof(EqualityComparer<T>.Default)),
				getHashCodeMethod,
				Expression.PropertyOrField(x, memberInfo.Name));
		}

		private static MethodCallExpression CreateForObjectStructureEqualityComparer(
			MemberInfo memberInfo, Type memberType, ParameterExpression x)
		{
			var equalityComparerType = typeof(ObjectStructureEqualityComparer<>).MakeGenericType(memberType);

			var getHashCodeMethod = equalityComparerType.GetMethod(nameof(GetHashCode), new[] {memberType});

			if (getHashCodeMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(GetHashCode)} method.");
			}

			return Expression.Call(
				Expression.Property(null, equalityComparerType, nameof(ObjectStructureEqualityComparer<T>.Instance)),
				getHashCodeMethod,
				Expression.PropertyOrField(x, memberInfo.Name));
		}

		private static Expression CreateForBuiltInGetHashCodeMethod(MemberInfo memberInfo,
			Type memberType, ParameterExpression x)
		{
			switch (Type.GetTypeCode(memberType))
			{
				case TypeCode.Byte:
				case TypeCode.Char:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.SByte:
				case TypeCode.UInt16:
					return Expression.Convert(Expression.PropertyOrField(x, memberInfo.Name), typeof(int));
			}

			var getHashCodeMethod = memberType.GetMethod(nameof(GetHashCode), new Type[0]);

			if (getHashCodeMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(GetHashCode)} method.");
			}

			return Expression.Call(Expression.PropertyOrField(x, memberInfo.Name), getHashCodeMethod);
		}

		private static ConditionalExpression CreateForStrings(MemberInfo memberInfo, Type memberType,
			ParameterExpression x)
		{
			var getHashCodeMethod = typeof(string).GetMethod(nameof(GetHashCode), new Type[0]);

			if (getHashCodeMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(getHashCodeMethod)} method.");
			}

			return Expression.Condition(
				// test
				Expression.NotEqual(
					Expression.PropertyOrField(x, memberInfo.Name),
					Expression.Constant(null, typeof(string))),
				// true part
				Expression.Call(
					Expression.PropertyOrField(x, memberInfo.Name),
					getHashCodeMethod),
				// false part
				Expression.Constant(0, typeof(int)));
		}

		private static MethodCallExpression CreateForEnums(MemberInfo memberInfo, Type memberType,
			ParameterExpression x)
		{
			if (!memberType.IsEnum)
			{
				throw new ArgumentException("It is required to pass enum type.", nameof(memberType));
			}

			var underlyingType = Enum.GetUnderlyingType(memberType);

			Debug.Assert(underlyingType != null, "underlyingType != null");

			var getHashMethod = underlyingType.GetMethod(nameof(GetHashCode), new Type[0]);

			if (getHashMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(GetHashCode)} method.");
			}

			return Expression.Call(
				Expression.Convert(
					Expression.PropertyOrField(x, memberInfo.Name),
					underlyingType),
				getHashMethod);
		}

		private static ConditionalExpression CreateForNullableEnums(MemberInfo memberInfo,
			Type memberType, ParameterExpression x)
		{
			var nullableUnderlyingType = Nullable.GetUnderlyingType(memberType);

			if (nullableUnderlyingType == null || !nullableUnderlyingType.IsEnum)
			{
				throw new ArgumentException("It is required to pass nullable enum type.", nameof(memberType));
			}

			var enumUnderlyingType = Enum.GetUnderlyingType(nullableUnderlyingType);

			Debug.Assert(enumUnderlyingType != null, "underlyingType != null");

			var getHashCodeMethod = enumUnderlyingType.GetMethod(nameof(GetHashCode), new Type[0]);

			if (getHashCodeMethod == null)
			{
				throw new InvalidOperationException(
					$"Can't get appropriate {nameof(MethodInfo)} for {nameof(getHashCodeMethod)} method.");
			}

			var hasValueMethodName = nameof(Nullable<int>.HasValue);
			var valueMethodName = nameof(Nullable<int>.Value);

			var specialNullHashCode = nullableUnderlyingType.AssemblyQualifiedName?.GetHashCode() ??
			                          nullableUnderlyingType.GetHashCode();

			return Expression.Condition(
				// test
				Expression.Equal(
					Expression.Constant(true),
					Expression.PropertyOrField(
						Expression.PropertyOrField(x, memberInfo.Name),
						hasValueMethodName)),
				// true part
				Expression.Call(
					Expression.Convert(
						Expression.PropertyOrField(
							Expression.PropertyOrField(x, memberInfo.Name),
							valueMethodName),
						enumUnderlyingType),
					getHashCodeMethod),
				// false part
				Expression.Constant(specialNullHashCode, typeof(int)));
		}
	}
}