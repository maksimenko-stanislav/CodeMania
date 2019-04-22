using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CodeMania.Core.Extensions;
using CodeMania.Core.Serialization.Converters;
using JetBrains.Annotations;

namespace CodeMania.Core.Serialization
{
	/// <summary>
	/// Contains methods for creating <see cref="QueryStringSerializerBuilder{T}"/> instances.
	/// </summary>
	[PublicAPI]
	public static class QueryStringSerializerBuilder
	{
		/// <summary>
		/// Creates new instance of <see cref="QueryStringSerializerBuilder{T}"/> with default settings.
		/// </summary>
		/// <typeparam name="T">Type to [de]serialize.</typeparam>
		/// <returns>New instance of <see cref="QueryStringSerializerBuilder{T}"/> with default settings.</returns>
		public static QueryStringSerializerBuilder<T> Create<T>() =>
			new QueryStringSerializerBuilder<T>(typeof(T).GetProperties().Select(PropertyConfiguration<T>.Create))
				.AdjustForEnumProperties();
	}

	/// <summary>
	/// Provides methods to configure query string serialization process.
	/// </summary>
	/// <typeparam name="T">Type to [de]serialize.</typeparam>
	[PublicAPI]
	public class QueryStringSerializerBuilder<T>
	{
		private const string ValueIsNullOrWhitespaceString = "Value cannot be null or whitespace.";

		private readonly Dictionary<string, PropertyConfiguration<T>> propertyContexts;

		internal QueryStringSerializerBuilder(IEnumerable<PropertyConfiguration<T>> propertyContexts)
		{
			this.propertyContexts =
				propertyContexts.ToDictionary(x => x.PropertyInfo.Name, x => x, StringComparer.Ordinal);
		}

		public QueryStringSerializer<T> Build()
		{
			return new QueryStringSerializer<T>(propertyContexts.Values);
		}

		public QueryStringSerializerBuilder<T> Ignore<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
		{
			var propertyName = propertyExpression.GetFieldOrPropertyName();

			if (!propertyContexts.Remove(propertyName))
			{
				throw new InvalidOperationException(
					$"Type '{typeof(T).FullName} doesn't contain property '{propertyName}'");
			}

			return this;
		}

		public QueryStringSerializerBuilder<T> WithName<TProperty>(
			Expression<Func<T, TProperty>> propertyExpression,
			string newName)
		{
			if (string.IsNullOrWhiteSpace(newName))
				throw new ArgumentException(ValueIsNullOrWhitespaceString, nameof(newName));

			var propertyContext = GetPropertyContext(propertyExpression);

			propertyContext.SerializationName = newName;

			return this;
		}

		public QueryStringSerializerBuilder<T> WithName<TProperty>(
			Expression<Func<T, TProperty>> propertyExpression,
			Func<PropertyInfo, string> getNewName)
		{
			if (getNewName == null)
				throw new ArgumentException(ValueIsNullOrWhitespaceString, nameof(getNewName));

			var propertyContext = GetPropertyContext(propertyExpression);

			propertyContext.SerializationName = getNewName(propertyContext.PropertyInfo);

			return this;
		}

		public QueryStringSerializerBuilder<T> WithNames(
			Func<PropertyInfo, string> getNewName)
		{
			if (getNewName == null)
				throw new ArgumentException(ValueIsNullOrWhitespaceString, nameof(getNewName));

			foreach (var context in propertyContexts.Values)
			{
				context.SerializationName = getNewName(context.PropertyInfo);
			}

			return this;
		}

		public QueryStringSerializerBuilder<T> ConvertPropertyValueWith<TProperty>(
			Expression<Func<T, TProperty>> propertyExpression,
			IConverter<TProperty, string> converter)
		{
			if (converter == null) throw new ArgumentNullException(nameof(converter));

			return ConvertPropertyValueWith(propertyExpression, (obj, value) => converter.Convert(value));
		}

