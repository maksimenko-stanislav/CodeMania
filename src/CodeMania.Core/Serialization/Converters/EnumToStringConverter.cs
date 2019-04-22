using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeMania.Core.Serialization.Converters
{
	public sealed class EnumToStringConverter<TEnum> : IConverter<TEnum, string>
		where TEnum : struct, Enum
	{
		private static readonly Dictionary<TEnum, string> NameMap;

		public static EnumToStringConverter<TEnum> Default { get; } = new EnumToStringConverter<TEnum>();

		static EnumToStringConverter()
		{
			NameMap = typeof(TEnum).GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static)
				.ToDictionary(
					x => (TEnum) x.GetValue(null),
					x => x.Name,
					EqualityComparer<TEnum>.Default);
		}

		public string Convert(TEnum source) =>
			NameMap.TryGetValue(source, out var result)
				? result
				: source.ToString();
	}
}