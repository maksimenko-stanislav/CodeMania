namespace CodeMania.Core.EqualityComparers.Common.ValueType
{
	public sealed class NullableValueTypeArrayEqualityComparer<T> : ArrayEqualityComparerBase<T?>
		where T : struct
	{
		public static NullableValueTypeArrayEqualityComparer<T> Default = new NullableValueTypeArrayEqualityComparer<T>();

		protected override int CalcCombinedHashCode(T? element, int hashCode) => EqualityComparisonHelper.CalcCombinedHashCode(element, hashCode, ElementEqualityComparer);

		protected override bool AreEquals(T? x, T? y) => EqualityComparisonHelper.AreEquals(x, y, ElementEqualityComparer);
	}
}