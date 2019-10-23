using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace CodeMania.Core.EqualityComparers
{
	public sealed class EqualityComparerContext
	{
		private const int DefaultCapacity = 1024;

		private static readonly ThreadLocal<EqualityComparerContext> CurrentLocal = new ThreadLocal<EqualityComparerContext>(() => new EqualityComparerContext());
		private readonly HashSet<Element> visitedList;

		public static EqualityComparerContext Current => CurrentLocal.Value;

		public bool IsAcquired { get; set; }

		private EqualityComparerContext()
		{
			visitedList = new HashSet<Element>(ElementEqualityComparer.Instance).SetCapacity(DefaultCapacity);
		}

		public void Free()
		{
			var currentCount = visitedList.Count;
			visitedList.Clear();
			if (currentCount > DefaultCapacity)
			{
				visitedList.SetCapacity(DefaultCapacity);
			}
		}

		public bool TryAdd(object obj) => obj == null || visitedList.Add(new Element(obj));

		#region Nested Types

		private struct Element
		{
			public readonly object Data;

			public Element(object data)
			{
				Data = data;
			}
		}

		private sealed class ElementEqualityComparer : EqualityComparer<Element>
		{
			public static readonly ElementEqualityComparer Instance = new ElementEqualityComparer();

			public override bool Equals(Element x, Element y) => ReferenceEquals(x.Data, y.Data);

			public override int GetHashCode(Element obj) => obj.Data?.GetHashCode() ?? 0;
		}

		#endregion
	}

	// NOTE: https://stackoverflow.com/questions/6771917/why-cant-i-preallocate-a-hashsett
	internal static class HashSetExtensions
	{
		private static class HashSetDelegateHolder<T>
		{
			public static readonly Action<HashSet<T>, int> SetCapacityDelegate;

			static HashSetDelegateHolder()
			{
				MethodInfo initializeMethod = typeof(HashSet<T>).GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic);

				if (initializeMethod != null)
				{
					try
					{
						var hs = Expression.Parameter(typeof(HashSet<T>), "hs");
						var capacity = Expression.Parameter(typeof(int), "capacity");

						SetCapacityDelegate = Expression.Lambda<Action<HashSet<T>, int>>(
							Expression.Call(hs, initializeMethod, capacity),
							hs, capacity).Compile();

						return;
					}
					// ReSharper disable once EmptyGeneralCatchClause
					catch (Exception)
					{
					}
				}

				SetCapacityDelegate = (hs, capacity) => { };
			}
		}

		public static HashSet<T> SetCapacity<T>(this HashSet<T> hs, int capacity)
		{
#if NETCOREAPP3_0
			hs.EnsureCapacity(capacity);
#else
			HashSetDelegateHolder<T>.SetCapacityDelegate(hs, capacity);
#endif
			return hs;
		}
	}
}