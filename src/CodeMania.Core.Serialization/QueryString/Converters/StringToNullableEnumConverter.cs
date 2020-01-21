using System;
using System.Collections.Generic;

namespace CodeMania.Core.Serialization.QueryString.Converters
{
	public sealed class StringToNullableEnumConverter<TEnum> : IConverter<ReadOnlyMemory<char>, TEnum?>
		where TEnum : struct, Enum
	{
		private static readonly Dictionary<ReadOnlyMemory<char>, TEnum> NameMap = StringToEnumConverter<TEnum>.NameMap;

		public static StringToNullableEnumConverter<TEnum> Default { get; } = new StringToNullableEnumConverter<TEnum>();

		public TEnum? Convert(ReadOnlyMemory<char> source)
		{
			return !source.IsEmpty && NameMap.TryGetValue(source, out var result)
				? result
				: default(TEnum?);
		}
	}
}