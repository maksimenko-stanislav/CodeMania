namespace CodeMania.Core.EqualityComparers
{
	public interface IReferenceTypeEqualityComparer<in T>
		where T : class
	{
		bool Equals(T x, T y, EqualityComparerContext context);
		int GetHashCode(T obj, EqualityComparerContext context);
	}
}