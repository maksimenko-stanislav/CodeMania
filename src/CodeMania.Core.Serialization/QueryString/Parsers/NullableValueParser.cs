using System;
using JetBrains.Annotations;

namespace CodeMania.Core.Serialization.QueryString.Parsers
{
	[UsedImplicitly]
	public static class NullableValueParser<T>
		where T : struct
	{
		private static readonly TryParseDelegate<QueryStringSerializerSettings, T> TryParseFunc;

		static NullableValueParser()
		{
			TryParseFunc = ValueParser<T>.CreateParseDelegate();
		}

		[UsedImplicitly]
		public static bool TryParse(ReadOnlyMemory<char> value, QueryStringSerializerSettings settings, out T? result)
		{
			T local;
			if (TryParseFunc(value, settings, out local))
			{
				result = local;
				return true;
			}

			result = default;
			return false;
		}
	}
}