		public QueryStringSerializerBuilder<T> ConvertPropertyValueWith<TProperty>(
			Expression<Func<T, TProperty>> propertyExpression,
			Expression<ValueConverter<T, TProperty, string>> converter)
		{
			if (converter == null)
			{
				throw new ArgumentNullException(nameof(converter));
			}

			var propertyContext = GetPropertyContext(propertyExpression);

			if (propertyContext.IsCollection)
			{
				throw new InvalidOperationException(
					$"The property {typeof(T).FullName}.{propertyContext.PropertyInfo.Name} returns collection of elements instead of scalar value." +
					$"Call {nameof(WithConverter)} instead.");
			}

			propertyContext.ScalarValueConverter = converter;

			return this;
		}

		public QueryStringSerializerBuilder<T> ConvertPropertyValueWith<TProperty>(
			Expression<Func<T, TProperty>> propertyExpression,
			IConverter<TProperty, IEnumerable<string>> converter)
			where TProperty : IEnumerable
		{
			if (converter == null) throw new ArgumentNullException(nameof(converter));

			return ConvertPropertyValueWith(propertyExpression, (obj, value) => converter.Convert(value));
		}

		public QueryStringSerializerBuilder<T> ConvertPropertyValueWith<TProperty>(
			Expression<Func<T, TProperty>> propertyExpression,
			Expression<ValueConverter<T, TProperty, IEnumerable<string>>> converter)
			where TProperty : IEnumerable
		{
			if (converter == null)
			{
				throw new ArgumentNullException(nameof(converter));
			}

			var propertyContext = GetPropertyContext(propertyExpression);

			if (!propertyContext.IsCollection)
			{
				throw new InvalidOperationException(
					$"The property {typeof(T).FullName}.{propertyContext.PropertyInfo.Name} returns scalar value instead of collection." +
					$"Call {nameof(WithConverter)} instead.");
			}

			propertyContext.CollectionValueConverter = converter;

			return this;
		}

		public QueryStringSerializerBuilder<T> WithConverter<TProperty>(
			IConverter<TProperty, string> converter)
		{
			if (converter == null) throw new ArgumentNullException(nameof(converter));

			return WithConverter((TProperty value) => converter.Convert(value));
		}

		public QueryStringSerializerBuilder<T> WithConverter<TProperty>(
			Expression<ValueConverter<TProperty, string>> converter)
		{
			if (converter == null) throw new ArgumentNullException(nameof(converter));

			var propertyParameter = Expression.Parameter(typeof(TProperty), "value");
			var objParameter = Expression.Parameter(typeof(T), "obj");

			foreach (var pair in propertyContexts)
			{
				var propertyContext = pair.Value as PropertyConfiguration<T, TProperty>;

				if (propertyContext != null)
				{
					propertyContext.ScalarValueConverter =
						Expression.Lambda<ValueConverter<T, TProperty, string>>(
							Expression.Invoke(
								converter,
								Expression.MakeMemberAccess(objParameter, propertyContext.PropertyInfo)),
							objParameter,
							propertyParameter);
				}
			}

			return this;
		}

		public QueryStringSerializerBuilder<T> WithConverter<TProperty>(
			IConverter<TProperty, IEnumerable<string>> converter)
			where TProperty : IEnumerable
		{
			if (converter == null) throw new ArgumentNullException(nameof(converter));

			return WithConverter<TProperty>(value => converter.Convert(value));
		}

		public QueryStringSerializerBuilder<T> WithConverter<TProperty>(
			Expression<ValueConverter<TProperty, IEnumerable<string>>> converter)
			where TProperty : IEnumerable
		{
			if (converter == null) throw new ArgumentNullException(nameof(converter));

			var propertyParameter = Expression.Parameter(typeof(TProperty), "value");
			var objParameter = Expression.Parameter(typeof(T), "obj");

			foreach (var pair in propertyContexts)
			{
				var propertyContext = pair.Value as PropertyConfiguration<T, TProperty>;

				if (propertyContext != null)
				{
					propertyContext.CollectionValueConverter =
						Expression.Lambda<ValueConverter<T, TProperty, IEnumerable<string>>>(
							Expression.Invoke(
								converter,
								Expression.MakeMemberAccess(objParameter, propertyContext.PropertyInfo)),
							objParameter,
							propertyParameter);
				}
			}

			return this;
		}

		public QueryStringSerializerBuilder<T> ShouldSerializeDefaultValues(bool shouldSerialize)
		{
			Func<T, bool> shouldSerializeFunc = obj => shouldSerialize;

			foreach (var propertyContext in propertyContexts)
			{
				propertyContext.Value.ShouldSerializeFunc = shouldSerializeFunc;
			}

			return this;
		}

