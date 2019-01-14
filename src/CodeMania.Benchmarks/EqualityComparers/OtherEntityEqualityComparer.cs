using System.Collections.Generic;
using Common.TestData.TestDataTypes;

namespace CodeMania.Benchmarks.EqualityComparers
{
	public sealed class OtherEntityEqualityComparer : IEqualityComparer<OtherEntity>
	{
		public static readonly OtherEntityEqualityComparer Instance = new OtherEntityEqualityComparer();

		public bool Equals(OtherEntity x, OtherEntity y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (ReferenceEquals(x, null)) return false;
			if (ReferenceEquals(y, null)) return false;
			if (x.GetType() != y.GetType()) return false;
			return x.Id == y.Id && string.Equals(x.Name, y.Name);
		}

		public int GetHashCode(OtherEntity obj)
		{
			unchecked
			{
				return (obj.Id * 397) ^ (obj.Name != null ? obj.Name.GetHashCode() : 0);
			}
		}
	}
}