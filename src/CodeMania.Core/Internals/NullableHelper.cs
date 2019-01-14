using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace CodeMania.Core.Internals
{
	internal static class NullableHelper
	{
		public static bool CanBeNull(Type type) => !type.IsValueType || Nullable.GetUnderlyingType(type) != null;

		public static LambdaExpression GetIsNullExpression(Type type)
		{
			var x = Expression.Parameter(type, "x");

			if (CanBeNull(type))
			{
				return Expression.Lambda(
					typeof(Func<,>).MakeGenericType(type, typeof(bool)),
					Expression.Equal(x, Expression.Default(type)), x);
			}

			return Expression.Lambda(
				typeof(Func<,>).MakeGenericType(type, typeof(bool)),
				Expression.Constant(false), x);
		}

		public static Expression<Func<T, bool>> GetIsNullExpression<T>()
		{
			if (CanBeNull(typeof(T)))
			{
				var x = Expression.Parameter(typeof(T), "x");

				return Expression.Lambda<Func<T, bool>>(Expression.Equal(x, Expression.Default(typeof(T))), x);
			}

			return x => false;
		}
	}

	public static class GetterExpressions
	{
		public static Expression<Func<T, object>> CreateGetterExpression<T>([NotNull] FieldInfo fieldInfo) =>
			CreateGetterExpression<T>(fieldInfo, Expression.Parameter(typeof(T), "x"));

		public static Expression<Func<T, object>> CreateGetterExpression<T>([NotNull] PropertyInfo propertyInfo) =>
			CreateGetterExpression<T>(propertyInfo, Expression.Parameter(typeof(T), "x"));

		public static Expression<Func<T, object>> CreateGetterExpression<T>(
			[NotNull] FieldInfo fieldInfo,
			[NotNull] ParameterExpression obj) =>
			Expression.Lambda<Func<T, object>>(
				Expression.Convert(CreateMemberExpression<T>(fieldInfo, obj), typeof(object)),
				obj);

		public static Expression<Func<T, object>> CreateGetterExpression<T>(
			[NotNull] PropertyInfo propertyInfo,
			[NotNull] ParameterExpression obj) =>
			Expression.Lambda<Func<T, object>>(
				Expression.Convert(CreateMemberExpression<T>(propertyInfo, obj), typeof(object)),
				obj);

		public static Expression<Func<T, TResult>> CreateGetterExpression<T, TResult>([NotNull] FieldInfo fieldInfo) =>
			CreateGetterExpression<T, TResult>(fieldInfo, Expression.Parameter(typeof(T), "x"));

		public static Expression<Func<T, TResult>> CreateGetterExpression<T, TResult>([NotNull] PropertyInfo propertyInfo) =>
			CreateGetterExpression<T, TResult>(propertyInfo, Expression.Parameter(typeof(T), "x"));

		public static Expression<Func<T, TResult>> CreateGetterExpression<T, TResult>(
			[NotNull] FieldInfo fieldInfo,
			[NotNull] ParameterExpression obj) =>
			Expression.Lambda<Func<T, TResult>>(
				CreateMemberExpression<T>(fieldInfo, obj),
				obj);

		public static Expression<Func<T, TResult>> CreateGetterExpression<T, TResult>(
			[NotNull] PropertyInfo propertyInfo,
			[NotNull] ParameterExpression obj) =>
			Expression.Lambda<Func<T, TResult>>(
				CreateMemberExpression<T>(propertyInfo, obj),
				obj);

		public static MemberExpression CreateMemberExpression<T>([NotNull] FieldInfo fieldInfo,
			[NotNull] ParameterExpression obj)
		{
			CheckParameters<T>(fieldInfo, obj);

			return Expression.Field(obj, fieldInfo);
		}

		public static MemberExpression CreateMemberExpression<T>([NotNull] PropertyInfo propertyInfo,
			[NotNull] ParameterExpression obj)
		{
			CheckParameters<T>(propertyInfo, obj);

			return Expression.Property(obj, propertyInfo);
		}

		private static void CheckParameters<T>([NotNull] MemberInfo memberInfo, [NotNull] ParameterExpression obj)
		{
			const BindingFlags bindingFlags =
				BindingFlags.FlattenHierarchy
				| BindingFlags.Instance
				| BindingFlags.Public
				| BindingFlags.GetField
				| BindingFlags.GetProperty;

			if (memberInfo == null)
				throw new ArgumentNullException(nameof(memberInfo));
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));
			if (!typeof(T).GetMember(memberInfo.Name, memberInfo.MemberType, bindingFlags).Contains(memberInfo))
				throw new ArgumentException($"Provided {nameof(MemberInfo)} doesn't belong to type {typeof(T)}.", nameof(memberInfo));
			if (!typeof(T).IsAssignableFrom(obj.Type))
				throw new ArgumentException("Parameter type is not supported.", nameof(obj));
		}
	}
}