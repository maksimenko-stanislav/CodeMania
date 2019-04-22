using System;
using System.Globalization;
using System.Reflection;
using CodeMania.Core.Utils;
using JetBrains.Annotations;

namespace CodeMania.Core.Serialization.Parsers
{
	// TODO: Use new Parse/TryParse API with Span/Memory support when porting to netstandard 2.0
	public static class ValueParser
	{
		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out char result)
		{
			if (!value.IsEmpty)
			{
				result = value.Span[0];

				return true;
			}

			result = default;
			return false;
		}

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out string result)
		{
			result = value.ToString();
			return true;
		}

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out bool result) =>
			bool.TryParse(value.ToString(), out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out sbyte result) =>
			sbyte.TryParse(value.ToString(), out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out byte result) =>
			byte.TryParse(value.ToString(), out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out short result) =>
			short.TryParse(value.ToString(), out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out int result) =>
			int.TryParse(value.ToString(), out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out long result) =>
			long.TryParse(value.ToString(), out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out ushort result) =>
			ushort.TryParse(value.ToString(), out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out uint result) =>
			uint.TryParse(value.ToString(), out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out ulong result) =>
			ulong.TryParse(value.ToString(), out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out float result) =>
			float.TryParse(value.ToString(), NumberStyles.Float, NumberFormatInfo.InvariantInfo, out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out double result) =>
			double.TryParse(value.ToString(), NumberStyles.Float, NumberFormatInfo.InvariantInfo, out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out decimal result) =>
			decimal.TryParse(value.ToString(), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out DateTime result) =>
			DateTime.TryParseExact(value.ToString(), "O", NumberFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal, out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out DateTimeOffset result) =>
			DateTimeOffset.TryParseExact(value.ToString(), "O", NumberFormatInfo.InvariantInfo, DateTimeStyles.AssumeLocal, out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out Guid result) =>
			Guid.TryParse(value.ToString(), out result);

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out TimeSpan result) =>
			TimeSpan.TryParse(value.ToString(), out result);
	}

	[UsedImplicitly]
	public static class ValueParser<T>
	{
		private static readonly TryParseDelegate<QueryStringSerializerSettings, T> TryParseMethod;

		static ValueParser()
		{
			TryParseMethod = CreateParseDelegate();
		}

		public static TryParseDelegate<QueryStringSerializerSettings, T> CreateParseDelegate()
		{
			var resultArgType = typeof(T).MakeByRefType();

			var tryParseMethod = typeof(ValueParser).GetMethod("TryParse",
				BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public,
				null,
				new[] { typeof(ReadOnlyMemory<char>), typeof(QueryStringSerializerSettings), resultArgType },
				new ParameterModifier[0]);

			return tryParseMethod != null
				? DelegateHelper.CreateDelegate<TryParseDelegate<QueryStringSerializerSettings, T>>(tryParseMethod)
				: InvalidTryParse;
		}

		private static bool InvalidTryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out T result)
		{
			result = default;
			return false;
		}

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out T result) =>
			TryParseMethod(value, settings, out result);
	}
}