namespace CodeMania.Core.EqualityComparers.Common.ValueType
{
	public sealed class ValueTypeCollectionEqualityComparer<T> : EnumerableEqualityComparerBase<T>
		where T : struct
	{
		public static ValueTypeCollectionEqualityComparer<T> Default = new ValueTypeCollectionEqualityComparer<T>();

		protected override int CalcCombinedHashCode(T element, int hashCode) => EqualityComparisonHelper.CalcCombinedHashCode(element, hashCode, ElementEqualityComparer);

		protected override bool AreEquals(T x, T y) => EqualityComparisonHelper.AreEquals(x, y, ElementEqualityComparer);
	}
}