using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using CodeMania.Core.EqualityComparers.Specialized;
using CodeMania.Core.Extensions;

namespace CodeMania.Core
{
	public sealed class EnumMap<TEnum>
		where TEnum : struct, Enum
	{
		private static readonly Lazy<EnumMap<TEnum>> LazyGetInstance = new Lazy<EnumMap<TEnum>>(() => new EnumMap<TEnum>(), true);
		public static EnumMap<TEnum> Instance => LazyGetInstance.Value;

		private readonly bool isFlagEnum;
		private readonly Dictionary<ReadOnlyMemory<char>, TEnum> nameToEnumMap;
		private readonly Dictionary<ReadOnlyMemory<char>, TEnum> ignoreCaseNameToEnumMap;

		private readonly Dictionary<TEnum, string> enumToNameMap;

		private readonly ThreadLocal<StringBuilder> flagsStringBuilder = new ThreadLocal<StringBuilder>(() => new StringBuilder());

		private EnumMap()
		{
			isFlagEnum = typeof(TEnum).IsDefined(typeof(FlagsAttribute), true);
			var names = Enum.GetNames(typeof(TEnum));

			nameToEnumMap = new Dictionary<ReadOnlyMemory<char>, TEnum>(names.Length, ReadOnlyMemoryOfCharEqualityComparer.Ordinal);
			ignoreCaseNameToEnumMap = new Dictionary<ReadOnlyMemory<char>, TEnum>(names.Length, ReadOnlyMemoryOfCharEqualityComparer.OrdinalIgnoreCase);
			enumToNameMap = new Dictionary<TEnum, string>(names.Length, EqualityComparer<TEnum>.Default);

			foreach (var name in names)
			{
				TEnum value = (TEnum) Enum.Parse(typeof(TEnum), name);

				var nameAsMemory = name.AsMemory();
				nameToEnumMap[nameAsMemory] = ignoreCaseNameToEnumMap[nameAsMemory] = value;
				enumToNameMap[value] = name;
			}
		}

		public bool HasFlag(TEnum enumValue, TEnum flagValue)
		{
			ulong value = UnsafeDynamicCast<TEnum, ulong>.Cast(enumValue);
			ulong flag = UnsafeDynamicCast<TEnum, ulong>.Cast(flagValue);

			return (value & flag) == flag;
		}

		public string ToString(TEnum enumValue)
		{
			if (isFlagEnum)
				return ToFlagsString(enumValue);

			if (enumToNameMap.TryGetValue(enumValue, out var result))
				return result;

			return null; // TODO: Think about returning enumValue.ToString()
		}

		private string ToFlagsString(TEnum enumValue)
		{
			ulong value = UnsafeDynamicCast<TEnum, ulong>.Cast(enumValue);

			var builder = flagsStringBuilder.Value.Clear();

			string result;
			if (enumToNameMap.TryGetValue(enumValue, out result))
			{
				return result;
			}
			else
			{
				ulong mask = 0x1;

				for (int i = 0; i < 64; i++)
				{
					if ((value & mask) == mask && enumToNameMap.TryGetValue(UnsafeDynamicCast<ulong, TEnum>.Cast(mask), out result))
					{
						builder.Append(result).Append(", ");
					}

					mask <<= 1;
				}
			}

			return builder.ToString(0, builder.Length - 2);
		}

		public string GetName<TEnumUnderlyingType>(TEnumUnderlyingType enumValue)
			where TEnumUnderlyingType : unmanaged =>
				ToString(UnsafeDynamicCast<TEnumUnderlyingType, TEnum>.Cast(enumValue));

		public TEnumUnderlyingType GetValue<TEnumUnderlyingType>(TEnum value)
			where TEnumUnderlyingType : unmanaged =>
				UnsafeDynamicCast<TEnum, TEnumUnderlyingType>.Cast(value);

		public TEnumUnderlyingType? GetValue<TEnumUnderlyingType>(TEnum? value)
			where TEnumUnderlyingType : unmanaged =>
				UnsafeDynamicCast<TEnum?, TEnumUnderlyingType?>.Cast(value);

		public TEnum Parse(string value, bool ignoreCase = false)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

			var dictionary = ignoreCase ? ignoreCaseNameToEnumMap : nameToEnumMap;

			if (isFlagEnum)
				return ParseFlags(value.AsMemory(), dictionary);

			ParseInternal(value.AsMemory(), dictionary, out TEnum result);

			return result;
		}

		private TEnum ParseFlags(ReadOnlyMemory<char> value, Dictionary<ReadOnlyMemory<char>, TEnum> dictionary)
		{
			ulong enumValue = 0;

			int pos = 0;
			while (pos < value.Length)
			{
				int start = pos;

				while (pos < value.Length && value.Span[pos] != ',')
				{
					pos++;
				}

				ParseInternal(value.Slice(start, pos - start), dictionary, out TEnum result);

				enumValue |= UnsafeDynamicCast<TEnum, ulong>.Cast(result);

				pos++;
			}

			return UnsafeDynamicCast<ulong, TEnum>.Cast(enumValue);
		}

		public bool TryParse(string value, out TEnum result, bool ignoreCase = false)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

			var dictionary = ignoreCase ? ignoreCaseNameToEnumMap : nameToEnumMap;

			if (isFlagEnum)
				return TryParseFlags(value.AsMemory(), dictionary, out result);

			return TryParseInternal(value.AsMemory(), dictionary, out result);
		}

		private bool TryParseFlags(ReadOnlyMemory<char> value, Dictionary<ReadOnlyMemory<char>, TEnum> dictionary, out TEnum result)
		{
			ulong enumValue = 0;

			int pos = 0;

			var span = value.Span;

			while (pos < value.Length)
			{
				int start = pos;

				while (pos < value.Length && span[pos] != ',')
				{
					pos++;
				}

				if (!TryParseInternal(value.Slice(start, pos - start), dictionary, out TEnum temp))
				{
					result = default;
					return false;
				}

				enumValue |= UnsafeDynamicCast<TEnum, ulong>.Cast(temp);

				pos++;
			}

			result = UnsafeDynamicCast<ulong, TEnum>.Cast(enumValue);

			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ParseInternal(ReadOnlyMemory<char> str, Dictionary<ReadOnlyMemory<char>, TEnum> dictionary, out TEnum result)
		{
			if (!dictionary.TryGetValue(str.Trim(), out result))
			{
				throw new ArgumentException("Provided value is a name, but not one of the named constants defined for the enumeration.", nameof(str));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryParseInternal(ReadOnlyMemory<char> str, Dictionary<ReadOnlyMemory<char>, TEnum> dictionary, out TEnum result) =>
			dictionary.TryGetValue(str.Trim(), out result);
	}
}