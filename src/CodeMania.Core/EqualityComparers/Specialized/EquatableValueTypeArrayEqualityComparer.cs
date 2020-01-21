using System;
using CodeMania.Core.Internals;

namespace CodeMania.Core.EqualityComparers.Specialized
{
	public sealed class EquatableValueTypeArrayEqualityComparer<T> : EquatableTypeArrayEqualityComparerBase<T>
		where T : struct, IEquatable<T>
	{
		public static readonly EquatableValueTypeArrayEqualityComparer<T> Default = new EquatableValueTypeArrayEqualityComparer<T>();

		public override int GetHashCode(T[] obj)
		{
			if (obj == null) return 0;
			if (obj.Length == 0) return HashHelper.CombineHashCodes(HashSeed, HashHelper.HashSeed);

			int hashCode = HashHelper.HashSeed;

			int i = 0;

			for (; i < obj.Length - obj.Length % 4; i += 4)
			{
				CombineHashCodes(ref obj[i + 0], ref hashCode);
				CombineHashCodes(ref obj[i + 1], ref hashCode);
				CombineHashCodes(ref obj[i + 2], ref hashCode);
				CombineHashCodes(ref obj[i + 3], ref hashCode);
			}

			for (; i < obj.Length; i++)
			{
				CombineHashCodes(ref obj[i], ref hashCode);
			}

			return hashCode;

			void CombineHashCodes(ref T item, ref int hash)
			{
				hash = HashHelper.CombineHashCodes(hash * 397, item.GetHashCode());
			}
		}
	}
}