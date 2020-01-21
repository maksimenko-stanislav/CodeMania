using System;
using System.Collections.Generic;

namespace CodeMania.Core.Serialization.QueryString.Converters
{
	public sealed class NullableEnumMemberCollectionToStringConverter<TEnum> : IConverter<IEnumerable<TEnum?>, IEnumerable<string>>
		where TEnum : struct, Enum
	{
		public static NullableEnumMemberCollectionToStringConverter<TEnum> Default { get; } = new NullableEnumMemberCollectionToStringConverter<TEnum>();

		public IEnumerable<string> Convert(IEnumerable<TEnum?> source)
		{
			if (source != null)
			{
				foreach (var item in source)
				{
					yield return item.HasValue ? EnumMemberToStringConverter<TEnum>.Default.Convert(item.Value) : null;
				}
			}
		}
	}
}