using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers
{
	public sealed class ArrayEqualityComparer<T> : EnumerableComparerBase<T, T[]>
	{
		[UsedImplicitly]
		public static ArrayEqualityComparer<T> Instance { get; } = new ArrayEqualityComparer<T>();

		public override bool Equals(T[] x, T[] y)
		{
			if (x == null) return y == null;
			if (ReferenceEquals(x, y)) return true;

			if (y != null)
			{
				if (x.Length != y.Length) return false;

				for (var i = 0; i < x.Length; i++)
				{
					if (!AreEqualsFunc(x[i], y[i])) return false;
				}

				return true;
			}

			return false;
		}
	}
}