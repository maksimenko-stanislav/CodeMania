using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CodeMania.Core.Extensions;
using CodeMania.Core.Internals;

namespace CodeMania.Core.Benchmarks.EqualityComparers
{
	public static class ReflectionBasedObjectStructureEqualityComparer
	{
		internal static readonly ConcurrentDictionary<Type, IEqualityComparer> Comparers =
			new ConcurrentDictionary<Type, IEqualityComparer>();
	}

	public class ReflectionBasedObjectStructureEqualityComparer<T> : EqualityComparer<T>
	{
		private readonly PropertyInfo[] _properties;

		public ReflectionBasedObjectStructureEqualityComparer()
		{
			_properties = typeof(T).GetProperties();
		}

		public override bool Equals(T x, T y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (ReferenceEquals(x, null)) return false;
			if (ReferenceEquals(y, null)) return false;
			if (x.GetType() != y.GetType()) return false;

			VisitedList visitedList = default(VisitedList);

			int i = 0;
			for (; i < _properties.Length; i++)
			{
				var property = _properties[i];

				object xValue = property.GetValue(x);
				object yValue = property.GetValue(y);

				if (ReferenceEquals(xValue, yValue) || Equals(xValue, yValue))
					continue; // OK, both property or field values are null, continuing comparision.

				if (ReferenceEquals(xValue, null)) return false;
				if (ReferenceEquals(yValue, null)) return false;
				if (xValue.GetType() != yValue.GetType()) return false;

				if (!xValue.GetType().IsPrimitive && !visitedList.HashSet.Add(xValue))
				{
					// its property value was already visited;
					continue;
				}

				if (!yValue.GetType().IsPrimitive && !visitedList.HashSet.Add(yValue))
				{
					// y and its property value was already visited;
					continue;
				}

				IEnumerable xCollection = xValue as IEnumerable;

				if (xCollection != null)
				{
					IEnumerable yCollection = (IEnumerable)yValue;

					if (!AreCollectionsEquals(xCollection, yCollection, ref visitedList))
					{
						Debug.Print("Values of property or field {0}.{1} are not equals.", typeof(T), property.Name);
						return false;
					}

					continue;
				}

				if (xValue.GetType().IsBuiltInPrimitive())
				{
					if (!Equals(xValue, yValue)) return false;

					continue;
				}

				if (!xValue.GetType().IsPrimitive && !GetOrCreateEqualityComparer(xValue.GetType()).Equals(xValue, yValue))
					return false;
			}

			Debug.Assert(i == _properties.Length, "i == _properties.Length");

			return true;
		}

		private bool AreCollectionsEquals(IEnumerable x, IEnumerable y, ref VisitedList visitedList)
		{
			var xEnumerator = x.GetEnumerator();
			var yEnumerator = y.GetEnumerator();

			try
			{
				while (true)
				{

					var canMoveX = xEnumerator.MoveNext();
					var canMoveY = yEnumerator.MoveNext();

					// collections have different count of elements, consider them as non equals.
					if (canMoveX != canMoveY) return false; 

					if (!canMoveX)
					{
						break;
					}

					var xValue = xEnumerator.Current;
					var yValue = yEnumerator.Current;

					// OK, both values are null or reference same object, continuing comparision.
					if (ReferenceEquals(xValue, yValue) || Equals(xValue, yValue)) continue;

					if (ReferenceEquals(xValue, null)) return false;
					if (ReferenceEquals(yValue, null)) return false;
					if (xValue.GetType() != yValue.GetType()) return false;

					// check if we've already used this references
					if (!visitedList.HashSet.Add(xValue)) continue;
					if (!visitedList.HashSet.Add(yValue)) continue;

					var xValueAsCollection = xValue as IEnumerable;

					if (xValueAsCollection != null)
					{
						var yValueAsCollection = (IEnumerable) yValue;

						// NOTE: recursion
						if (!AreCollectionsEquals(xValueAsCollection, yValueAsCollection, ref visitedList))
						{
							return false;
						}
					}

					if (xValue.GetType().IsBuiltInPrimitive())
					{
						if (!Equals(xValue, yValue)) return false;

						continue;
					}

					// now we know, that xValue and yValue are not collections, so consider them as "complex" type.
					if (!GetOrCreateEqualityComparer(xValue.GetType()).Equals(xValue, yValue))
					{
						return false;
					}
				}
			}
			finally
			{
				(xEnumerator as IDisposable)?.Dispose();
				(yEnumerator as IDisposable)?.Dispose();
			}

#if DEBUG
			// ReSharper disable PossibleMultipleEnumeration
			int xCount = x.OfType<object>().Count();
			int yCount = y.OfType<object>().Count();
			// ReSharper restore PossibleMultipleEnumeration
			Debug.Assert(xCount == yCount, "xCount == yCount");
#endif

			return true;
		}
		
