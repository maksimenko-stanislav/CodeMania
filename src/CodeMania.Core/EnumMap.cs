using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace CodeMania.Core
{
	public sealed class EnumMap<TEnum>
		where TEnum : struct
	{
		private static readonly Lazy<EnumMap<TEnum>> LazyGetInstance = new Lazy<EnumMap<TEnum>>(() => new EnumMap<TEnum>(), true);
		public static EnumMap<TEnum> Instance => LazyGetInstance.Value;

		private readonly bool isFlagEnum;
		private readonly Dictionary<string, TEnum> nameToEnumMap;
		private readonly Dictionary<string, TEnum> ignoreCaseNameToEnumMap;

		private readonly Dictionary<TEnum, string> enumToNameMap;

		private readonly ThreadLocal<StringBuilder> flagsStringBuilder = new ThreadLocal<StringBuilder>(() => new StringBuilder());

		private EnumMap()
		{
			if (!typeof(TEnum).IsEnum || typeof(TEnum) == typeof(Enum))
			{
				throw new InvalidOperationException($"Generic type parameter should refer to an enumeration. Provided type: {typeof(TEnum).FullName}");
			}

			isFlagEnum = typeof(TEnum).IsDefined(typeof(FlagsAttribute), true);

			nameToEnumMap =
				Enum.GetNames(typeof(TEnum)).ToDictionary(
					x => x,
					x => (TEnum) Enum.Parse(typeof(TEnum), x),
					StringComparer.Ordinal);

			ignoreCaseNameToEnumMap = new Dictionary<string, TEnum>(
				nameToEnumMap,
				StringComparer.OrdinalIgnoreCase);

			enumToNameMap =
				Enum.GetNames(typeof(TEnum)).ToDictionary(
					x => (TEnum) Enum.Parse(typeof(TEnum), x),
					x => x,
					EqualityComparer<TEnum>.Default);
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

			return null;
		}

		private string ToFlagsString(TEnum enumValue)
		{
			ulong value = UnsafeDynamicCast<TEnum, ulong>.Cast(enumValue);

			var builder = flagsStringBuilder.Value.Clear();

			if (enumToNameMap.TryGetValue(enumValue, out var result))
			{
				builder.Append(result).Append(", ");
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
			where TEnumUnderlyingType : struct =>
				ToString(UnsafeDynamicCast<TEnumUnderlyingType, TEnum>.Cast(enumValue));

		public TEnumUnderlyingType GetValue<TEnumUnderlyingType>(TEnum value)
			where TEnumUnderlyingType : struct =>
			UnsafeDynamicCast<TEnum, TEnumUnderlyingType>.Cast(value);

		public TEnumUnderlyingType? GetValue<TEnumUnderlyingType>(TEnum? value)
			where TEnumUnderlyingType : struct =>
			UnsafeDynamicCast<TEnum?, TEnumUnderlyingType?>.Cast(value);

		public TEnum Parse(string value, bool ignoreCase = false)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

			var dictionary = ignoreCase ? ignoreCaseNameToEnumMap : nameToEnumMap;

			if (isFlagEnum)
				return ParseFlags(value.Trim(), dictionary);

			ParseInternal(value, dictionary, out TEnum result);

			return result;
		}

		private TEnum ParseFlags(string value, Dictionary<string, TEnum> dictionary)
		{
			ulong enumValue = 0;

			int pos = 0;
			while (pos < value.Length)
			{
				int start = pos;

				while (pos < value.Length && value[pos] != ',')
				{
					pos++;
				}

				ParseInternal(value.Substring(start, pos - start), dictionary, out TEnum result);

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
				return TryParseFlags(value.Trim(), dictionary, out result);

			return TryParseInternal(value, dictionary, out result);
		}

		private bool TryParseFlags(string value, Dictionary<string, TEnum> dictionary, out TEnum result)
		{
			ulong enumValue = 0;

			int pos = 0;
			while (pos < value.Length)
			{
				int start = pos;

				while (pos < value.Length && value[pos] != ',')
				{
					pos++;
				}

				if (!TryParseInternal(value.Substring(start, pos - start), dictionary, out TEnum temp))
				{
					result = default(TEnum);
					return false;
				}

				enumValue |= UnsafeDynamicCast<TEnum, ulong>.Cast(temp);

				pos++;
			}

			result = UnsafeDynamicCast<ulong, TEnum>.Cast(enumValue);

			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ParseInternal(string str, Dictionary<string, TEnum> dictionary, out TEnum result)
		{
			if (!dictionary.TryGetValue(str.Trim(), out result))
			{
				throw new ArgumentException("Provided value is a name, but not one of the named constants defined for the enumeration.", nameof(str));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryParseInternal(string str, Dictionary<string, TEnum> dictionary, out TEnum result) =>
			dictionary.TryGetValue(str.Trim(), out result);
	}
}