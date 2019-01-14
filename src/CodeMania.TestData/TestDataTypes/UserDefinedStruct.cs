using System;

namespace Common.TestData.TestDataTypes
{
	[Serializable]
	public struct UserDefinedStruct : IEquatable<UserDefinedStruct>
	{
		public static bool operator ==(UserDefinedStruct left, UserDefinedStruct right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(UserDefinedStruct left, UserDefinedStruct right)
		{
			return !left.Equals(right);
		}

		public bool Equals(UserDefinedStruct other)
		{
			return Bool == other.Bool && Int32 == other.Int32 && Float.Equals(other.Float);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is UserDefinedStruct && Equals((UserDefinedStruct) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Bool.GetHashCode();
				hashCode = (hashCode * 397) ^ Int32;
				hashCode = (hashCode * 397) ^ Float.GetHashCode();
				return hashCode;
			}
		}

		public readonly bool Bool;
		public readonly int Int32;
		public readonly float Float;

		public UserDefinedStruct(bool b, int int32, float f)
		{
			Bool = b;
			Int32 = int32;
			Float = f;
		}
	}
}