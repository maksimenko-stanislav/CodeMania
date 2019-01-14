using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CodeMania.Core.Messaging
{
	public readonly struct Payload : IEquatable<Payload>, IReadOnlyList<byte>
	{
		#region Fields

		public readonly byte[] Buffer;
		public readonly int Offset;
		public readonly int Count;

		#endregion

		#region ctor

		public Payload([NotNull] byte[] buffer, int offset, int count)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (offset < 0)
				throw new ArgumentOutOfRangeException(nameof(offset), "Invalid offset.");
			if (count < 0)
				throw new ArgumentOutOfRangeException(nameof(count), "Invalid count.");
			if (buffer.Length - offset < count)
				throw new ArgumentException("Invalid offset or count.");

			Offset = offset;
			Count = count;
			Buffer = buffer;
		}

		#endregion

		#region Methods

		public bool Equals(Payload other) => Offset == other.Offset && Count == other.Count && Buffer.Equals(other.Buffer);

		public override bool Equals(object obj) => obj is Payload other && Equals(other);

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Offset;
				hashCode = (hashCode * 397) ^ Count;
				hashCode = (hashCode * 397) ^ Buffer.GetHashCode();
				return hashCode;
			}
		}

		#endregion

		#region IReadOnlyList<byte> Support

		public PayloadEnumerator GetEnumerator() => new PayloadEnumerator(this);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		IEnumerator<byte> IEnumerable<byte>.GetEnumerator() => GetEnumerator();

		int IReadOnlyCollection<byte>.Count => Count;

		public byte this[int index]
		{
			get
			{
				if (index < 0 || index > Count - 1)
				{
					throw new ArgumentOutOfRangeException(nameof(index), index, "Index should be greater or equals to zero and less than count.");
				}

				return Buffer[Offset + index];
			}
		}

		#endregion

		#region Operators

		public static bool operator ==(Payload left, Payload right) => left.Equals(right);

		public static bool operator !=(Payload left, Payload right) => !left.Equals(right);

		public static implicit operator ArraySegment<byte>(Payload payload) => new ArraySegment<byte>(payload.Buffer, payload.Offset, payload.Count);

		public static implicit operator Payload(ArraySegment<byte> segment) => new Payload(segment.Array ?? Array.Empty<byte>(), segment.Offset, segment.Count);

		public static implicit operator Payload([NotNull] byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes));
			}

			return new Payload(bytes, 0, bytes.Length);
		}

		#endregion

		#region Nested Types

		public struct PayloadEnumerator : IEnumerator<byte>
		{
			private readonly Payload payload;
			private int position;

			internal PayloadEnumerator(Payload payload)
			{
				this.payload = payload;
				position = payload.Offset;
			}

			public bool MoveNext() => position < payload.Offset + payload.Count;

			public void Reset() => position = payload.Offset;

			public byte Current => payload.Buffer[position++];

			object IEnumerator.Current => Current;

			public void Dispose()
			{
			}
		}

		#endregion
	}
}