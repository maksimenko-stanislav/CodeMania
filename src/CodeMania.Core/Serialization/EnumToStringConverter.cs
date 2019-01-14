using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace CodeMania.Core.Serialization
{
	public sealed class EnumToStringConverter<TEnum> : IConverter<TEnum, string>
		where TEnum : struct
	{
		private static readonly Dictionary<TEnum, string> NameMap;
		private static readonly EnumToStringConverter<TEnum> DefaultInstance = new EnumToStringConverter<TEnum>();
		public static EnumToStringConverter<TEnum> Default => DefaultInstance;

		static EnumToStringConverter()
		{
			if (typeof(TEnum).IsEnum)
			{
				NameMap = typeof(TEnum).GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static)
					.ToDictionary(
						x => (TEnum) x.GetValue(null),
						x => x.Name,
						EqualityComparer<TEnum>.Default);
			}
		}

		public string Convert(TEnum source)
		{
			string result;
			return NameMap != null && NameMap.TryGetValue(source, out result) ? result : source.ToString();
		}
	}

	public sealed class NullableEnumToStringConverter<TEnum> : IConverter<TEnum?, string>
		where TEnum : struct
	{
		private readonly EnumToStringConverter<TEnum> Converter = EnumToStringConverter<TEnum>.Default;

		private static readonly NullableEnumToStringConverter<TEnum> DefaultInstance = new NullableEnumToStringConverter<TEnum>();
		public static NullableEnumToStringConverter<TEnum> Default => DefaultInstance;

		public string Convert(TEnum? source)
		{
			return source.HasValue ? Converter.Convert(source.Value) : null;
		}
	}

	public sealed class EnumCollectionToStringsConverter<TEnum> : IConverter<IEnumerable<TEnum>, IEnumerable<string>>
		where TEnum : struct
	{
		private readonly EnumToStringConverter<TEnum> Converter = EnumToStringConverter<TEnum>.Default;

		private static readonly EnumCollectionToStringsConverter<TEnum> DefaultInstance = new EnumCollectionToStringsConverter<TEnum>();
		public static EnumCollectionToStringsConverter<TEnum> Default => DefaultInstance;

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

	public sealed class NullableEnumCollectionToStringsConverter<TEnum> : IConverter<IEnumerable<TEnum?>, IEnumerable<string>>
		where TEnum : struct
	{
		private readonly EnumToStringConverter<TEnum> Converter = EnumToStringConverter<TEnum>.Default;

		private static readonly NullableEnumCollectionToStringsConverter<TEnum> DefaultInstance = new NullableEnumCollectionToStringsConverter<TEnum>();
		public static NullableEnumCollectionToStringsConverter<TEnum> Default => DefaultInstance;

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

	public sealed class EnumMemberToStringConverter<TEnum> : IConverter<TEnum, string>
	{
		private static readonly Dictionary<TEnum, string> NameMap;

		static EnumMemberToStringConverter()
		{
			if (typeof(TEnum).IsEnum)
			{
				NameMap = typeof(TEnum).GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static)
					.ToDictionary(
						x => (TEnum) x.GetValue(null),
						x => x.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? x.Name,
						EqualityComparer<TEnum>.Default);
			}
		}

		public string Convert(TEnum source)
		{
			string result;
			return NameMap != null && NameMap.TryGetValue(source, out result) ? result : source.ToString();
		}
	}
}