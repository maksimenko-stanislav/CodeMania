using System;
using System.Collections.Generic;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.Specialized
{
	public sealed class ReadOnlyMemoryOfCharEqualityComparer : EqualityComparer<ReadOnlyMemory<char>>
	{
		public static EqualityComparer<ReadOnlyMemory<char>> Ordinal { get; } = new ReadOnlyMemoryOfCharEqualityComparer(StringComparison.Ordinal);
		public static EqualityComparer<ReadOnlyMemory<char>> OrdinalIgnoreCase { get; } = new ReadOnlyMemoryOfCharEqualityComparer(StringComparison.OrdinalIgnoreCase);

		private readonly StringComparison stringComparison;
		private readonly Func<char, int> getCharHashCode;

		public ReadOnlyMemoryOfCharEqualityComparer(StringComparison stringComparison)
		{
			this.stringComparison = stringComparison;
			switch (stringComparison)
			{
				case StringComparison.OrdinalIgnoreCase:
				case StringComparison.CurrentCultureIgnoreCase:
					getCharHashCode = ch => char.ToUpper(ch);
					break;
				case StringComparison.InvariantCultureIgnoreCase:
					getCharHashCode = ch => char.ToUpperInvariant(ch);
					break;
				default:
					getCharHashCode = ch => ch;
					break;
			}
		}

		public override bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y)
		{
			return x.Span.Equals(y.Span, stringComparison);
		}

		public override int GetHashCode(ReadOnlyMemory<char> obj)
		{
			unchecked
			{
				if (obj.IsEmpty) return 0;

				var span = obj.Span;

				var hashCode = HashHelper.HashSeed;

				int i = 0;
				for (; i < obj.Length - obj.Length % 4; i += 4)
				{
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, getCharHashCode(span[i + 0]));
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, getCharHashCode(span[i + 1]));
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, getCharHashCode(span[i + 2]));
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, getCharHashCode(span[i + 3]));
				}

				for (; i < obj.Length; i++)
				{
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, getCharHashCode(span[i]));
				}

				return hashCode;
			}
		}
	}
}