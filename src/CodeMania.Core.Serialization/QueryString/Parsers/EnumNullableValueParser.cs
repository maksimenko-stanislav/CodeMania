using System;
using CodeMania.Core.Serialization.QueryString.Converters;
using JetBrains.Annotations;

namespace CodeMania.Core.Serialization.QueryString.Parsers
{
	[UsedImplicitly]
	public static class EnumNullableValueParser<TEnum>
		where TEnum : struct, Enum
	{
		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out TEnum? result)
		{
			if (!value.Span.IsWhiteSpace())
			{
				result = StringToNullableEnumConverter<TEnum>.Default.Convert(value);

				return result.HasValue;
			}

			result = default;
			return true;
		}
	}
}