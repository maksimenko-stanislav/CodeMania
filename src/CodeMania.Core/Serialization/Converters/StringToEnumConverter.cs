using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeMania.Core.EqualityComparers;

namespace CodeMania.Core.Serialization.Converters
{
	public sealed class StringToEnumConverter<TEnum> : IConverter<ReadOnlyMemory<char>, TEnum>
		where TEnum : struct, Enum
	{
		internal static readonly Dictionary<ReadOnlyMemory<char>, TEnum> NameMap;

		public static StringToEnumConverter<TEnum> Default { get; } = new StringToEnumConverter<TEnum>();

		static StringToEnumConverter()
		{
			NameMap = typeof(TEnum).GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static)
				.ToDictionary(
					x => x.Name.AsMemory(),
					x => (TEnum) x.GetValue(null),
					ReadOnlyMemoryOfCharEqualityComparer.OrdinalIgnoreCase);
		}

		public TEnum Convert(ReadOnlyMemory<char> source) =>
			!source.IsEmpty && NameMap.TryGetValue(source, out var result)
				? result
				: default;
	}
}