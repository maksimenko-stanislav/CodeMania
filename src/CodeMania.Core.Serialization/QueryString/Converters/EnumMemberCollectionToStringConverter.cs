using System;
using System.Collections.Generic;

namespace CodeMania.Core.Serialization.QueryString.Converters
{
	public sealed class EnumMemberCollectionToStringConverter<TEnum> : IConverter<IEnumerable<TEnum>, IEnumerable<string>>
		where TEnum : struct, Enum
	{
		public static EnumMemberCollectionToStringConverter<TEnum> Default { get; } = new EnumMemberCollectionToStringConverter<TEnum>();

		public IEnumerable<string> Convert(IEnumerable<TEnum> source)
		{
			if (source != null)
			{
				foreach (var item in source)
				{
					yield return EnumMemberToStringConverter<TEnum>.Default.Convert(item);
				}
			}
		}
	}
}