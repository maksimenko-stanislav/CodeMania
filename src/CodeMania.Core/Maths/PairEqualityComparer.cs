using System.Collections.Generic;

namespace CodeMania.Core.Maths
{
	internal sealed class PairEqualityComparer<T> : IEqualityComparer<Pair<T>>
	{
		private static readonly EqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;

		public bool Equals(Pair<T> x, Pair<T> y) =>
			EqualityComparer.Equals(x.First, y.First) &&
			EqualityComparer.Equals(x.Second, y.Second);

		public int GetHashCode(Pair<T> obj) =>
			unchecked((EqualityComparer.GetHashCode(obj.First) * 397) ^ EqualityComparer.GetHashCode(obj.Second));
	}
}