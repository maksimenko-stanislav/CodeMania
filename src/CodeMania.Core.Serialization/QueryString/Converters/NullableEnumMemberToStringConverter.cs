using System;

namespace CodeMania.Core.Serialization.QueryString.Converters
{
	public sealed class NullableEnumMemberToStringConverter<TEnum> : IConverter<TEnum?, string>
		where TEnum : struct, Enum
	{
		private static readonly EnumMemberToStringConverter<TEnum> Converter = EnumMemberToStringConverter<TEnum>.Default;
		public static NullableEnumMemberToStringConverter<TEnum> Default { get; } = new NullableEnumMemberToStringConverter<TEnum>();

		public string Convert(TEnum? source) => source.HasValue ? Converter.Convert(source.Value) : null;
	}
}