using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CodeMania.Core.Internals;
using Common.TestData.TestDataTypes;

namespace CodeMania.Core.Benchmarks.EqualityComparers
{
	internal static class Extensions
	{
		public static int GetDictionaryHashCode<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
		{
			if (dictionary == null) return 0;

			unchecked
			{
				int hashCode = HashHelper.HashSeed;

				foreach (var pair in dictionary)
				{
					hashCode = HashHelper.CombineHashCodes(
						hashCode * 397,
						HashHelper.CombineHashCodes(
							EqualityComparer<TKey>.Default.GetHashCode(pair.Key),
							EqualityComparer<TValue>.Default.GetHashCode(pair.Value)));
				}

				return hashCode;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetCombinedHashCode<T>(this IList<T> obj, Func<T, int> getHashCode) =>
			GetCombinedHashCodeForIList(obj, getHashCode ?? (x => EqualityComparer<T>.Default.GetHashCode(x)));

		private static int GetCombinedHashCodeForIList<TElement>(IList<TElement> collection,
			Func<TElement, int> getHashCodeMethod)
		{
			if (collection == null) throw new ArgumentNullException(nameof(collection));
			if (getHashCodeMethod == null) throw new ArgumentNullException(nameof(getHashCodeMethod));

			unchecked
			{
				int hashCode = HashHelper.HashSeed;

				for (var i = 0; i < collection.Count; i++)
				{
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, getHashCodeMethod(collection[i]));
				}

				return hashCode;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetCombinedHashCode<T>(this IList<T> obj)
		{
			unchecked
			{
				int hashCode = HashHelper.HashSeed;

				var equalityComparer = EqualityComparer<T>.Default;

				for (var i = 0; i < obj.Count; i++)
				{
					hashCode = HashHelper.CombineHashCodes(hashCode * 397, equalityComparer.GetHashCode(obj[i]));
				}

				return hashCode;
			}
		}
	}
	public sealed class TestEntityEqualityComparer : IEqualityComparer<TestEntity>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool Equals<T>(List<T> x, List<T> y, IEqualityComparer<T> comparer)
		{
			if (x != null)
			{
				if (y != null)
				{
					if (ReferenceEquals(x, y)) return true;
					if (x.Count != y.Count) return false;

					for (int i = 0; i < x.Count; i++)
					{
						if (!comparer.Equals(x[i], y[i])) return false;
					}

					return true;
				}
			}

			return y == null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool Equals<T>(List<T> x, List<T> y)
		{
			if (x != null)
			{
				if (y != null)
				{
					if (ReferenceEquals(x, y)) return true;
					if (x.Count != y.Count) return false;

					var equalityComparer = EqualityComparer<T>.Default;

					for (int i = 0; i < x.Count; i++)
					{
						if (!equalityComparer.Equals(x[i], y[i])) return false;
					}

					return true;
				}
			}

			return y == null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool Equals<TKey, TValue, TDictionary>(TDictionary x, TDictionary y)
			where TDictionary: IReadOnlyDictionary<TKey, TValue>
		{
			if (x != null)
			{
				if (y != null)
				{
					if (ReferenceEquals(x, y)) return true;
					if (x.Count != y.Count) return false;

					var valueEqualityComparer = EqualityComparer<TValue>.Default;

					foreach (var xPair in x)
					{
						if (!y.TryGetValue(xPair.Key, out var yValue) || !valueEqualityComparer.Equals(xPair.Value, yValue))
						{
							return false;
						}
					}

					return true;
				}
			}

			return y == null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool Equals<T>(T[] x, T[] y)
		{
			if (x != null)
			{
				if (y != null)
				{
					if (ReferenceEquals(x, y)) return true;
					if (x.Length != y.Length) return false;

					var equalityComparer = EqualityComparer<T>.Default;

					for (int i = 0; i < x.Length; i++)
					{
						if (!equalityComparer.Equals(x[i], y[i])) return false;
					}

					return true;
				}
			}

			return y == null;
		}

		private static bool Equals<T>(T? x, T? y)
			where T: struct
		{
			return EqualityComparer<T?>.Default.Equals(x, y);
		}

		public bool Equals(TestEntity x, TestEntity y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (ReferenceEquals(x, null)) return false;
			if (ReferenceEquals(y, null)) return false;
			if (x.GetType() != y.GetType()) return false;

			return x.Integer == y.Integer && Equals(x.Ints, y.Ints) && x.NullableInt == y.NullableInt &&
			       Equals(x.NullableInts, y.NullableInts) && x.Byte == y.Byte && Equals(x.Bytes, y.Bytes) &&
			       x.NullableByte == y.NullableByte && Equals(x.NullableBytes, y.NullableBytes) && x.Bool == y.Bool &&
			       Equals(x.Bools, y.Bools) && x.NullableBool == y.NullableBool &&
			       Equals(x.NullableBools, y.NullableBools) && x.Float.Equals(y.Float) && Equals(x.Floats, y.Floats) &&
			       Equals(x.NullableFloat, y.NullableFloat) && Equals(x.NullableFloats, y.NullableFloats) &&
			       x.Double.Equals(y.Double) && Equals(x.Doubles, y.Doubles) &&
			       Equals(x.NullableDouble, y.NullableDouble) && Equals(x.NullableDoubles, y.NullableDoubles) &&
			       x.Decimal == y.Decimal && Equals(x.Decimals, y.Decimals) && x.NullableDecimal == y.NullableDecimal &&
			       Equals(x.NullableDecimals, y.NullableDecimals) && x.Guid.Equals(y.Guid) &&
			       Equals(x.Guids, y.Guids) && Equals(x.NullableGuid, y.NullableGuid) &&
			       Equals(x.NullableGuids, y.NullableGuids) && x.DateTime.Equals(y.DateTime) &&
			       Equals(x.DateTimes, y.DateTimes) && Equals(x.NullabelDateTime, y.NullabelDateTime) &&
			       Equals(x.NullabelDateTimes, y.NullabelDateTimes) && x.TimeSpan.Equals(y.TimeSpan) &&
			       Equals(x.TimeSpans, y.TimeSpans) && Equals(x.NullableTimeSpan, y.NullableTimeSpan) &&
			       Equals(x.NullableTimeSpans, y.NullableTimeSpans) && x.DateTimeOffset.Equals(y.DateTimeOffset) &&
			       Equals(x.DateTimeOffsets, y.DateTimeOffsets) &&
			       Equals(x.NullableDateTimeOffset, y.NullableDateTimeOffset) &&
			       Equals(x.NullableDateTimeOffsets, y.NullableDateTimeOffsets) && string.Equals(x.String, y.String, StringComparison.Ordinal) &&
			       Equals(x.Strings, y.Strings) && x.ByteEnum == y.ByteEnum && Equals(x.ByteEnums, y.ByteEnums) &&
			       x.NullableByteEnum == y.NullableByteEnum && Equals(x.NullableByteEnums, y.NullableByteEnums) &&
			       x.Int16Enum == y.Int16Enum && Equals(x.Int16Enums, y.Int16Enums) &&
			       x.NullableInt16Enum == y.NullableInt16Enum && Equals(x.NullableInt16Enums, y.NullableInt16Enums) &&
			       x.Int32Enum == y.Int32Enum && Equals(x.Int32Enums, y.Int32Enums) &&
			       x.NullableInt32Enum == y.NullableInt32Enum && Equals(x.NullableInt32Enums, y.NullableInt32Enums) &&
			       x.Int64Enum == y.Int64Enum && Equals(x.Int64Enums, y.Int64Enums) &&
			       x.NullableInt64Enum == y.NullableInt64Enum && Equals(x.NullableInt64Enums, y.NullableInt64Enums) &&
			       x.UserDefinedStruct.Equals(y.UserDefinedStruct) &&
			       Equals(x.UserDefinedStructs, y.UserDefinedStructs) &&
			       Equals(x.NullableUserDefinedStruct, y.NullableUserDefinedStruct) &&
			       Equals(x.NullableUserDefinedStructs, y.NullableUserDefinedStructs) &&
				   // avoid circular references if you don't like StackOverflowException
				   Equals(x.Parent, y.Parent) &&
			       Equals(x.Children, y.Children) && OtherEntityEqualityComparer.Instance.Equals(x.OtherEntity, y.OtherEntity) &&
			       Equals(x.OtherEntities, y.OtherEntities, OtherEntityEqualityComparer.Instance)
			       //&& Equals<string, int, Dictionary<string, int>>(x.StringIntDictionary, y.StringIntDictionary)
				;
		}

		public int GetHashCode(TestEntity obj)
		{
			unchecked
			{
				var hashCode = obj.Integer;
				hashCode = (hashCode * 397) ^ (obj.Ints != null ? obj.Ints.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableInt.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableInts != null ? obj.NullableInts.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.Byte.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.Bytes != null ? obj.Bytes.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableByte.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableBytes != null ? obj.NullableBytes.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.Bool.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.Bools != null ? obj.Bools.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableBool.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableBools != null ? obj.NullableBools.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.Float.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.Floats != null ? obj.Floats.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableFloat.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableFloats != null ? obj.NullableFloats.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.Double.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.Doubles != null ? obj.Doubles.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableDouble.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableDoubles != null ? obj.NullableDoubles.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.Decimal.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.Decimals != null ? obj.Decimals.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableDecimal.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableDecimals != null ? obj.NullableDecimals.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.Guid.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.Guids != null ? obj.Guids.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableGuid.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableGuids != null ? obj.NullableGuids.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.DateTime.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.DateTimes != null ? obj.DateTimes.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullabelDateTime.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullabelDateTimes != null ? obj.NullabelDateTimes.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.TimeSpan.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.TimeSpans != null ? obj.TimeSpans.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableTimeSpan.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableTimeSpans != null ? obj.NullableTimeSpans.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.DateTimeOffset.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.DateTimeOffsets != null ? obj.DateTimeOffsets.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableDateTimeOffset.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableDateTimeOffsets != null ? obj.NullableDateTimeOffsets.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ (obj.String != null ? obj.String.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (obj.Strings != null ? obj.Strings.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int)obj.ByteEnum;
				hashCode = (hashCode * 397) ^ (obj.ByteEnums != null ? obj.ByteEnums.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableByteEnum.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableByteEnums != null ? obj.NullableByteEnums.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int)obj.Int16Enum;
				hashCode = (hashCode * 397) ^ (obj.Int16Enums != null ? obj.Int16Enums.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableInt16Enum.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableInt16Enums != null ? obj.NullableInt16Enums.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int)obj.Int32Enum;
				hashCode = (hashCode * 397) ^ (obj.Int32Enums != null ? obj.Int32Enums.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableInt32Enum.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableInt32Enums != null ? obj.NullableInt32Enums.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ ((long) obj.Int64Enum).GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.Int64Enums != null ? obj.Int64Enums.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableInt64Enum.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableInt64Enums != null ? obj.NullableInt64Enums.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.UserDefinedStruct.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.UserDefinedStructs != null ? obj.UserDefinedStructs.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.NullableUserDefinedStruct.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.NullableUserDefinedStructs != null ? obj.NullableUserDefinedStructs.GetCombinedHashCode() : 0);
				hashCode = (hashCode * 397) ^ (obj.Parent != null ? obj.Parent.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (obj.Children != null ? obj.Children.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (obj.OtherEntity != null ? OtherEntityEqualityComparer.Instance.GetHashCode(obj.OtherEntity) : 0);
				hashCode = (hashCode * 397) ^ (obj.OtherEntities != null ? obj.OtherEntities.GetCombinedHashCode(x => OtherEntityEqualityComparer.Instance.GetHashCode(x)) : 0);
				//hashCode = (hashCode * 397) ^ (obj.StringIntDictionary != null ? obj.StringIntDictionary.GetDictionaryHashCode() : 0);
				return hashCode;
			}
		}
	}
}