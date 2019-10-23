using System;
using System.Collections.Generic;
using CodeMania.Core.EqualityComparers.Common.ReferenceType;
using CodeMania.Core.EqualityComparers.Common.ValueType;
using CodeMania.Core.EqualityComparers.Specialized;
using CodeMania.Core.Extensions;
using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers
{
	public static class EqualityComparerTypeProvider
	{
		public static Type GetCustomEqualityComparerTypeFor([NotNull] Type itemType, IEqualityComparisonConfiguration equalityComparisonConfiguration = null)
		{
			equalityComparisonConfiguration = equalityComparisonConfiguration ?? EqualityComparisonConfiguration.Default;

			if (itemType == null)
			{
				throw new ArgumentNullException(nameof(itemType));
			}

			// NOTE: Don't change 'if' blocks order.
			Type elementType;
			if (itemType.IsArray)
			{
				if (itemType.GetArrayRank() == 1)
				{
					elementType = itemType.GetElementType() ?? throw new InvalidOperationException($"Unexpected error occured. Can't get element type for '{itemType}'.");

					if (elementType.IsEnum)
					{
						return typeof(EnumArrayEqualityComparer<>).MakeGenericType(elementType);
					}

					if (elementType.IsGenericAssignable(typeof(IEquatable<>)))
					{
						if (equalityComparisonConfiguration.PreferBuiltInEquatableImplementation)
						{
							return elementType.IsValueType
								? typeof(EquatableValueTypeArrayEqualityComparer<>).MakeGenericType(elementType)
								: typeof(EquatableReferenceTypeArrayEqualityComparer<>).MakeGenericType(elementType);
						}

						if (elementType.IsValueType)
						{
							return TryGetUnmanagedTypeArrayEqualityComparerType(elementType, out var comparerType)
								? comparerType
								: typeof(EquatableValueTypeArrayEqualityComparer<>).MakeGenericType(elementType);
						}

						return typeof(EquatableReferenceTypeArrayEqualityComparer<>).MakeGenericType(elementType);
					}

					if (elementType.IsNullable(out var nullableUnderlyingType))
					{
						return typeof(NullableValueTypeArrayEqualityComparer<>).MakeGenericType(nullableUnderlyingType);
					}

					return typeof(ReferenceTypeArrayEqualityComparer<>).MakeGenericType(elementType);
				}

				return typeof(MultiDimensionalArrayEqualityComparer);
			}

			if (itemType.IsGenericAssignable(typeof(List<>), out var listType))
			{
				elementType = listType.GetGenericArguments()[0];

				if (elementType.IsNullable(out var nullabelUnderlyingType))
				{
					return typeof(NullableValueTypeListEqualityComparer<>).MakeGenericType(nullabelUnderlyingType);
				}

				if (elementType.IsValueType)
				{
					return typeof(ValueTypeListEqualityComparer<>).MakeGenericType(elementType);
				}

				return typeof(ReferenceTypeListEqualityComparer<>).MakeGenericType(elementType);
			}

			if (itemType.IsGenericAssignable(typeof(IReadOnlyDictionary<,>), out var dictionaryType))
			{
				var kvpGenericArgs = dictionaryType.GetGenericArguments();
				var keyType = kvpGenericArgs[0];
				var valueType = kvpGenericArgs[1];
				return typeof(DictionaryEqualityComparer<,,>).MakeGenericType(keyType, valueType, itemType);
			}

			if (itemType.IsCollection(out elementType))
			{
				if (elementType.IsNullable(out var nullabelUnderlyingType))
				{
					return typeof(NullableValueTypeCollectionEqualityComparer<>).MakeGenericType(nullabelUnderlyingType);
				}

				if (elementType.IsValueType)
				{
					return typeof(ValueTypeCollectionEqualityComparer<>).MakeGenericType(elementType);
				}

				return typeof(ReferenceTypeCollectionEqualityComparer<>).MakeGenericType(elementType);
			}

			return typeof(ObjectStructureEqualityComparer<>).MakeGenericType(itemType);
		}

		private static bool TryGetUnmanagedTypeArrayEqualityComparerType(Type elementType, out Type comparerType)
		{
			try
			{
				comparerType = typeof(UnmanagedTypeArrayEqualityComparer<>).MakeGenericType(elementType);
				return true;
			}
			catch (Exception)
			{
				comparerType = null;
				return false;
			}
		}
	}
}