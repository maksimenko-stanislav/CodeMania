using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace CodeMania.Core.Extensions
{
	public static class ExpressionExtensions
	{
		public static string GetFieldOrPropertyName<T, TProperty>([NotNull] this Expression<Func<T, TProperty>> memberAccessExpression)
		{
			if (memberAccessExpression == null) throw new ArgumentNullException(nameof(memberAccessExpression));

			var memberExpression = memberAccessExpression.Body as MemberExpression;

			if (memberExpression == null)
				throw new ArgumentException("Expression must represent field or property access expression.", nameof(memberAccessExpression));

			return memberExpression.Member.Name;
		}

		public static PropertyInfo GetPropertyInfo<T, TProperty>([NotNull] this Expression<Func<T, TProperty>> memberAccessExpression)
		{
			if (memberAccessExpression == null) throw new ArgumentNullException(nameof(memberAccessExpression));

			var propertyInfo = (memberAccessExpression.Body as MemberExpression)?.Member as PropertyInfo;

			if (propertyInfo == null)
				throw new ArgumentException("Expression must represent field or property access expression.", nameof(memberAccessExpression));

			return propertyInfo;
		}
	}
}