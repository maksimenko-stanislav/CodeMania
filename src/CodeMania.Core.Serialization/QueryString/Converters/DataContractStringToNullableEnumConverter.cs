using System;
using System.Collections.Generic;

namespace CodeMania.Core.Serialization.QueryString.Converters
{
	public sealed class DataContractStringToNullableEnumConverter<TEnum> : IConverter<ReadOnlyMemory<char>, TEnum?>
		where TEnum : struct, Enum
	{
		private static readonly Dictionary<ReadOnlyMemory<char>, TEnum> NameMap = DataContractStringToEnumConverter<TEnum>.NameMap;

		public static DataContractStringToNullableEnumConverter<TEnum> Default { get; } = new DataContractStringToNullableEnumConverter<TEnum>();

		public TEnum? Convert(ReadOnlyMemory<char> source) =>
			!source.Span.IsWhiteSpace() && NameMap.TryGetValue(source, out var result)
				? result
				: default(TEnum?);
	}
}