using System;
using System.Collections.Generic;

namespace CodeMania.Core.Serialization.QueryString.Converters
{
	public sealed class EnumCollectionToStringsConverter<TEnum> : IConverter<IEnumerable<TEnum>, IEnumerable<string>>
		where TEnum : struct, Enum
	{
		private static readonly EnumToStringConverter<TEnum> Converter = EnumToStringConverter<TEnum>.Default;

		public static EnumCollectionToStringsConverter<TEnum> Default { get; } = new EnumCollectionToStringsConverter<TEnum>();

		public IEnumerable<string> Convert(IEnumerable<TEnum> source)
		{
			if (source != null)
			{
				foreach (var value in source)
				{
					yield return Converter.Convert(value);
				}
			}
		}
	}
}