using System.Collections.Generic;
using System.Reflection;
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
			visitedList = new HashSet<Element>(DataEqualityComparer.Instance).SetCapacity(1024); // init some default capacity.
		}

		public void Free() => visitedList.Clear();

		public bool TryAdd(object obj) => obj == null || visitedList.Add(new Element(obj));
    }

    // TODO: use appropriate HashSet ctor with capacity argument when move to netstandard 2.1 and remove this crutch.
    // TODO: thanks to this thread: https://stackoverflow.com/questions/6771917/why-cant-i-preallocate-a-hashsett
    internal static class HashSetExtensions
    {
        private static class HashSetDelegateHolder<T>
        {
            private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
            public static MethodInfo InitializeMethod { get; } = typeof(HashSet<T>).GetMethod("Initialize", Flags);
        }

        public static HashSet<T> SetCapacity<T>(this HashSet<T> hs, int capacity)
        {
            HashSetDelegateHolder<T>.InitializeMethod.Invoke(hs, new object[] { capacity });
            return hs;
        }
    }
}