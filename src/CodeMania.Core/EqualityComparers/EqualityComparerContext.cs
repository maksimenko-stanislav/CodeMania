using System.Collections.Generic;
using System.Threading;

namespace CodeMania.Core.EqualityComparers
{
	internal sealed class EqualityComparerContext
	{
		private struct Element
		{
			public readonly object Data;

			public Element(object data)
			{
				Data = data;
			}
		}
		
		private sealed class DataEqualityComparer : EqualityComparer<Element>
		{
			public static readonly DataEqualityComparer Instance = new DataEqualityComparer();
			
			public override bool Equals(Element x, Element y) => ReferenceEquals(x.Data, y.Data);

			public override int GetHashCode(Element obj) => obj.Data?.GetHashCode() ?? 0;
		}
		
		private static readonly ThreadLocal<EqualityComparerContext> CurrentLocal = new ThreadLocal<EqualityComparerContext>(() => new EqualityComparerContext());
		private readonly HashSet<Element> visitedList;

		public static EqualityComparerContext Current => CurrentLocal.Value;

		public bool IsAcquired { get; set; }

		private EqualityComparerContext()
		{
			visitedList = new HashSet<Element>(DataEqualityComparer.Instance);
		}

		public void Free() => visitedList.Clear();

		public bool TryAdd(object obj) => obj == null || visitedList.Add(new Element(obj));
	}
}