		internal QueryStringSerializerBuilder<T> AdjustForEnumProperties()
		{
			foreach (var pair in propertyContexts)
			{
				var propertyContext = pair.Value;

				var propertyInfo = propertyContext.PropertyInfo;

				var enumType = propertyContext.IsCollection
					? propertyContext.CollectionElementType
					: propertyInfo.PropertyType;

				var conversionResultType = propertyContext.IsCollection
					? typeof(IEnumerable<string>)
					: typeof(string);

				var nullableUnderlyingType = Nullable.GetUnderlyingType(enumType);

				Type converterType;

				if (enumType.IsEnum)
				{
					converterType = propertyContext.IsCollection
						? typeof(EnumCollectionToStringsConverter<>).MakeGenericType(enumType)
						: typeof(EnumToStringConverter<>).MakeGenericType(enumType);
				}
				else if (nullableUnderlyingType != null && nullableUnderlyingType.IsEnum)
				{
					converterType = propertyContext.IsCollection
						? typeof(NullableEnumCollectionToStringsConverter<>).MakeGenericType(nullableUnderlyingType)
						: typeof(NullableEnumToStringConverter<>).MakeGenericType(nullableUnderlyingType);
				}
				else
				{
					continue;
				}

				Type converterInterfaceType;
				converterType.IsGenericAssignable(typeof(IConverter<,>), out converterInterfaceType);

				var converterDefaultProperty = converterType.GetProperty("Default", BindingFlags.Public | BindingFlags.Static);

				if (converterDefaultProperty == null)
				{
					continue;
				}

				var objParameter = Expression.Parameter(typeof(T), "obj");

				// obj => obj.CustomProperty
				LambdaExpression propertyAccessor =
					Expression.Lambda(
						typeof(Func<,>).MakeGenericType(typeof(T), propertyInfo.PropertyType),
						Expression.MakeMemberAccess(objParameter, propertyInfo),
						objParameter);

				var methodName = nameof(ConvertPropertyValueWith);

				var withConverterMethodInfo = (
					from x in GetType().GetMethods(BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public)
					where x.Name == methodName
					let parameters = x.GetParameters()
					where
						x.IsGenericMethodDefinition
						&& parameters.Length == 2
						&& parameters[0].ParameterType.GetGenericTypeDefinition() == propertyAccessor.GetType().GetGenericTypeDefinition()
						&& parameters[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>)
						&& parameters[1].ParameterType.GetGenericArguments()[0].GetGenericArguments()[2] == conversionResultType
					select x
				).SingleOrDefault();

				if (withConverterMethodInfo == null)
				{
					continue;
				}

				var convertMethodInfo = converterType.GetMethod("Convert");

				if (convertMethodInfo == null)
				{
					continue;
				}

				var typeToConvert = propertyInfo.PropertyType;

				var propertyParameter = Expression.Parameter(typeToConvert, "x");

				var converterExpression =
					Expression.Lambda(
						typeof(ValueConverter<,,>).MakeGenericType(
							typeof(T),
							typeToConvert,
							converterInterfaceType.GetGenericArguments()[1]),
						Expression.Call(
							Expression.Property(null, converterDefaultProperty),
							convertMethodInfo,
							propertyParameter),
						objParameter,
						propertyParameter
					);

				withConverterMethodInfo
					.MakeGenericMethod(propertyInfo.PropertyType)
					.Invoke(this, new object[] { propertyAccessor, converterExpression });
			}

			return this;
		}

		private PropertyConfiguration<T, TProperty> GetPropertyContext<TProperty>(
			Expression<Func<T, TProperty>> propertyExpression)
		{
			if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));

			var propertyName = propertyExpression.GetFieldOrPropertyName();

			PropertyConfiguration<T> propertyConfiguration;
			if (!propertyContexts.TryGetValue(propertyName, out propertyConfiguration))
			{
				throw new InvalidOperationException($"Type '{typeof(T).FullName} doesn't contain property '{propertyName}'");
			}

			return (PropertyConfiguration<T, TProperty>) propertyConfiguration;
		}
	}
}