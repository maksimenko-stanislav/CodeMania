﻿using System;
using CodeMania.Core.Serialization.Converters;
using JetBrains.Annotations;

namespace CodeMania.Core.Serialization.Parsers
{
	[UsedImplicitly]
	public static class DataContractEnumNullableValueParser<TEnum>
		where TEnum : struct, Enum
	{
		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out TEnum? result)
		{
			if (!value.Span.IsWhiteSpace())
			{
				result = DataContractStringToNullableEnumConverter<TEnum>.Default.Convert(value);

				return true;
			}

			result = default;
			return false;
		}
	}
}