using System;
using System.Collections.Generic;

namespace CodeMania.Core
{
	internal static class CustomConversions
	{
		private static readonly Dictionary<(Type from, Type to), Delegate> Delegates;

		static CustomConversions()
		{
			Delegates = new Dictionary<(Type from, Type to), Delegate>
			{
				[(typeof(DateTime), typeof(double))] = new Func<DateTime, double>(DateTimeToDouble),
				[(typeof(double), typeof(DateTime))] = new Func<double, DateTime>(DoubleToDateTime),
				[(typeof(TimeSpan), typeof(double))] = new Func<TimeSpan, double>(TimeSpanToDouble),
                [(typeof(double), typeof(TimeSpan))] = new Func<double, TimeSpan>(DoubleToTimeSpan) 
			};
		}

		public static bool Contains<TFrom, TTo>() => Delegates.ContainsKey((typeof(TFrom), typeof(TTo)));

		public static Func<TFrom, TTo> GetDelegate<TFrom, TTo>()
		{
            var pair = (typeof(TFrom), typeof(TTo));
            if (Delegates.TryGetValue(pair, out var result))
			{
				return (Func<TFrom, TTo>) result;
			}

			throw new KeyNotFoundException($"Can't find value for key {pair}.");
		}

		private static double DateTimeToDouble(DateTime d) => d.ToOADate();

		private static DateTime DoubleToDateTime(double d) => DateTime.FromOADate(d);

		private static double TimeSpanToDouble(TimeSpan d) => d.TotalMilliseconds;

		private static TimeSpan DoubleToTimeSpan(double d) => TimeSpan.FromMilliseconds(d);
	}
}