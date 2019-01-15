using System;
using System.Linq.Expressions;

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
}