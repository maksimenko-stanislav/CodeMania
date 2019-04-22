using System;
using System.Collections.Generic;

namespace CodeMania.Core.Serialization.Converters
{
	public sealed class NullableEnumCollectionToStringsConverter<TEnum> : IConverter<IEnumerable<TEnum?>, IEnumerable<string>>
		where TEnum : struct, Enum
	{
		private static readonly EnumToStringConverter<TEnum> Converter = EnumToStringConverter<TEnum>.Default;

		public static NullableEnumCollectionToStringsConverter<TEnum> Default { get; } = new NullableEnumCollectionToStringsConverter<TEnum>();

		public IEnumerable<string> Convert(IEnumerable<TEnum?> source)
		{
			if (source != null)
			{
				foreach (var value in source)
				{
					if (value.HasValue)
						yield return Converter.Convert(value.Value);
				}
			}
		}
	}
}