using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace CodeMania.Core.Benchmarks.Serializers
{
	/// <summary>
	/// Internal use only
	/// </summary>
	public class ReflectionBasedQueryStringSerializer<T>
	{
		private static readonly List<PropertyInfo> Properties;
		private static readonly Dictionary<string, string> escapedPropertyNames;
		private readonly SerializationContextPool pool;

		static ReflectionBasedQueryStringSerializer()
		{
			Properties = typeof(T).GetProperties().OrderBy(x => x.Name).ToList();
			escapedPropertyNames =
				Properties.ToDictionary(x => x.Name, x => Uri.EscapeDataString(x.Name), StringComparer.Ordinal);
		}

		public ReflectionBasedQueryStringSerializer()
		{
			pool = new SerializationContextPool(512);
		}

		public string Serialize(T obj)
		{
			if (obj == null) return string.Empty;

			var builder = pool.Rent();
			try
			{
				foreach (var propertyInfo in Properties)
				{
					Serialize(propertyInfo.Name, propertyInfo.GetValue(obj), builder);
				}

				return builder.ToString(0, builder.Length - 1);
			}
			finally
			{
				builder.Clear();
				pool.Return(builder);
			}
		}

		public void Serialize(string propertyName, object value, StringBuilder builder)
		{
			if (value == null) return;

			var enumerable = value as IEnumerable;
			if (enumerable != null)
			{
				foreach (var obj in enumerable)
				{
					Serialize(propertyName, obj, builder);

					builder.Append('&');
				}

				return;
			}

			builder.Append(escapedPropertyNames[propertyName]).Append('=');

			if (value is DateTime dt)
			{
				WriteValue(dt.ToString("s"), builder);
			}
			else if (value is DateTimeOffset dto)
			{
				WriteValue(dto.UtcDateTime.ToString("s") + "Z", builder);
			}
			else if (value is float f)
			{
				WriteValue(f.ToString(NumberFormatInfo.InvariantInfo), builder);
			}
			else if (value is double d)
			{
				WriteValue(d.ToString(NumberFormatInfo.InvariantInfo), builder);
			}
			else if (value is decimal dec)
			{
				WriteValue(dec.ToString(NumberFormatInfo.InvariantInfo), builder);
			}
			else
			{
				WriteValue(value.ToString(), builder);
			}
		}

		private void WriteValue(string value, StringBuilder builder)
		{
			builder.Append(Uri.EscapeDataString(value));
		}

		#region Nested Types

		private sealed class SerializationContextPool
		{
			private readonly int maxSize;
			private readonly ConcurrentQueue<StringBuilder> pool;
			private int count;

			public SerializationContextPool(int maxSize)
			{
				this.maxSize = maxSize;
				pool = new ConcurrentQueue<StringBuilder>(Enumerable.Range(0, maxSize).Select(x => new StringBuilder(4096)));
			}

			public StringBuilder Rent()
			{
				StringBuilder context;
				if (pool.TryDequeue(out context))
				{
					Interlocked.Decrement(ref count);
					return context;
				}

				return new StringBuilder();
			}

			public void Return(StringBuilder context)
			{
				if (count < maxSize)
				{
					pool.Enqueue(context);
					Interlocked.Increment(ref count);
				}
			}
		}

		#endregion
	}
}