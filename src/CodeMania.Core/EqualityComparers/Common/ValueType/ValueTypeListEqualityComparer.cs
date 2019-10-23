namespace CodeMania.Core.EqualityComparers.Common.ValueType
{
	public sealed class ValueTypeListEqualityComparer<T> : ListEqualityComparerBase<T>
		where T : struct
	{
		public static ValueTypeListEqualityComparer<T> Default = new ValueTypeListEqualityComparer<T>();

		protected override int CalcCombinedHashCode(T element, int hashCode) => EqualityComparisonHelper.CalcCombinedHashCode(element, hashCode, ElementEqualityComparer);

		protected override bool AreEquals(T x, T y) => EqualityComparisonHelper.AreEquals(x, y, ElementEqualityComparer);
	}
}