		public override int GetHashCode(T obj)
		{
			if (obj == null) return 0;

			var hashCode = HashHelper.HashSeed;

			VisitedList visitedList = default(VisitedList);

			int i = 0;
			for (; i < _properties.Length; i++)
			{
				var property = _properties[i];

				object value = property.GetValue(obj);

				if (value == null)
				{
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, HashHelper.HashSeed);
					continue;
				}

				if (value.GetType().IsValueType || value is string)
				{
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, value.GetHashCode());
					continue;
				}

                // we have already visited this reference, continuing calculation to avoid StackOverFlowException.
                if (!visitedList.HashSet.Add(value)) continue;

                IEnumerable collection = value as IEnumerable;

				if (collection != null)
				{
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, CalculateHashCodeForCollection(collection, ref visitedList));
					continue;
				}

				// now we know that both objects are "complex", so we can get or create appropriate and use it for comparision ObjectStructureEqualityComparer<T>.
				var comparer = GetOrCreateEqualityComparer(value.GetType());

				hashCode = HashHelper.CombineHashCodes(hashCode * 397, comparer.GetHashCode(value));
			}

			Debug.Assert(i == _properties.Length, "i == _properties.Length");

			return hashCode;
		}

		private int CalculateHashCodeForCollection(IEnumerable collection, ref VisitedList visitedList)
		{
			var enumerator = collection.GetEnumerator();
			var hashCode = HashHelper.HashSeed;

			try
			{
#if DEBUG
				// ReSharper disable once PossibleMultipleEnumeration
				int count = collection.OfType<object>().Count();

				int counter = 0;
#endif
				while (enumerator.MoveNext())
				{
#if DEBUG
					counter++;
#endif

					var value = enumerator.Current;

					if (value == null)
					{
						hashCode = HashHelper.CombineHashCodes(hashCode * 397, HashHelper.HashSeed);

						continue;
					}

					var type = value.GetType();
					if (type == typeof(string) || type.IsValueType)
					{
						hashCode = HashHelper.CombineHashCodes(hashCode * 397, value.GetHashCode());
						continue;
					}

					// we have already visited this reference, continuing calculation to avoid StackOverFlowException.
					if (!visitedList.HashSet.Add(value)) continue;

					var valueAsCollection = value as IEnumerable;

					if (valueAsCollection != null)
					{
						// NOTE: recursion
						hashCode = HashHelper.CombineHashCodes(hashCode * 397, CalculateHashCodeForCollection(valueAsCollection, ref visitedList));

						continue;
					}
					
					// now we know, that value is not collections, so consider it as "complex" type.
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, GetOrCreateEqualityComparer(type).GetHashCode(value));
				}

#if DEBUG
				Debug.Assert(counter == count, "counter == count");
#endif
			}
			finally
			{
				(enumerator as IDisposable)?.Dispose();
			}

			return hashCode;
		}

		private IEqualityComparer GetOrCreateEqualityComparer(Type type)
		{
			return type == typeof(T)
				? this
				: ReflectionBasedObjectStructureEqualityComparer.Comparers.GetOrAdd(type,
					key => (IEqualityComparer) Activator.CreateInstance(
						typeof(ReflectionBasedObjectStructureEqualityComparer<>).MakeGenericType(key)));
		}

		private struct VisitedList
		{
			private HashSet<object> _objects;

			public HashSet<object> HashSet => _objects ?? (_objects = new HashSet<object>());
		}
	}
}