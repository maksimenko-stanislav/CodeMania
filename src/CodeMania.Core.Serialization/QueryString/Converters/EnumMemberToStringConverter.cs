using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace CodeMania.Core.Serialization.QueryString.Converters
{
	public sealed class EnumMemberToStringConverter<TEnum> : IConverter<TEnum, string>
		where TEnum : struct, Enum
	{
		private static readonly Dictionary<TEnum, string> NameMap;

		public static EnumMemberToStringConverter<TEnum> Default { get; } = new EnumMemberToStringConverter<TEnum>();

		static EnumMemberToStringConverter()
		{
			NameMap = typeof(TEnum).GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static)
				.ToDictionary(
					x => (TEnum) x.GetValue(null),
					x => x.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? x.Name,
					EqualityComparer<TEnum>.Default);
		}

		public string Convert(TEnum source) =>
			NameMap.TryGetValue(source, out var result)
				? result
				: source.ToString();
	}
}