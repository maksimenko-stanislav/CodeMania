namespace CodeMania.Core.EqualityComparers.Common.ValueType
{
	public sealed class ValueTypeArrayEqualityComparer<T> : ArrayEqualityComparerBase<T>
		where T : struct
	{
		public static ValueTypeArrayEqualityComparer<T> Default = new ValueTypeArrayEqualityComparer<T>();

		protected override int CalcCombinedHashCode(T element, int hashCode) => EqualityComparisonHelper.CalcCombinedHashCode(element, hashCode, ElementEqualityComparer);

		protected override bool AreEquals(T x, T y) => EqualityComparisonHelper.AreEquals(x, y, ElementEqualityComparer);
	}
}