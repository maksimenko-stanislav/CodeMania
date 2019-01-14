using System;
using System.Collections.Generic;

namespace CodeMania.Core.Maths
{
	internal struct Pair<T> : IEquatable<Pair<T>> // consider using ValueTuple instead
	{
		public readonly T First;
		public readonly T Second;

		public Pair(T first, T second)
		{
			First = first;
			Second = second;
		}

		public bool Equals(Pair<T> other) =>
			EqualityComparer<T>.Default.Equals(First, other.First) && EqualityComparer<T>.Default.Equals(Second, other.Second);

		public override bool Equals(object obj)
		{
			return obj is Pair<T> pair && Equals(pair);
		}

		public override int GetHashCode() => unchecked((EqualityComparer<T>.Default.GetHashCode(First) * 397) ^ EqualityComparer<T>.Default.GetHashCode(Second));

		public static implicit operator (T First, T Second)(Pair<T> pair) => (pair.First, pair.Second);

		public static implicit operator Pair<T>((T First, T Second) tuple) => new Pair<T>(tuple.First, tuple.Second);

		public static implicit operator KeyValuePair<T, T>(Pair<T> pair) => new KeyValuePair<T, T>(pair.First, pair.Second);

		public static implicit operator Pair<T>(KeyValuePair<T, T> tuple) => new Pair<T>(tuple.Key, tuple.Value);

		public static bool operator ==(Pair<T> left, Pair<T> right) => left.Equals(right);

		public static bool operator !=(Pair<T> left, Pair<T> right) => !left.Equals(right);

		public override string ToString() => $"[{First}, {Second}]";
	}
}