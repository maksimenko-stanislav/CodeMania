using System;
using System.Collections.Generic;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers
{
	public static class ReadOnlyMemoryOfCharEqualityComparer
	{
		public static EqualityComparer<ReadOnlyMemory<char>> Ordinal { get; } = new ReadOnlyMemoryOfCharOrdinalEqualityComparer();
		public static EqualityComparer<ReadOnlyMemory<char>> OrdinalIgnoreCase { get; } = new ReadOnlyMemoryOfCharOrdinalIgnoreCaseEqualityComparer();

		public sealed class ReadOnlyMemoryOfCharOrdinalEqualityComparer : EqualityComparer<ReadOnlyMemory<char>>
		{
			public override bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y)
			{
				return x.Span.Equals(y.Span, StringComparison.Ordinal);
			}

			public override int GetHashCode(ReadOnlyMemory<char> obj)
			{
				unchecked
				{
					if (obj.IsEmpty) return 0;

					var span = obj.Span;

					var hashCode = 5381;

					for (int i = 0; i < obj.Length; i++)
					{
						hashCode = HashHelper.CombineHashCodes(hashCode * 397, span[i]);
					}

					return hashCode;
				}
			}
		}

		public sealed class ReadOnlyMemoryOfCharOrdinalIgnoreCaseEqualityComparer : EqualityComparer<ReadOnlyMemory<char>>
		{
			public override bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y)
			{
				return x.Span.Equals(y.Span, StringComparison.OrdinalIgnoreCase);
			}

			public override int GetHashCode(ReadOnlyMemory<char> obj)
			{
				unchecked
				{
					if (obj.IsEmpty) return 0;

					var span = obj.Span;

					var hashCode = 5381;

					for (int i = 0; i < obj.Length; i++)
					{
						hashCode = HashHelper.CombineHashCodes(hashCode * 397, char.ToUpperInvariant(span[i]));
					}

					return hashCode;
				}
			}
		}
	}
}