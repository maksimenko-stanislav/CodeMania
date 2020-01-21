namespace CodeMania.Core.EqualityComparers.Common.ValueType
{
	public sealed class NullableValueTypeListEqualityComparer<T> : ListEqualityComparerBase<T?>
		where T : struct
	{
		public static NullableValueTypeListEqualityComparer<T> Default = new NullableValueTypeListEqualityComparer<T>();

		protected override int CalcCombinedHashCode(T? element, int hashCode) => EqualityComparisonHelper.CalcCombinedHashCode(element, hashCode, ElementEqualityComparer);

		protected override bool AreEquals(T? x, T? y) => EqualityComparisonHelper.AreEquals(x, y, ElementEqualityComparer);
	}
}