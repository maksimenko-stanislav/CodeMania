using System;

namespace CodeMania.Core.Extensions
{
	public static class ReadOnlyMemoryOfCharExtensions
	{
#if !NETCOREAPP3_0
		public static ReadOnlyMemory<char> Trim(this ReadOnlyMemory<char> source)
		{
			int start = 0;
			int end = source.Length - 1;

			var span = source.Span;

			while (start < source.Length && char.IsWhiteSpace(span[start]))
			{
				++start;
			}

			while (end >= start && char.IsWhiteSpace(span[end]))
			{
				--end;
			}

			return source.Slice(start, end - start + 1);
		}
#endif

		public static int IndexOf(this ReadOnlyMemory<char> source, char ch)
		{
			return source.Span.IndexOf(ch);
		}

		public static bool Contains(this ReadOnlyMemory<char> source, char ch)
		{
			return source.Span.IndexOf(ch) != -1;
		}
	}
}