using System;
using System.Collections.Generic;

namespace CodeMania.Core.Maths
{
	public static class GenericMath
	{
		public static T Abs<T>(T value)
		{
			Numeric<T> numeric = value;

			return numeric >= default(T) ? numeric : -numeric;
		}

		// TODO: Add overloads for ICollection<T> and IList<T> for each method below

		public static T Sum<T>(IEnumerable<T> source)
		{
			Numeric<T> sum = default(T);

			foreach (var item in source)
			{
				sum += item;
			}

			return sum;
		}

		public static T Sum<T, TCollection>(TCollection source)
			where TCollection : IEnumerable<T>
		{
			Numeric<T> sum = default(T);

			foreach (var item in source)
			{
				sum += item;
			}

			return sum;
		}

		public static T Avg<T>(IEnumerable<T> source)
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements.");
				}

				Numeric<T> sum = enumerator.Current;
				int count = 1;

				while (enumerator.MoveNext())
				{
					sum += enumerator.Current;
					count++;
				}

				return (Numeric<T>) ((double) sum / count);
			}
		}

		public static T Min<T>(IEnumerable<T> source)
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements.");
				}

				Numeric<T> min = enumerator.Current;

				while (enumerator.MoveNext())
				{
					if (min > enumerator.Current) min = enumerator.Current;
				}

				return min;
			}
		}

		public static T Max<T>(IEnumerable<T> source)
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements.");
				}

				Numeric<T> max = enumerator.Current;

				while (enumerator.MoveNext())
				{
					if (max < enumerator.Current) max = enumerator.Current;
				}

				return max;
			}
		}
	}
}