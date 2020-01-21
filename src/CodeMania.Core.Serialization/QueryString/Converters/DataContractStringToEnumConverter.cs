using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using CodeMania.Core.EqualityComparers.Specialized;

namespace CodeMania.Core.Serialization.QueryString.Converters
{
	public sealed class DataContractStringToEnumConverter<TEnum> : IConverter<ReadOnlyMemory<char>, TEnum>
		where TEnum : struct, Enum
	{
		internal static readonly Dictionary<ReadOnlyMemory<char>, TEnum> NameMap;

		public static DataContractStringToEnumConverter<TEnum> Default { get; } = new DataContractStringToEnumConverter<TEnum>();

		static DataContractStringToEnumConverter()
		{
			NameMap = typeof(TEnum).GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static)
				.ToDictionary(
					x => (x.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? x.Name).AsMemory(),
					x => (TEnum) x.GetValue(null),
					ReadOnlyMemoryOfCharEqualityComparer.OrdinalIgnoreCase);
		}

		public TEnum Convert(ReadOnlyMemory<char> source) =>
			NameMap.TryGetValue(source, out var result)
				? result
				: StringToEnumConverter<TEnum>.Default.Convert(source);
	}
}