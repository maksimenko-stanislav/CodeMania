using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace CodeMania.Core.Extensions
{
	public static class TypeExtensions
	{
		public static bool IsBuiltInPrimitive(this Type type) =>
			(type ?? throw new ArgumentNullException(nameof(type))).IsPrimitive
			|| type == typeof(DateTime)
			|| type == typeof(DateTimeOffset)
			|| type == typeof(TimeSpan)
			|| type == typeof(Guid);

		/// <summary>
		/// Gets type of collection element if <paramref name="type"/> supports <see cref="IEnumerable{T}"/>.
		/// Returns null if type doesn't support <see cref="IEnumerable{T}"/> or implements it for different element types.
		/// </summary>
		/// <param name="type">Type to determine collection element.</param>
		/// <returns>Type of element in collection, if collection supports only one type of element, otherwise false.</returns>
		[CanBeNull]
		public static Type GetCollectionElementType([NotNull] this Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));

			IList<Type> collectionElementTypes = type.GetInterfaces()
				.Where(x => x != typeof(IEnumerable) && x.IsGenericAssignable(typeof(IEnumerable<>)))
				.Select(x => x.GetGenericArguments()[0])
				.Distinct()
				.ToList();

			if (collectionElementTypes.Count == 1)
			{
				return collectionElementTypes[0];
			}

			return null;
		}

		public static bool IsCollection([NotNull] this Type type, out Type collectionElementType)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));

			IList<Type> collectionElementTypes = type.GetInterfaces()
				.Where(x => x != typeof(IEnumerable) && x.IsGenericAssignable(typeof(IEnumerable<>)))
				.Select(x => x.GetGenericArguments()[0])
				.Distinct()
				.ToList();

			if (collectionElementTypes.Count == 1)
			{
				collectionElementType = collectionElementTypes[0];
				return true;
			}

			collectionElementType = null;
			return false;
		}

		public static bool IsNullable(this Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));

			return Nullable.GetUnderlyingType(type) != null;
		}

		public static bool IsNullable(this Type type, out Type underlyingType)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));

			underlyingType = Nullable.GetUnderlyingType(type);

			return underlyingType != null;
		}

		public static bool IsGenericAssignable(this Type type, Type openGenericType) => IsGenericAssignable(type, openGenericType, out _);

		public static bool IsGenericAssignable(this Type type, Type openGenericType, out Type foundSupportedType)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));
			if (openGenericType == null)
				throw new ArgumentNullException(nameof(openGenericType));
			if (!openGenericType.IsGenericTypeDefinition)
				throw new ArgumentException("Parameter must be open generic type.", nameof(openGenericType));

			if (type.IsAssignableFrom(openGenericType))
			{
				foundSupportedType = null;
				return false;
			}

			if (openGenericType.IsInterface)
			{
				foreach (var @interface in type.GetInterfaces())
				{
					if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == openGenericType)
					{
						foundSupportedType = @interface;
						return true;
					}
				}
			}
			else
			{
				Type temp = type;

				while (temp != null)
				{
					if (temp.IsConstructedGenericType && temp.GetGenericTypeDefinition() == openGenericType)
					{
						foundSupportedType = temp;
						return true;
					}

					temp = temp.BaseType;
				}
			}

			foundSupportedType = null;

			return false;
		}

		public static bool IsReadOnlyDictionary(this Type type, out Type dictionaryType) =>
			IsGenericAssignable(type, typeof(IReadOnlyDictionary<,>), out dictionaryType);

		public static bool IsReadOnlyDictionary(this Type type, out Type dictionaryType, out Type keyValuePairType)
		{
			if (IsGenericAssignable(type, typeof(IReadOnlyDictionary<,>), out dictionaryType))
			{
				var genericArguments = dictionaryType.GetGenericArguments();

				keyValuePairType = typeof(KeyValuePair<,>).MakeGenericType(
					genericArguments[0],
					genericArguments[1]);

				return true;
			}

			keyValuePairType = null;
			return false;
		}

		public static MethodInfo GetPublicStaticMethod(this Type type, string name, params Type[] argumentTypes)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("The name of method can't be null or empty.", nameof(name));
			}

			return type.GetMethod(name, BindingFlags.Public | BindingFlags.Static, null, argumentTypes,
				new ParameterModifier[0]);
		}

		public static MethodInfo GetPublicInstanceMethod(this Type type, string name, params Type[] argumentTypes)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("The name of method can't be null or empty.", nameof(name));
			}

			return type.GetMethod(name, BindingFlags.Public | BindingFlags.Instance, null, argumentTypes,
				new ParameterModifier[0]);
		}

		public static bool IsWellKnownPrimitiveOrCollectionOfThesePrimitives(this Type type) =>
			(type ?? throw new ArgumentNullException(nameof(type))).IsBuiltInPrimitive()
			|| type == typeof(string)
			|| type.IsEnum
			|| type.IsNullable()
			|| type.IsGenericAssignable(typeof(IEquatable<>))
			|| (type.IsGenericAssignable(typeof(IReadOnlyDictionary<,>), out var keyValuePairType)
			    && IsWellKnownPrimitiveOrCollectionOfThesePrimitives(keyValuePairType.GetGenericArguments()[0])
			    && IsWellKnownPrimitiveOrCollectionOfThesePrimitives(keyValuePairType.GetGenericArguments()[1]))
			|| (type.IsCollection(out var collectionElementType)
			    && IsWellKnownPrimitiveOrCollectionOfThesePrimitives(collectionElementType));

		public static PropertyInfo[] GetPublicInstanceProperties([NotNull] this Type type) =>
			(type ?? throw new ArgumentNullException(nameof(type)))
				.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

		public static FieldInfo[] GetPublicInstanceFields([NotNull] this Type type) =>
			(type ?? throw new ArgumentNullException(nameof(type)))
				.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
	}
}