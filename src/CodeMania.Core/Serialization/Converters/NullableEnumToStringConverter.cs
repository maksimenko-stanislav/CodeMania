using System;

namespace CodeMania.Core.Serialization.Converters
{
	public sealed class NullableEnumToStringConverter<TEnum> : IConverter<TEnum?, string>
		where TEnum : struct, Enum
	{
		private static readonly EnumToStringConverter<TEnum> Converter = EnumToStringConverter<TEnum>.Default;

		public static NullableEnumToStringConverter<TEnum> Default { get; } = new NullableEnumToStringConverter<TEnum>();

		public string Convert(TEnum? source) => source.HasValue ? Converter.Convert(source.Value) : null;
	}
}