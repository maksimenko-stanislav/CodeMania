using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CodeMania.Core
{
	public static class UnsafeDynamicCast<TFrom, TTo>
	{
		private static readonly Func<TFrom, TTo> CastFunc;

		static UnsafeDynamicCast()
		{
			var sourceParam = Expression.Parameter(typeof(TFrom), "source");
			try
			{
				if (typeof(TFrom) == typeof(TTo))
				{
					// source => source
					CastFunc = ExpressionCompiler.Default.Compile(
						Expression.Lambda<Func<TFrom, TTo>>(sourceParam, sourceParam));
				}
				else if (CustomConversions.Contains<TFrom, TTo>())
				{
					CastFunc = CustomConversions.GetDelegate<TFrom, TTo>();
				}
				else if (!typeof(TFrom).IsEnum &&
				         !typeof(TTo).IsEnum &&
				         typeof(IConvertible).IsAssignableFrom(typeof(TFrom)) &&
				         typeof(IConvertible).IsAssignableFrom(typeof(TTo)))
				{
					// looking for appropriate methods of class System.Convert, like ToInt32, ToDouble and so on.
					var convertMethod = typeof(Convert).GetMethod("To" + typeof(TTo).Name,
						BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(TFrom) },
						new ParameterModifier[0]);

					if (convertMethod != null)
					{
						// source => Convert.ToXXX(source)
						CastFunc = ExpressionCompiler.Default.Compile(
							Expression.Lambda<Func<TFrom, TTo>>(
								Expression.Call(null, convertMethod, sourceParam),
								sourceParam
						));
					}
					else
					{
						// nonsense case, boxing and unboxing in case of value type:
						CastFunc = source => (TTo) Convert.ChangeType(source, typeof(TTo));
					}
				}
				else
				{
					// (source) => (TTo) source;
					// if you look at this and think why do we need it, just try to compile method "TTo Cast<TFrom, TTo>(TFrom source) => (TTo) source;" ;-)
					CastFunc = ExpressionCompiler.Default.Compile(
						Expression.Lambda<Func<TFrom, TTo>>(
							Expression.Convert(sourceParam, typeof(TTo)),
							sourceParam
					));
				}
			}
			catch (Exception)
			{
				CastFunc = source => throw new InvalidCastException($"Can't cast '{typeof(TFrom).AssemblyQualifiedName}' to type '{typeof(TTo).AssemblyQualifiedName}'.");
			}
		}

		/// <summary>
		/// Casts value of type <typeparamref name="TFrom"/> to value of type <typeparamref name="TTo"/> avoiding boxing/unboxing operations if possible.
		/// </summary>
		/// <param name="source">Value to cast.</param>
		///
		/// <exception cref="InvalidCastException">Cast from <typeparamref name="TFrom"/> to <typeparamref name="TTo"/> is not supported.</exception>
		/// <returns>Value or reference of type <typeparamref name="TTo"/>.</returns>
		public static TTo Cast(TFrom source) => CastFunc(source);
	}
}