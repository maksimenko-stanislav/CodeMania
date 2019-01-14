using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CodeMania.Core.Extensions;

namespace CodeMania.Core.Serialization
{
	public abstract class PropertyContext<T>
	{
		public readonly PropertyInfo PropertyInfo;
		private string serializationName;
		private readonly Type collectionElementType;

		protected Action<T, QueryStringWriter> Serializer;
		internal Func<T, bool> ShouldSerializeFunc;

		protected PropertyContext(PropertyInfo propertyInfo)
		{
			PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
			SerializationName = propertyInfo.Name;
			IsCollection = propertyInfo.PropertyType != typeof(string) && propertyInfo.PropertyType.IsCollection(out collectionElementType);
		}

		public bool IsCollection { get; }
		public Type CollectionElementType => collectionElementType;

		public string SerializationName
		{
			get { return serializationName ?? PropertyInfo.Name; }
			internal set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException("Value is null or empty (whitespace) string.", nameof(value));
				}

				serializationName = value;
			}
		}

		public static PropertyContext<T> Create(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
				throw new ArgumentNullException(nameof(propertyInfo));
			if (!propertyInfo.IsInstancePropertyOf(typeof(T)))
				throw new ArgumentException("", nameof(propertyInfo));

			return (PropertyContext<T>) Activator.CreateInstance(
				typeof(PropertyContext<,>).MakeGenericType(typeof(T), propertyInfo.PropertyType), propertyInfo);
		}

		public abstract Expression GetSerializerExpression(ParameterExpression objParameter, ParameterExpression writerParameter);
	}

	public sealed class PropertyContext<T, TProperty> : PropertyContext<T>
	{
		public PropertyContext(string propertyName)
			: this(typeof(T).GetProperty(propertyName))
		{
		}

		public PropertyContext(Expression<Func<T, TProperty>> propertyExpression)
			: this(propertyExpression.GetPropertyInfo())
		{
		}

		public PropertyContext(PropertyInfo propertyInfo)
			: base(propertyInfo)
		{
		}

		public Expression<ValueConverter<T, TProperty, string>> ScalarValueConverter { get; internal set; }

		public Expression<ValueConverter<T, TProperty, IEnumerable<string>>> CollectionValueConverter { get; internal set; }

		public Expression<Func<T, bool>> CustomShouldSerializeExpression { get; internal set; }

		public override Expression GetSerializerExpression(ParameterExpression objParameter, ParameterExpression writerParameter)
		{
			if (objParameter == null)
				throw new ArgumentNullException(nameof(objParameter));
			if (writerParameter == null)
				throw new ArgumentNullException(nameof(writerParameter));
			if (!typeof(T).IsAssignableFrom(objParameter.Type))
				throw new ArgumentException($"Unexpected parameter type: {objParameter.Type}.", nameof(objParameter));
			if (!typeof(QueryStringWriter).IsAssignableFrom(writerParameter.Type))
				throw new ArgumentException($"Unexpected parameter type: {writerParameter.Type}.", nameof(objParameter));

			Type propertyType;

			Expression getPropertyValueExpression;
			MethodInfo writerMethod;

			var propertyValueExpression = Expression.Property(objParameter, PropertyInfo);

			if (CollectionValueConverter != null)
			{
				propertyType = CollectionValueConverter.ReturnType; // expected typeof(IEnumerable<string>)
				getPropertyValueExpression = Expression.Invoke(
					CollectionValueConverter,
					objParameter,
					propertyValueExpression);

				writerMethod = typeof(QueryStringWriter).GetMethod(nameof(QueryStringWriter.WriteProperty), new[] { typeof(string), propertyType });
			}
			else if (ScalarValueConverter != null)
			{
				propertyType = ScalarValueConverter.ReturnType; // expected typeof(string)
				getPropertyValueExpression = Expression.Invoke(
					ScalarValueConverter,
					objParameter,
					propertyValueExpression);

				writerMethod = typeof(QueryStringWriter).GetMethod(nameof(QueryStringWriter.WriteProperty), new[] { typeof(string), propertyType });
			}
			else
			{
				propertyType = IsCollection ? typeof(IEnumerable<>).MakeGenericType(CollectionElementType) : typeof(TProperty);
				getPropertyValueExpression = propertyValueExpression;

				writerMethod =
					typeof(QueryStringWriter).GetMethod(
						nameof(QueryStringWriter.WriteProperty),
						BindingFlags.ExactBinding | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
						null,
						new[] { typeof(string), typeof(TProperty) },
						new ParameterModifier[0])
						??
					typeof(QueryStringWriter).GetMethod(
						nameof(QueryStringWriter.WriteProperty),
						BindingFlags.ExactBinding | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
						null,
						new[] {typeof(string), propertyType},
						new ParameterModifier[0]);
			}

			if (writerMethod == null)
			{
				throw new InvalidOperationException(
					"Can't find appropriate method for serialization. " +
				    $"Property type: {propertyType.FullName}, property name: {PropertyInfo.Name}. " +
					$"To avoid this configure serialization by calling methods of {nameof(QueryStringSerializerBuilder<T>)}: " +
					$"{nameof(QueryStringSerializerBuilder<T>.ConvertPropertyValueWith)}, {nameof(QueryStringSerializerBuilder<T>.ConvertPropertyValueWith)}" +
					$"{nameof(QueryStringSerializerBuilder<T>.WithConverter)}, {nameof(QueryStringSerializerBuilder<T>.WithConverter)}.");
			}

			return Expression.IfThen(
				GetShouldSerializeExpression(objParameter, propertyValueExpression),
				Expression.Call(
					writerParameter,
					writerMethod,
					Expression.Constant(
						string.Intern(Uri.EscapeDataString(SerializationName)), // cache escaped property name
						typeof(string)),
					getPropertyValueExpression
				)
			);
		}

		private Expression GetShouldSerializeExpression(ParameterExpression objParameter, MemberExpression propertyExpression)
		{
			// defensive reference copy
			var existingExpression = CustomShouldSerializeExpression;
			if (existingExpression != null)
			{
				return Expression.Invoke(existingExpression, objParameter);
			}

			// null for nullables and reference types, default value for value types, i.e. 0 for integral and floating point types, false for bool and so on.
			var propertyDefaultValue = Expression.Default(typeof(TProperty));

			if (IsCollection)
			{
				var countProperty = typeof(TProperty).GetProperty(nameof(ICollection<object>.Count), typeof(int));

				if (countProperty != null)
				{
					// obj => obj.CustomCollectionProperty != default(TProperty) && obj.CustomCollectionProperty.Count > 0;
					return Expression.AndAlso(
							Expression.NotEqual(
								propertyExpression,
								propertyDefaultValue),
							Expression.GreaterThan(
								Expression.Property(propertyExpression, countProperty),
								Expression.Constant(0, typeof(int)))
					);
				}
			}

			// obj => obj.CustomCollectionProperty != default(TProperty)
			return Expression.NotEqual(propertyExpression, propertyDefaultValue);
		}
	}
}