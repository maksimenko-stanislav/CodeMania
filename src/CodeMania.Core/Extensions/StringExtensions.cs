using System;
using System.Globalization;

namespace CodeMania.Core.Extensions
{
	public static class StringExtensions
	{
		public static T Parse<T>(this string value, T fallback = default(T), bool throwOnError = false, string format = null, IFormatProvider formatProvider = null, Func<string, T> customParseFunc = null)
		{
			Type nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(T));

			if (nullableUnderlyingType != null && string.IsNullOrWhiteSpace(value))
			{
				return fallback;
			}

			if (typeof(T) == typeof(string))
			{
				return (T) (object) value;
			}

			try
			{
				if (customParseFunc != null)
				{
					return customParseFunc(value);
				}

				return (T) ParseInternal(value,
					nullableUnderlyingType ?? typeof(T),
					format,
					formatProvider ?? CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				if (throwOnError)
					throw;

				return fallback;
			}
		}

		private static object ParseInternal(string value, Type type, string format, IFormatProvider formatProvider)
		{
			if (type.IsEnum)
			{
				return ParseEnumInternal(value, type);
			}

			if (type == typeof(TimeSpan))
			{
				return format != null ? TimeSpan.ParseExact(value, format, formatProvider) : TimeSpan.Parse(value, formatProvider);
			}

			if (type == typeof(Guid))
			{
				return format != null ? Guid.ParseExact(value, format) : Guid.Parse(value);
			}

			if (type == typeof(DateTimeOffset))
			{
				return format != null ? DateTimeOffset.ParseExact(value, format, formatProvider) : DateTimeOffset.Parse(value, formatProvider);
			}

			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Boolean:
					return bool.Parse(value);
				case TypeCode.Char:
					return value[0];
				case TypeCode.SByte:
					return sbyte.Parse(value);
				case TypeCode.Byte:
					return byte.Parse(value);
				case TypeCode.Int16:
					return short.Parse(value);
				case TypeCode.UInt16:
					return ushort.Parse(value);
				case TypeCode.Int32:
					return int.Parse(value);
				case TypeCode.UInt32:
					return uint.Parse(value);
				case TypeCode.Int64:
					return long.Parse(value);
				case TypeCode.UInt64:
					return ulong.Parse(value);
				case TypeCode.Single:
					return float.Parse(value, formatProvider);
				case TypeCode.Double:
					return double.Parse(value, formatProvider);
				case TypeCode.Decimal:
					return decimal.Parse(value, formatProvider);
				case TypeCode.DateTime:
					return format != null ? DateTime.ParseExact(value, format, formatProvider) : DateTime.Parse(value, formatProvider);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static object ParseEnumInternal(string value, Type type)
		{
			return Enum.Parse(type, value, ignoreCase: true);
		}
	}
}