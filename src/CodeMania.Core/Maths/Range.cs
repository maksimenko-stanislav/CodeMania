using System;

// ReSharper disable MultipleSpaces

namespace CodeMania.Core.Maths
{
	public struct Range<T> : IEquatable<Range<T>>
	{
		private readonly Numeric<T> _fromInclusive;
		private readonly Numeric<T> _toInclusive;

		public Range(T fromInclusive, T toInclusive)
		{
			if (!Operations<T>.IsOperatorSupported(Operation.GreaterThan)        ||
			    !Operations<T>.IsOperatorSupported(Operation.GreaterThanOrEqual) ||
			    !Operations<T>.IsOperatorSupported(Operation.LessThan)           ||
			    !Operations<T>.IsOperatorSupported(Operation.LessThanOrEqual))
			{
				throw new InvalidOperationException($"Type '{typeof(T).FullName}' doesn't support required operators: <, <=, >, >=.");
			}
			if (Operations<T>.LessThan(toInclusive, fromInclusive))
			{
				throw new ArgumentOutOfRangeException(nameof(toInclusive), toInclusive,
					$"The value of {nameof(toInclusive)} should be less than value of {nameof(fromInclusive)}");
			}

			_fromInclusive = fromInclusive;
			_toInclusive = toInclusive;
		}

		public T FromInclusive => _fromInclusive;
		public T ToInclusive => _toInclusive;

		public override bool Equals(object other) => other is Range<T> range && Equals(range);

		public override int GetHashCode() => unchecked((_fromInclusive.GetHashCode() * 397) ^ _toInclusive.GetHashCode());

		public bool Equals(Range<T> other) => _fromInclusive == other._fromInclusive && _toInclusive == other._toInclusive;

		public override string ToString() => $"Range<{typeof(T).FullName}>: [{_fromInclusive}, {_toInclusive}]";

		public bool Contains(T value) => value >= _fromInclusive && value <= _toInclusive;

		public bool Include(Range<T> other) => _fromInclusive <= other._fromInclusive && _toInclusive >= other._toInclusive;

		public bool IsIncludedIn(Range<T> other) => _fromInclusive >= other._fromInclusive && _toInclusive <= other._toInclusive;

		public bool Intersects(Range<T> other) =>
			Include(other)
			|| (_fromInclusive > other._fromInclusive && _toInclusive > other._toInclusive)
			|| (_fromInclusive < other._fromInclusive && _toInclusive < other._toInclusive);

		public static bool operator ==(Range<T> x, Range<T> y) => x.Equals(y);

		public static bool operator !=(Range<T> x, Range<T> y) => !x.Equals(y);
	}
}